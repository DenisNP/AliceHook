using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AliceHook.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Token { get; set; }
        public List<Webhook> Webhooks { get; set; } = new List<Webhook>();

        public Webhook FindWebhook(string phrase)
        {
            return Webhooks.FirstOrDefault(w =>
            {
                var shorten = w.Phrase.Replace(" ", "");
                var startPhrase = phrase.SafeSubstring(shorten.Length);
                return Utils.LevenshteinRatio(startPhrase, shorten) < 0.15;
            });
        }
    }
}