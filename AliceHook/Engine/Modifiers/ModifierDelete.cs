using System.Linq;
using AliceHook.Models;

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
            state.User.Webhooks.Remove(w);
            
            using var db = new DatabaseContext();
            db.Users.Update(state.User);
            db.SaveChanges();

            return new SimpleResponse
            {
                Text = $"Удален вебхук: {w.Phrase}",
                Tts = $"Удалён вэбхук: {w.Phrase}",
                Buttons = new []{ "Добавить вебхук", "Список", "Помощь", "Выход" }
            };
        }

        private Webhook GetWebhook(AliceRequest request, State state)
        {
            return state.User.Webhooks.FirstOrDefault(w => request.Request.Command.Contains(w.Phrase));
        }
    }
}