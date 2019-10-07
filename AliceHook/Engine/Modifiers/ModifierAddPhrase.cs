using System.Text.RegularExpressions;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierAddPhrase : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            if (state.Step != Step.AwaitForUrl) return false;

            var urlRegex = new Regex(@"^(?:http(s)?:\/\/)[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");
            return urlRegex.IsMatch(request.Request.OriginalUtterance);
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