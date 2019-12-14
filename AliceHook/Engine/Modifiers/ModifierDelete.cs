using System.Linq;
using System.Text.RegularExpressions;
using AliceHook.Models;
using Microsoft.EntityFrameworkCore;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierDelete : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            return state.Step == Step.None 
                   && request.Request.Command.ToLower().StartsWith("удали") 
                   && request.Request.Nlu.Tokens.Count > 1
                   && GetWebhook(request, state) != null;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            var w = GetWebhook(request, state);

            if (w == null)
            {
                return new SimpleResponse
                {
                    Text = "Не могу найти вебхук по вашему запросу. Что хотите сделать сейчас?",
                    Tts = "Не могу найти бэбх+ук по вашему запросу. Что хотите сделать сейчас?",
                    Buttons = state.User.Token.IsNullOrEmpty()
                        ? new []{ "Добавить вебхук", "Список", "Авторизация", "Помощь", "Выход" }
                        : new []{ "Добавить вебхук", "Список", "Помощь", "Выход" }
                };
            }
            
            using var db = new DatabaseContext();
            db.Remove(w);
            state.User.Webhooks.Remove(w);
            db.SaveChanges();

            return new SimpleResponse
            {
                Text = $"Удален вебхук: {w.Phrase.CapitalizeFirst()}. Что теперь?",
                Tts = $"Удалён вэбх+ук: {w.Phrase.CapitalizeFirst()}. Что теперь?",
                Buttons = state.User.Token.IsNullOrEmpty()
                    ? new []{ "Добавить вебхук", "Список", "Авторизация", "Помощь", "Выход" }
                    : new []{ "Добавить вебхук", "Список", "Помощь", "Выход" }
            };
        }

        private Webhook GetWebhook(AliceRequest request, State state)
        {
            var tokens = Regex.Split(request.Request.Command.ToLower(), "\\s+");
            if (tokens.Length <= 1)
            {
                return null;
            }
            var trimmedCommand = tokens.Skip(1).Join("").Trim();
            return state.User.FindWebhook(trimmedCommand);
        }
    }
}