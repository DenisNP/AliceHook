using System;
using System.Linq;
using AliceHook.Engine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace AliceHook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new DatabaseContext())
            {
                var usersCount = db.Users.Count(); // database warmup
                Console.WriteLine("Users count: " + usersCount);
            }
            StartServer();
        }

        private static void StartServer()
        {
            new WebHostBuilder()
                .UseKestrel()
//#if DEBUG
                .ConfigureLogging(logging =>
                {
                    logging.AddDebug();
                    logging.AddConsole();
                })
//#endif                           
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}