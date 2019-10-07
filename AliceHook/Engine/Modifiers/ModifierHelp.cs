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
                    Text = "",
                    Tts = "",
                    Buttons = new []{"Добавить вебхук", "Список", "Выход"}
                },
    
                Step.AwaitForUrl => new SimpleResponse
                {
                    Text = "",
                    Tts = "",
                    Buttons = new []{"Отмена", "Выход"}
                },
                
                Step.AwaitForKeyword => new SimpleResponse
                {
                    Text = "",
                    Tts = "",
                    Buttons = new []{"Отмена", "Выход"}
                },
                
                _ => throw new ArgumentException("Unknown Step")
            };
        }
    }
}