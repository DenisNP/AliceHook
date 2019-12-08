using System.Collections.Generic;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierAuthorize : ModifierBaseKeywords
    {
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

            return new SimpleResponse
            {
                IsAuthorize = true
            };
        }

        protected override List<string> Keywords { get; } = new List<string>
        {
            "авториз",
            "как станц",
            "работа станц",
            "запус станц",
            "запус колонк",
            "как колонк",
            "работа колонк"
        };
        
        protected override bool CheckState(State state)
        {
            return state.Step == Step.None || state.Step == Step.AwaitForKeyword;
        }
    }
}