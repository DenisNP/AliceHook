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
                Text = $"Удален вебхук: {w.Phrase}. Что теперь?",
                Tts = $"Удалён вэбх+ук: {w.Phrase}. Что теперь?",
                Buttons = new []{ "Добавить вебхук", "Список", "Помощь", "Выход" }
            };
        }

        private Webhook GetWebhook(AliceRequest request, State state)
        {
            return state.User.Webhooks.FirstOrDefault(w => request.Request.Command.Contains(w.Phrase));
        }
    }
}