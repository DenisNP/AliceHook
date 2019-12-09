using System.Collections.Generic;
using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierList : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "список",
            "все вебхуки",
            "все веб хуки",
            "все вебхук и",
            "все webhook и",
            "все вебхук",
            "все webhook"
        };
        
        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            if (state.Step == Step.AwaitForKeyword)
            {
                return new SimpleResponse
                {
                    Text = "Эта ключевая фраза пересекается с одной из команд данного навыка. Выберите, " +
                           "пожалуйста, другую.",
                    Buttons = new []{ "Отмена", "Помощь", "Выход" }
                };
            }
            
            if (state.User.Webhooks.Count == 0)
            {
                return new SimpleResponse
                {
                    Text = "У вас пока нет вебхуков. Самое время добавить.",
                    Tts = "У вас пока нет вэбх+уков. Самое время добавить.",
                    Buttons = state.User.Token.IsNullOrEmpty()
                        ? new []{ "Добавить вебхук", "Авторизация", "Помощь", "Выход" }
                        : new []{ "Добавить вебхук", "Помощь", "Выход" }
                };
            }

            var removeFirst = $"Удалить {state.User.Webhooks.First().Phrase.CapitalizeFirst()}";
            var list = state.User.Webhooks.Select(w => "• " + w.Phrase + ": " + w.Url);

            if (request.HasScreen())
            {
                return new SimpleResponse
                {
                    Text = $"Вывела на экран ваши вебхуки:\n\n {list.Join("\n")}\n\nДля удаления скажите" +
                           $" \"Удалить\" и ключевую фразу. Например: \"{removeFirst}\"",

                    Tts =
                        $"Вывела на экран ваши вэбх+уки. Для удаления скаж+ите - - \"Удалить\" - - и ключевую фразу. " +
                        $"Например - - {removeFirst}",

                    Buttons = new[] {removeFirst, "Помощь", "Выход"}
                };
            }
            
            // no screen
            var wCount = state.User.Webhooks.Count.ToPhrase(
                "ключевая фраза",
                "ключевые фразы",
                "ключевых фраз"
            );
            var tts = $"У вас {wCount}: " + state.User.Webhooks.Select(w => w.Phrase).Join(" - - ");
            return new SimpleResponse
            {
                Text = tts,
                Tts = tts
            };
        }
        
        protected override bool CheckState(State state)
        {
            return state.Step == Step.None || state.Step == Step.AwaitForKeyword;
        }
    }
}