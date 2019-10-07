using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }
        
        protected override bool CheckState(State state)
        {
            return state.Step == Step.None;
        }
    }
}