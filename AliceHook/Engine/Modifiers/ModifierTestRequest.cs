using System.Collections.Generic;
using System.Net.Http;
using AliceHook.Models;

namespace AliceHook.Engine.Modifiers
{
    public class ModifierTestRequest : ModifierBase
    {
        protected override bool Check(AliceRequest request, State state)
        {
            if (state.Step != Step.None && !request.Request.Command.StartsWith("http"))
            {
                return false;
            }

            return ModifierAddPhrase.UrlRegex.IsMatch(request.Request.OriginalUtterance);
        }

        protected override SimpleResponse Respond(AliceRequest request, State state)
        {
            var url = request.Request.OriginalUtterance;

            // test post
            using var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback
                    = (message, certificate2, arg3, arg4) => true
            };
            using var client = new HttpClient(handler);
            var data = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"value1", "тестовый запрос — сокращённая фраза"},
                {"value2", "тестовый запрос — полная фраза (ключевая + сокращённая)"}, // full command
                {"value3", "тестовый запрос — ключевая фраза"}
            });

            client.PostAsync(url, data);
            
            // response
            return new SimpleResponse
            {
                Text = "Отправила тестовый запрос на этот адрес. Что теперь?",
                Buttons = new []{ "Добавить вебхук", "Список", /*"Примеры,"*/ "Выход" }
            };
        }
    }
}