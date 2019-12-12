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
            },
            NullValueHandling = NullValueHandling.Ignore
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
            if (aliceRequest.IsPing())
            {
                var pongResponse = new AliceResponse(aliceRequest).ToPong();
                var stringPong = JsonConvert.SerializeObject(pongResponse, ConverterSettings);
                return Response.WriteAsync(stringPong);
            }
            
            var userId = aliceRequest.Session.UserId;
            var token = ExtractToken(Request);

            Console.WriteLine(JsonConvert.SerializeObject(aliceRequest, ConverterSettings));

            if (token.IsNullOrEmpty() && !aliceRequest.HasScreen())
            {
                // auth needed
                var authResponse = new AliceResponse(aliceRequest).ToAuthorizationResponse();
                var stringAuthResponse = JsonConvert.SerializeObject(authResponse, ConverterSettings);
                return Response.WriteAsync(stringAuthResponse);
            } 
            
            var session = GetOrCreateSession(userId, token, aliceRequest.HasScreen());
            var aliceResponse = session.HandleRequest(aliceRequest);
            var stringResponse = JsonConvert.SerializeObject(aliceResponse, ConverterSettings);

            Console.WriteLine(stringResponse);
            
            return Response.WriteAsync(stringResponse);
        }

        private static UserSession GetOrCreateSession(string uid, string token, bool hasScreen)
        {
            UserSession s = null;
            if (Sessions.ContainsKey(uid))
            {
                s = Sessions[uid];
                // check token
                if (s.Token != token/* && !hasScreen*/)
                {
                    Sessions.TryRemove(uid, out _);
                    s = null;
                }
            }

            if (s == null)
            {
                s = new UserSession(uid, token, hasScreen);
                Sessions.TryAdd(uid, s);
            }

            return s;
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
        
        private static string ExtractToken(HttpRequest request)
        {
            var token = request.Headers.ContainsKey("Authorization")
                ? request.Headers["Authorization"].ToString()
                : "";

            if (!token.IsNullOrEmpty())
            {
                token = token.Replace("Bearer ", "");
            }

            return token;
        }

        private static void RemoveOldSessions(object state)
        {
            var sessionsToRemove = Sessions.Where(s => s.Value.IsOld()).ToList();
            sessionsToRemove.ForEach(s => { Sessions.TryRemove(s.Key, out _); });
        }
    }
}