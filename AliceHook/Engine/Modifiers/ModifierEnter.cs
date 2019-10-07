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
            return new SimpleResponse
            {
                Text = "Привет, в этом навыке ты можешь добавиться вебхуки и вызывать их ключевыми словами. " +
                       "Скажи \"Добавить вебхук\""
            };
        }
    }
}