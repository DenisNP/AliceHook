using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierUnknown : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            return true;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            return new SimpleResponse
            {
                Text = "Не поняла"
            };
        }
    }
}