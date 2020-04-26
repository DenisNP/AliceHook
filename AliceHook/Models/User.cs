using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Firestore;

namespace AliceHook.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Token { get; set; }
        [FirestoreProperty]
        public List<Webhook> Webhooks { get; set; } = new List<Webhook>();

        public Webhook FindWebhook(string phrase)
        {
            return Webhooks.FirstOrDefault(w =>
            {
                var shorten = w.Phrase.Replace(" ", "");
                var startPhrase = phrase.SafeSubstring(shorten.Length);
                return Utils.LevenshteinRatio(startPhrase, shorten) < Utils.PossibleRatio(shorten.Length);
            });
        }
    }
}