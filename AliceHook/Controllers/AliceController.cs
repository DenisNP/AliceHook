using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AliceHook.Engine;
using AliceHook.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AliceHook.Controllers
{
    [ApiController]
    [Route("/")]
    public class AliceController : ControllerBase
    {
        private static Timer _timer;
        private static readonly ConcurrentDictionary<string, UserSession> Sessions 
            = new ConcurrentDictionary<string, UserSession>();
        private static readonly JsonSerializerSettings ConverterSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };
        
        [HttpGet]
        public string Get()
        {
            return "It works!";
        }

        [HttpPost]
        public Task Post()
        {
            CreateTimer();
            
            using var reader = new StreamReader(Request.Body);
            var body = reader.ReadToEnd();

            var aliceRequest = JsonConvert.DeserializeObject<AliceRequest>(body, ConverterSettings);
            var userId = aliceRequest.Session.UserId;
            var session = Sessions.GetOrAdd(userId, uid => new UserSession(uid));

            var aliceResponse = session.HandleRequest(aliceRequest);
            var stringResponse = JsonConvert.SerializeObject(aliceResponse, ConverterSettings);
            
            return Response.WriteAsync(stringResponse);
        }

        private static void CreateTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer(
                    RemoveOldSessions,
                    null,
                    TimeSpan.Zero,
                    new TimeSpan(0, 10, 0)
                );
            }
        }

        private static void RemoveOldSessions(object state)
        {
            var sessionsToRemove = Sessions.Where(s => s.Value.IsOld()).ToList();
            sessionsToRemove.ForEach(s => { Sessions.TryRemove(s.Key, out _); });
        }
    }
}