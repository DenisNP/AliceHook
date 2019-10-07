using System.Collections.Generic;
using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierAddWebhook : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "добав вебхук",
            "новый вебхук",
            "добав webhook",
            "новый webhook",
            "добав веб хук",
            "новый веб хук",
        };

        protected override bool CheckState(State state)
        {
            return state.Step == Step.None;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            state.Step = Step.AwaitForUrl;
            return new SimpleResponse
            {
                Text = "Введи URL вебхука:",
                Tts = "Введи URL вэбх+ука",
                Buttons = new []{ "Отмена", "Помощь", "Выход" }
            };
        }
    }
}