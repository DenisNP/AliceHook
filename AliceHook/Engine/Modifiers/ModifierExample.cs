using System;
using System.Collections.Generic;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierExample : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            if (state.Step != Step.None || state.Step != Step.Examples)
            {
                return false;
            }
            
            var example = FindExample(request.Request.Command, state);
            if (example != null)
            {
                return true;
            }
            
            if (state.Step == Step.Examples)
            {
                state.Step = Step.None; // TODO change this behavior, check method shouldn't mutate state
            }

            return false;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            throw new Exception("Method should not be invoked");
        }

        protected override AliceResponse CreateResponse(AliceRequest request, State state)
        {
            var response = new AliceResponse(request);
            var example = FindExample(request.Request.Command, state);
            state.Step = Step.None;

            response.Response.Text = example.Description;
            response.Response.Tts = example.DescriptionTts;
            response.Response.Buttons = new List<Button>
            {
                new Button
                {
                    Title = "Открыть видео",
                    Hide = false,
                    Url = example.Link
                }
            };
            
            return response;
        }

        private Example FindExample(string input, State state)
        {
            foreach (var example in Example.List())
            {
                if (example.Check(input, state.Step == Step.Examples))
                {
                    return example;
                }
            }

            return null;
        } 
    }
}