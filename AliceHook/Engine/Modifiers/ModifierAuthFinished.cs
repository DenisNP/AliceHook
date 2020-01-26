using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierAuthFinished : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            return request.IsAccountLinking();
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            if (state.User == null || state.User.Token.IsNullOrEmpty())
            {
                return new SimpleResponse
                {
                    Text = "Авторизация отменена пользователем. Что хотите сделать сейчас?",
                    Tts = "Авторизация отменена пользователем. Что хотите сделать сейчас?",
                    Buttons = new []{ "Добавить вебхук", "Список", "Авторизация", "Помощь", "Выход" }
                };
            }
            
            if (!request.HasScreen())
            {
                // device without screen
                return new SimpleResponse
                {
                    Text = "Авторизация успешна! Теперь вам нужно авторизоваться под тем же самым аккаунтом на " +
                           "устройстве с экраном, чтобы добавить вебхуки, тогда вы сможете вызывать их с колонки. " +
                           "Для выхода скажите \"Выход\".",
                    Tts = "Авторизация успешна! Теперь вам нужно авторизоваться под тем же самым аккаунтом на " +
                          "устройстве с экраном, чтобы добавить вэбх+уки, тогда вы сможете вызывать их с колонки. " +
                          "Для выхода скаж+ите - Выход.",
                };
            }
            
            // device with screen
            return new SimpleResponse
            {
                Text = "Авторизация успешна! Теперь вы можете войти на колонке с того же самого аккаунта и " +
                       "использовать вебхуки, добавленные здесь. Что хотите сделать сейчас?",
                Tts = "Авторизация успешна! Теперь вы можете войти на колонке с того же самого аккаунта и " +
                      "использовать вэбх+уки, добавленные здесь. Что хотите сделать сейчас?",
                Buttons = state.User.Token.IsNullOrEmpty()
                    ? new []{ "Добавить вебхук", /*"Примеры,"*/ "Список", "Авторизация", "Выход" }
                    : new []{ "Добавить вебхук", /*"Примеры,"*/ "Список", "Выход" }
            };
        }
    }
}