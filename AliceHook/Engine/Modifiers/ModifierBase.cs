using System.Linq;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public abstract class ModifierBase
    {
        public bool Run(AliceRequest request, State state, out AliceResponse response)
        {
            if (!Check(request, state))
            {
                response = null;
                return false;
            }
            
            response = CreateResponse(request, state);
            return true;
        }

        protected  virtual AliceResponse CreateResponse(AliceRequest request, State state)
        {
            var response = new AliceResponse(request);
            var simple = Respond(request, state);

            if (simple.IsAuthorize)
            {
                return response.ToAuthorizationResponse();
            }

            response.Response.Text = simple.Text;
            response.Response.Tts = string.IsNullOrEmpty(simple.Tts) ? simple.Text : simple.Tts;
            if (simple.Buttons != null)
            {
                response.Response.Buttons = simple.Buttons.Select(t => new Button {Title = t}).ToList();
            }

            return response;
        }

        protected abstract bool Check(AliceRequest request, State state);
        protected abstract SimpleResponse Respond(AliceRequest request, State state);
    }
}