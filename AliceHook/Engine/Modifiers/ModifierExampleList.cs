using System.Collections.Generic;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierExampleList : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "пример",
            "например"
        };
        
        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            return new SimpleResponse
            {
                Text = "Вот список некоторых пользовательских сценариев, которые возможны с помощью этого навыка.",
                Buttons = new []{ "Добавить вебхук", "Список вебхуков", "Выход" }
            };
        }

        protected override AliceResponse CreateResponse(AliceRequest request, State state)
        {
            var response = base.CreateResponse(request, state);
            ModifierHelp.AddExamplesTo(response);

            return response;
        }

        protected override bool CheckState(State state)
        {
            return state.Step == Step.None;
        }

        protected override bool Check(AliceRequest request, State state)
        {
            return request.HasScreen() && CheckState(state) && CheckTokens(request);
        }
    }
}