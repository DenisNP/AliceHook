using System.Collections.Generic;
using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public abstract class ModifierBaseKeywords : ModifierBase
    {
        protected abstract List<string> Keywords { get; }
        
        protected abstract bool CheckState(State state);
        
        protected override bool Check(AliceRequest request, State state)
        {
            return CheckState(state) && CheckTokens(request);
        }
        
        private bool CheckTokens(AliceRequest request)
        {
            return CheckTokens(request.Request.Nlu.Tokens, Keywords.ToArray());
        }

        private bool CheckTokens(IEnumerable<string> tokens, params string[] expected)
        {
            return expected.Any(expectedString =>
            {
                var expectedTokens = expectedString.Split(" ");
                return expectedTokens.All(tokens.ContainsStartWith);
            });
        }
    }
}