using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierEnter : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            return request.Request.Command == "";
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            state.Clear();

            if (state.HasWebhooks())
            {
                return new SimpleResponse
                {
                    Text = "Слушаю",
                    Tts = "Слушаю",
                    Buttons = state.User.Token.IsNullOrEmpty()
                        ? new []{ "Добавить вебхук", "Список", "Примеры", "Авторизация", "Выход" }
                        : new []{ "Добавить вебхук", "Список", "Примеры", "Выход" }
                };
            }
            
            return ModifierHelp.GetHelp(Step.None, request.HasScreen());
        }

        protected override AliceResponse CreateResponse(AliceRequest request, State state)
        {
            var response = base.CreateResponse(request, state);
            if (state.HasWebhooks())
            {
                ModifierHelp.AddExamplesTo(response);
            }

            return response;
        }
    }
}