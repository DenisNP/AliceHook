using System;
using System.Collections.Generic;
using System.Linq;
using AliceHook.Engine.Modifiers;
using AliceHook.Models;

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
            new ModifierTestRequest(),
            new ModifierExample(),
            new ModifierExamplesList(),
            new ModifierWebhookResponse(),
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

            Console.WriteLine($"=== create session {userId}, token: {token}\n");
            
            if (!token.IsNullOrEmpty())
            {
                user = FirestoreContext.Me.Get<User>("users", "Token", token);
                Console.WriteLine($"==== token is not null, user found: {user != null}\n");
                if (user != null && user.Id != userId)
                {
                    // user found
                    var userById = FirestoreContext.Me.Get<User>("users", userId);
                    if (userById != null)
                    {
                        Console.WriteLine("==== user id is different, move webhooks to old\n");
                        user.Webhooks.AddRange(userById.Webhooks);
                        FirestoreContext.Me.SetAsync("users", userId, user);
                    }
                }
            }

            if (user == null && hasScreen)
            {
                user = FirestoreContext.Me.Get<User>("users", userId);
                Console.WriteLine($"==== user by token is null, screen, found by id: {user != null}\n");
                
                if (user == null)
                {
                    user = new User {Id = userId, Token = token};
                    FirestoreContext.Me.Create("users", userId, user);
                }
            }
            
            if (user != null && hasScreen && user.Token != token)
            {
                Console.WriteLine($"==== token is different, screen, update to new: {user.Token} > {token}\n");
                user.Token = token;
                FirestoreContext.Me.SetAsync("users", userId, user);
            }

            _state.User = user;
            // Console.WriteLine("User Loaded: " + JsonConvert.SerializeObject(_state.User));
        }

        public AliceResponse HandleRequest(AliceRequest aliceRequest)
        {
            _lastActive = DateTime.Now;
            
            AliceResponse response = null;
            try
            {
                if (!Modifiers.Any(modifier => modifier.Run(aliceRequest, _state, out response))) {
                    throw new NotSupportedException("No default modifier");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ERROR");
                Console.WriteLine(e);
                response = new AliceResponse(aliceRequest)
                {
                    Response = new Response
                    {
                        Text = "Произошла какая-то ошибка на сервере навыка, разработчик уже уведомлён. " +
                               "Приносим извинения."
                    }
                };
                Console.WriteLine("");
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