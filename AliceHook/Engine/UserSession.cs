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
        private DateTime _lastActive;
        private static readonly List<ModifierBase> Modifiers = new List<ModifierBase>
        {
            new ModifierAuthFinished(),
            new ModifierNeedAuth(),
            new ModifierEnter(),
            new ModifierHelp(),
            new ModifierExit(),
            new ModifierList(),
            new ModifierAuthorize(),
            new ModifierCancel(),
            new ModifierDelete(),
            new ModifierAddWebhook(),
            new ModifierAddPhrase(),
            new ModifierFinalWebhook(),
            new ModifierRunWebhook(),
            new ModifierUnknown()
        };
        
        private readonly State _state = new State();

        public UserSession(string userId, string token, bool hasScreen)
        {
            _lastActive = DateTime.Now;
            User user = null;
            using var db = new DatabaseContext();

            Console.WriteLine($"=== create session {userId}, token: {token}\n");
            
            if (!token.IsNullOrEmpty())
            {
                user = db.Users.Include(u => u.Webhooks).FirstOrDefault(u => u.Token == token);
                if (user != null && user.Id != userId)
                {
                    var userById = db.Users.Include(u => u.Webhooks).FirstOrDefault(u => u.Id == userId);
                    if (userById != null)
                    {
                        Console.WriteLine("==== user is different, move webhooks to old\n");
                        user.Webhooks.AddRange(userById.Webhooks);
                        db.Users.Update(user);
                        db.Users.Remove(userById);
                        db.SaveChanges();
                    }
                }
            }

            if (user == null && hasScreen)
            {
                user = db.Users.Include(u => u.Webhooks).FirstOrDefault(u => u.Id == userId);
                Console.WriteLine($"==== user by token is null, screen, found by id: {user == null}\n");
                
                if (user == null)
                {
                    user = new User {Id = userId, Token = token};
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
            else if (hasScreen && user.Token != token)
            {
                Console.WriteLine($"==== token is different, screen, update to new: {user.Token} > {token}\n");
                user.Token = token;
                db.Users.Update(user);
                db.SaveChanges();
            }

            _state.User = user;
            // Console.WriteLine("User Loaded: " + JsonConvert.SerializeObject(_state.User));
        }

        public AliceResponse HandleRequest(AliceRequest aliceRequest)
        {
            _lastActive = DateTime.Now;
            
            AliceResponse response = null;
            if (!Modifiers.Any(modifier => modifier.Run(aliceRequest, _state, out response))) {
                throw new NotSupportedException("No default modifier");
            }
            return response;
        }

        public bool IsOld()
        {
            return (DateTime.Now - _lastActive) > new TimeSpan(1, 0, 0);
        }

        public string Token => _state?.User?.Token ?? "";
    }
}