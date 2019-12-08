using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierNeedAuth : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            return state.User == null;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            return new SimpleResponse
            {
                Text = "Вам нужно авторизоваться под тем же аккаунтом на устройстве с экраном. " +
                       "Скажите \"Выход\" для выхода",
                Tts = "Вам нужно авторизоваться под тем же аккаунтом на устройстве с экраном. " +
                      "Скаж+ите - Выход - для выхода."
            };
        }
    }
}