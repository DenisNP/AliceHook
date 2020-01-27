using System;
using System.Collections.Generic;
using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierExample : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            if (state.Step != Step.None)
            {
                return false;
            }
            
            var example = FindExample(request.Request.Command, state);
            if (example != null)
            {
                return true;
            }

            return false;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            throw new NotSupportedException("Method should not be invoked");
        }

        protected override AliceResponse CreateResponse(AliceRequest request, State state)
        {
            var response = new AliceResponse(request);
            var example = FindExample(request.Request.Command, state);
            state.Step = Step.None;

            response.Response.Text = example.Description + " По кнопке подробная видеоинструкция о том, как это сделать.";
            response.Response.Tts = example.DescriptionTts + " - По кнопке подробная видеоинструкция о том, как это сделать.";
            response.Response.Buttons = new List<Button>
            {
                new Button
                {
                    Title = "Открыть видео",
                    Hide = false,
                    Url = example.Link
                }
            };
            
            var suggests = new [] {"Добавить вебхук", "Примеры", "Список", "Выход"};
            response.Response.Buttons.AddRange(suggests.Select(x => new Button{Title = x}));
            
            return response;
        }

        private Example FindExample(string input, State state)
        {
            foreach (var example in Example.List())
            {
                if (example.Check(input))
                {
                    return example;
                }
            }

            return null;
        } 
    }
}