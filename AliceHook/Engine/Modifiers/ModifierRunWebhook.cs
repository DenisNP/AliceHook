using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
            var skipCount = webhook.Phrase.Split(" ").Length;
            var textToSend = request.Request.Nlu.Tokens.Skip(skipCount).Join(" ").CapitalizeFirst();

            state.ClearLastResult();

            Task.Run(() =>
            {
                var localStarted = DateTime.Now;
                using var client = new HttpClient();
                var data = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "value1", textToSend },
                    { "value2", request.Request.Command.CapitalizeFirst() }, // full command
                    { "value3", request.Request.OriginalUtterance }
                });

                try
                {
                    var httpResponse = client.PostAsync(webhook.Url, data).Result;
                    var body = httpResponse.Content.ReadAsStringAsync().Result;
                    state.LastResult = body.IsNullOrEmpty() ? "Выполнено!" : $"Выполнено:\n{body}";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    state.LastError = e.ToString();
                }
                finally
                {
                    var total = DateTime.Now - localStarted;
                    Console.WriteLine($"Webhook \"{webhook.Phrase}\" {webhook.Url} " +
                                      $"was finished in {total.TotalMilliseconds}ms\n");
                }
            });

            var started = DateTime.Now;
            while (true)
            {
                var diff = DateTime.Now - started;
                if (state.HasLastResult() ||  diff > new TimeSpan(0, 0, 0, 1800))
                {
                    break;
                }

                Thread.Sleep(10);
            }
            
            if(!state.LastResult.IsNullOrEmpty())
            {
                return new SimpleResponse
                {
                    Text = state.LastResult,
                    Buttons = new []{ "Список", "Помощь", "Выход" }
                };
            }

            if(!state.LastError.IsNullOrEmpty())
            {
                return new SimpleResponse
                {
                    Text = "С вызовом произошла ошибка.",
                    Buttons = new []{ "Список", "Помощь", "Выход" }
                };
            } 
            
            // weebhook too long
            return new SimpleResponse
            {
                Text = "Вызов запущен в фоне из-за высокой длительности.",
                Buttons = new[] {"Список", "Помощь", "Выход"}
            };
        }

        private Webhook GetWebhook(AliceRequest request, State state)
        {
            var requestCommand = request.Request.Command.ToLower().Trim();
            return state.User.FindWebhook(requestCommand);
        }
    }
}