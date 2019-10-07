using System;
using System.Collections.Generic;
using System.Linq;
using AliceHook.Engine.Modifiers;
using AliceHook.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AliceHook.Engine
{
    public class UserSession
    {
        private static readonly List<ModifierBase> Modifiers = new List<ModifierBase>
        {
            new ModifierEnter(),
            new ModifierAddWebhook(),
            new ModifierAddPhrase(),
            new ModifierFinalWebhook(),
            new ModifierRunWebhook(),
            new ModifierUnknown()
        };
        
        private readonly State _state = new State();

        public UserSession(string userId)
        {
            using var db = new DatabaseContext();
            var user = db.Users.Include(u => u.Webhooks).FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                user = new User
                {
                    Id = userId
                };
                db.Users.Add(user);
                db.SaveChanges();
            }

            _state.User = user;
            Console.WriteLine("User Loaded: " + JsonConvert.SerializeObject(_state.User));
        }

        public AliceResponse HandleRequest(AliceRequest aliceRequest)
        {
            AliceResponse response = null;
            if (!Modifiers.Any(modifier => modifier.Run(aliceRequest, _state, out response))) {
                throw new NotSupportedException("No default modifier");
            }
            return response;
        }
    }
}