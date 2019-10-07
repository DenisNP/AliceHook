using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace AliceHook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartServer();
        }

        private static void StartServer()
        {
            new WebHostBuilder()
                .UseKestrel()
#if DEBUG
                .ConfigureLogging(logging =>
                {
                    logging.AddDebug();
                    logging.AddConsole();
                })
#endif                           
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}