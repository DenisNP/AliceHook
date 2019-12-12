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
            if (state.Step == Step.None)
            {
                if (request.HasScreen())
                {
                    return new SimpleResponse
                    {
                        Text = "Команда не распознана. Вы можете добавить вебхук, посмотреть список ключевых " +
                               "фраз или авторизоваться. Что хотите сделать?",
                        Tts = "Команда не распознана. Вы можете добавить вэбх+ук, посмотреть список ключевых " +
                              "фраз или авторизоваться. Что хотите сделать?",
                        Buttons = new []{ "Добавить вебхук", "Список", "Авторизация", "Помощь", "Выход" }
                    };
                }
                
                // no screen
                return new SimpleResponse
                {
                    Text = "Команда не распознана. Вы можете прослушать список ключевых фраз либо выйти. " +
                           "Что хотите сделать?",
                    Tts = "Команда не распознана. Вы можете прослушать список ключевых фраз либо выйти. " +
                          "Что хотите сделать?"
                };
            }
            return ModifierHelp.GetHelp(state.Step, request.HasScreen());
        }
    }
}