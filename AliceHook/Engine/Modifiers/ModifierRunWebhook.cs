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

        protected override AliceResponse CreateResponse(AliceRequest request, State state)
        {
            var response = base.CreateResponse(request, state);
            response.Response.EndSession = state.Step != Step.AwaitWebhookResponse && request.IsOutsideCommand();
            return response;
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            var webhook = GetWebhook(request, state);
            var tokens = request.Request.Command.ToLower().Split(" ");
            var skipCount = Utils.OptimalSkipLength(webhook.Phrase, tokens);
            var textToSend = tokens.Skip(skipCount).Join(" ").CapitalizeFirst();

            state.ClearLastResult();

            Task.Run(() =>
            {
                var localStarted = DateTime.Now;
                using var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback 
                        = (message, certificate2, arg3, arg4) => true
                };
                using var client = new HttpClient(handler);
                var data = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "value1", textToSend },
                    { "value2", request.Request.Command.CapitalizeFirst() }, // full command
                    { "value3", webhook.Phrase.ToLower() }
                });

                try
                {
                    var httpResponse = client.PostAsync(webhook.Url, data).Result;
                    var body = httpResponse.Content.ReadAsStringAsync().Result;
                    state.LastResult = body.IsNullOrEmpty() ? "Выполнено!" : $"{body}";
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
                if (state.HasLastResult() ||  diff > new TimeSpan(0, 0, 0, 1200))
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

            state.Step = Step.AwaitWebhookResponse;
            
            // weebhook too long
            return new SimpleResponse
            {
                Text = "Вебхук отвечает очень долго. Попробовать узнать результат сейчас?",
                Tts = "Вэбх+ук отвечает очень долго. Попробовать узнать результат сейчас?",
                Buttons = new[] {"Да", "Нет"}
            };
        }

        private Webhook GetWebhook(AliceRequest request, State state)
        {
            var requestCommand = request.Request.Command.ToLower().Trim();
            return state.User.FindWebhook(requestCommand.Replace(" ", ""));
        }
    }
}