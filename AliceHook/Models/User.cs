using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AliceHook.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public List<Webhook> Webhooks { get; set; } = new List<Webhook>();

        public Webhook FindWebhook(string phrase)
        {
            return Webhooks.FirstOrDefault(w =>
            {
                var startPhrase = phrase.SafeSubstring(w.Phrase.Length);
                return Utils.LevenshteinRatio(startPhrase, phrase) < 0.15;
            });
        }
    }
}