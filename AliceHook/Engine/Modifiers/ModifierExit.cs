using System.Collections.Generic;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierExit : ModifierBaseKeywords
    {
        protected override List<string> Keywords { get; } = new List<string>
        {
            "выход",
            "выйти",
            "пока"
        };

        protected override bool CheckState(State state)
        {
            return true; // any state
        }

        protected override AliceResponse CreateResponse(AliceRequest request, State state)
        {
            var resp = base.CreateResponse(request, state);
            resp.Response.EndSession = true;
            return resp;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            state.Clear();
            return new SimpleResponse
            {
                Text = "Выхожу. Хорошего дня.",
                Tts = "Выхожу - - хорошего дня"
            };
        }
    }
}