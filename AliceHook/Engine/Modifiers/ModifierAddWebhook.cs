using System.Collections.Generic;
using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierAddWebhook : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "добав вебхук",
            "новый вебхук",
            "добав webhook",
            "новый webhook",
            "добав веб хук",
            "новый веб хук",
        };

        protected override bool CheckState(State state)
        {
            return state.Step == Step.None;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            if (!request.HasScreen())
            {
                return new SimpleResponse
                {
                    Text = "Добавлять вебхуки можно только на устройстве с экраном",
                    Tts = "Добавлять вэбх+уки можно только на устройстве с экраном"
                };
            }
            
            if (state.User.Webhooks.Count >= 20)
            {
                return new SimpleResponse
                {
                    Text = "У вас очень много вебхуков, удалите что-нибудь сначала с помощью команды \"Список\"",
                    Tts = "У вас очень много вэбх+уков, удалите что-нибудь сначала с помощью команды - Список",
                    Buttons = new []{ "Список", "Помощь", "Выход" }
                };
            }
            state.Step = Step.AwaitForUrl;
            return new SimpleResponse
            {
                Text = "Введите URL вебхука:",
                Tts = "Введите URL вэбх+ука",
                Buttons = new []{ "Отмена", "Помощь", "Выход" }
            };
        }
    }
}