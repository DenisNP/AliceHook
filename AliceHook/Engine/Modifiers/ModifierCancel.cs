using System.Collections.Generic;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierCancel : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "отмен"
        };

        protected override bool CheckState(State state)
        {
            return state.Step != Step.None;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            state.Clear();
            return new SimpleResponse
            {
                Text = "Отменено. Что теперь?",
                Tts = "Отменено. Что теперь?",
                Buttons = state.User.Token.IsNullOrEmpty()
                    ? new []{ "Добавить вебхук", "Примеры", "Список", "Авторизация", "Помощь", "Выход" }
                    : new []{ "Добавить вебхук", "Примеры", "Список", "Помощь", "Выход" }
            };
        }
    }
}