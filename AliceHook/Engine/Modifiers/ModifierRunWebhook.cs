using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierRunWebhook : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            return state.Step == Step.None && GetWebhook(request, state) != null;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            var webhook = GetWebhook(request, state);
            using var client = new HttpClient();
            var data = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"value1", request.Request.Command}
            });
            var httpResponseMessage = client.PostAsync(webhook.Url, data).Result;

            return new SimpleResponse
            {
                Text = "Выполнено!"
            };
        }

        private Webhook GetWebhook(AliceRequest request, State state)
        {
            var requestCommand = request.Request.Command.ToLower().Trim();
            return state.User.Webhooks.FirstOrDefault(w => requestCommand.StartsWith(w.Phrase));
        }
    }
}