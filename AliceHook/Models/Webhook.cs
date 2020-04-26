using Google.Cloud.Firestore;

namespace AliceHook.Models
{
    [FirestoreData]
    public class Webhook
    {
        [FirestoreProperty]
        public int Id { get; set; }
        [FirestoreProperty]
        public string Phrase { get; set; }
        [FirestoreProperty]
        public string Url { get; set; }
    }
}