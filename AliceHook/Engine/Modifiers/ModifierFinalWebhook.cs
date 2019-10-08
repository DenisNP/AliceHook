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

            state.Clear();

            return new SimpleResponse
            {
                Text = $"Теперь, когда вы скажете фразу, которая начинается на \"{webhook.Phrase.CapitalizeFirst()}" +
                       $"\", я вызову этот адрес и передам туда весь ваш текст в параметр value1.\n\nЧто делаем дальше?",
                
                Tts = $"Теперь, когда вы скажете фразу, которая начинается на - \"{webhook.Phrase}\" -, я вызову этот " +
                       $"адрес и передам туда весь ваш текст в параметр value 1. - - - Что делаем дальше?",
                
                Buttons = new []{ "Список", "Помощь", "Выход" }
            };
        }
    }
}