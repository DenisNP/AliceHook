using System.Collections.Generic;
using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierExamplesList : ModifierBaseKeywords
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
            if (state.Step == Step.None && request.HasScreen())
            {
                response.Response.Buttons.AddRange(
                    Example.List().Select(
                        x => new Button
                        {
                            Title = x.Title,
                            Hide = false
                        }
                    )
                );
            }

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