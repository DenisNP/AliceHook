using System.Linq;
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
            
            using var db = new DatabaseContext();
            db.Remove(w);
            state.User.Webhooks.Remove(w);
            db.SaveChanges();

            return new SimpleResponse
            {
                Text = $"Удален вебхук: {w.Phrase.CapitalizeFirst()}. Что теперь?",
                Tts = $"Удалён вэбх+ук: {w.Phrase.CapitalizeFirst()}. Что теперь?",
                Buttons = new []{ "Добавить вебхук", "Список", "Помощь", "Выход" }
            };
        }

        private Webhook GetWebhook(AliceRequest request, State state)
        {
            var trimmedCommand = request.Request.Nlu.Tokens.Skip(1).Join(" ").ToLower().Trim();
            return state.User.FindWebhook(trimmedCommand);
        }
    }
}