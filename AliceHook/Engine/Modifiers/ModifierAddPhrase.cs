using System.Text.RegularExpressions;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierAddPhrase : ModifierBase
    {
        public static readonly Regex UrlRegex 
            = new Regex(@"^(?:http(s)?:\/\/)[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");
        
        protected override bool Check(AliceRequest request, State state)
        {
            if (state.Step != Step.AwaitForUrl) return false;
            return UrlRegex.IsMatch(request.Request.OriginalUtterance);
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            state.Step = Step.AwaitForKeyword;
            state.TempUrl = request.Request.OriginalUtterance;

            return new SimpleResponse
            {
                Text = "А теперь назовите фразу активации:",
                Buttons = new []{ "Отмена", "Помощь", "Выход" }
            };
        }
    }
}