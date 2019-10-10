using System;
using System.Collections.Generic;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierHelp : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "помощ",
            "помог",
            "что ты умеешь",
            "что делать"
        };

        protected override bool CheckState(State state)
        {
            return true;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            return GetHelp(state.Step);
        }

        public static SimpleResponse GetHelp(Step step)
        {
            return step switch
            {
                Step.None => new SimpleResponse
                {
                    Text = "В этом навыке вы можете добавлять URL-адреса, на которые я буду отправлять POST-запросы. " +
                           "Для каждого URL я предложу вам задать ключевую фразу. Сказали фразу — вызвался запрос. " +
                           "Это позволит интегрироваться с сервисами автоматизации и интернета вещей, например IFTTT, " +
                           "Zapier и Integromat.\n\nСейчас вы можете добавить вебхук или посмотреть список всех вебхуков. " +
                           "Что хотите сделать?",
                    
                    Tts = "В этом навыке вы можете добавлять URL-адрес+а, на которые я буду отправлять пост запросы. " +
                          "Для каждого URL я предложу вам задать ключевую фразу. Сказали фразу — вызвался запрос. " +
                          "Это позволит интегрироваться с сервисами автоматизации и интернета вещей, например иф три тэ, " +
                          "Зэпиер и Интегромат - - Сейчас вы можете добавить вэбхук или посмотреть список всех вэбхуков. - - " +
                          "Что хотите сделать?",
                    
                    Buttons = new []{ "Добавить вебхук", "Список", "Выход" }
                },
    
                Step.AwaitForUrl => new SimpleResponse
                {
                    Text = "Отправьте мне корректный адрес URL, и я его запомню. Можно отменить это действие.",
                    Buttons = new []{ "Отмена", "Выход" }
                },
                
                Step.AwaitForKeyword => new SimpleResponse
                {
                    Text = "Отправьте мне ключевую фразу, в дальнейшем я буду вызывать указанный URL, когда " +
                           "вы произнесёте эту фразу.",
                    Buttons = new []{ "Отмена", "Выход" }
                },
                
                _ => throw new ArgumentException("Unknown Step")
            };
        }
    }
}