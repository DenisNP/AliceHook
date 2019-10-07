using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierFinalWebhook : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            return state.Step == Step.AwaitForKeyword;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            var webhook = new Webhook
            {
                Phrase = request.Request.Command,
                Url = state.TempUrl
            };
            
            using var db = new DatabaseContext();
            state.User.Webhooks.Add(webhook);
            db.Users.Update(state.User);
            db.SaveChanges();

            state.Step = Step.None;
            state.TempUrl = "";

            return new SimpleResponse
            {
                Text = $"Добавлен вебхук {webhook.Url} для ключевой фразы {webhook.Phrase}"
            };
        }
    }
}