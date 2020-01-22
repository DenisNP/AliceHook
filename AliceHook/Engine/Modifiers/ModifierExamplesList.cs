using System;
using System.Collections.Generic;
using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierExamplesList : ModifierBaseKeywords
    {

        protected override List<string> Keywords { get; } = new List<string>
        {
            "пример",
            "демо",
            "сценари",
            "например"
        };

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            throw new Exception("Method should not be invoked");
        }

        protected override AliceResponse CreateResponse(AliceRequest request, State state)
        {
            var response = new AliceResponse(request);
            if (state.Step == Step.AwaitForKeyword)
            {
                response.Response.Text = "Эта ключевая фраза пересекается с одной из команд данного навыка. " +
                                         "Выберите, пожалуйста, другую.";
                response.Response.Buttons = new[] {"Отмена", "Помощь", "Выход"}
                    .Select(b => new Button {Title = b}).ToList();
            }
            else
            {
                response.Response.Buttons = new[] {"Добавить вебхук", "Помощь", "Выход"}
                    .Select(b => new Button {Title = b}).ToList();

                response.Response.Text = "Вот список примеров тех возможностей, которые даёт вам навык. " +
                                         "Выберите любой, чтобы узнать подробнее:";

                response.Response.Buttons.AddRange(
                    Example.List().Select(x => new Button {Title = x.Title, Hide = false})
                );

                state.Step = Step.Examples;
            }

            return response;
        }

        protected override bool CheckState(State state)
        {
            return state.Step == Step.None || state.Step == Step.AwaitForKeyword;
        }

        protected override bool Check(AliceRequest request, State state)
        {
            return CheckTokens(request) && CheckState(state) && request.HasScreen();
        }
    }
}