using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierEnter : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            return request.Request.Command == "";
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            state.Clear();

            if (state.User.Webhooks != null && state.User.Webhooks.Any())
            {
                return new SimpleResponse
                {
                    Text = "Слушаю",
                    Tts = "Слушаю",
                    Buttons = new []{ "Добавить вебхук", "Список", "Помощь", "Выход" }
                };
            }
            
            return new SimpleResponse
            {
                Text = "Привет, в этом навыке ты можешь добавить вебхуки и вызывать их ключевыми словами. " +
                       "Это нужно для автоматизации через такие сервисы, как IFTTT, Zapier и Integromat.",
                Tts = "Привет, в этом навыке ты можешь добавить вэбхуки и вызывать их ключевыми словами. " +
                      "Это нужно для автоматизации через такие сервисы, как иф три тэ, запиер и интегромат.",
                Buttons = new []{ "Добавить вебхук", "Помощь", "Выход" }
            };
        }
    }
}