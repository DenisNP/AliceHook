using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace AliceHook.Engine
{
    public class FirestoreContext
    {
        public static FirestoreContext Me;
        public FirestoreDb Db { get; }

        public FirestoreContext(string credentialsJson)
        {
            // read firestore credentials
            dynamic credentialsObject = JsonConvert.DeserializeObject(credentialsJson);
            if (credentialsObject == null) throw new ArgumentNullException(nameof(credentialsJson));
            
            var projectId = credentialsObject.project_id;

            // create db object
            Db = new FirestoreDbBuilder
            {
                JsonCredentials = credentialsJson,
                ProjectId = projectId
            }.Build();

            Me = this;
        }

        public static void Init(string credentialsJson)
        {
            Me = new FirestoreContext(credentialsJson);
        }

        public T Get<T>(string path, string id)
        {
            var doc = Db.Collection(path).Document(id).GetSnapshotAsync().Result;
            return doc.Exists ? doc.ConvertTo<T>() : default;
        }

        public T Get<T>(string path, string field, object value)
        {
            var snapshot = Db.Collection(path).WhereEqualTo(field, value).GetSnapshotAsync().Result;
            return snapshot.Count == 0 ? default : snapshot.First().ConvertTo<T>();
        }

        private Task<WriteResult> CreateTask(string path, string id, object document)
        {
            return Db.Collection(path).Document(id).CreateAsync(document);
        }

        public WriteResult Create(string path, string id, object document)
        {
            return CreateTask(path, id, document).Result;
        }

        public void CreateAsync(string path, string id, object document)
        {
            CreateTask(path, id, document).ContinueWith(
                t => Console.WriteLine(t.Exception),
                TaskContinuationOptions.OnlyOnFaulted
            );
        }
        
        private Task<WriteResult> SetTask(string path, string id, object document)
        {
            return Db.Collection(path).Document(id).SetAsync(document);
        }

        public WriteResult Set(string path, string id, object document)
        {
            return SetTask(path, id, document).Result;
        }

        public void SetAsync(string path, string id, object document)
        {
            SetTask(path, id, document).ContinueWith(
                t => Console.WriteLine(t.Exception),
                TaskContinuationOptions.OnlyOnFaulted
            );
        }
    }
}