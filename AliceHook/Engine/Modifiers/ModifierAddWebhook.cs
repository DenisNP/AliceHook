using System.Collections.Generic;
using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierAddWebhook : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            if (state.Step != Step.None) return false;
            
            var keywords = new List<string>
            {
                "добав вебхук",
                "новый вебхук"
            };

            var requestString = request.Request.Nlu.Tokens;
            return keywords.Any(kw =>
            {
                var tokens = kw.Split(" ");
                return tokens.All(requestString.ContainsStartWith);
            });
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            state.Step = Step.AwaitForUrl;
            return new SimpleResponse
            {
                Text = "Введи Url вебхука"
            };
        }
    }
}