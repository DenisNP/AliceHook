using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierFinalWebhook : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            return state.Step == Step.AwaitForKeyword && request.HasScreen();
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            var exists = state.User.FindWebhook(
                request.Request.Command
                    .ToLower()
                    .Trim()
                    .Replace(" ", "")
            );
            
            if (exists != null)
            {
                return new SimpleResponse
                {
                    Text = $"У вас уже есть вебхук с похожей ключевой фразой: {exists.Phrase.CapitalizeFirst()}. " +
                           "Назовите другую фразу.",
                    Tts = $"У вас уже есть вэбхук с похожей ключевой фразой - {exists.Phrase} - - Назовите другую фразу.",
                    Buttons = new []{ "Отмена", "Помощь", "Выход" }
                };
            }

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
                       "\", я вызову этот адрес и передам туда весь ваш текст в параметр value1.\n\nЧто делаем дальше?",
                
                Tts = $"Теперь, когда вы скажете фразу, которая начинается на - \"{webhook.Phrase}\" -, я вызову этот " +
                       "адрес и передам туда весь ваш текст в параметр value 1. - - - Что делаем дальше?",
                
                Buttons = state.User.Token.IsNullOrEmpty()
                    ? new []{ "Добавить вебхук", "Список", "Примеры", "Авторизация", "Помощь", "Выход" }
                    : new []{ "Добавить вебхук", "Список", "Примеры", "Помощь", "Выход" }
            };
        }
    }
}