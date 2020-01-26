using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierWebhookResponse : ModifierBase
    {
        private readonly string[] Yes = new[]
        {
            "да",
            "ага",
            "давай",
            "узнай",
            "ок",
            "хорошо",
            "угу"
        };
        
        protected override bool Check(AliceRequest request, State state)
        {
            return state.Step == Step.AwaitWebhookResponse;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            state.Step = Step.None;
            
            var command = request.Request.Command;
            if (Yes.Any(x => command.StartsWith(x)))
            {
                // yes
                if (!state.LastResult.IsNullOrEmpty())
                {
                    return new SimpleResponse
                    {
                        Text = "Вебхук ответил:\n" + state.LastResult,
                        Buttons = new []{ "Список", "Помощь", "Выход" }
                    };
                }

                if (!state.LastError.IsNullOrEmpty())
                {
                    return new SimpleResponse
                    {
                        Text = "К сожалению, с вызовом произошла ошибка. Что теперь?",
                        Buttons = new []{ "Список", "Помощь", "Выход" }
                    };
                }

                return new SimpleResponse
                {
                    Text = "К сожалению, получить ответ от вебхука не удалось. Что теперь?",
                    Buttons = new[] {"Список", "Помощь", "Выход"}
                };
            }

            return new SimpleResponse
            {
                Text = "Ожидание ответа от вебхука прекращено. Что дальше?",
                Buttons = new []{ "Добавить вебхук", "Список", /*"Примеры,"*/ "Выход" }
            };
        }
    }
}