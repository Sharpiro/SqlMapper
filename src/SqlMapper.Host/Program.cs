using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.Console;

namespace SqlMapper.Host
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            WriteLine("Initializing application...");
            var cmdApp = new CommandLineApplication(throwOnUnexpectedArg: false);
            cmdApp.HelpOption("-? | -h | --help");

            var config = new ConfigurationBuilder()
                .AddCommandLine(new[] { "--server.urls", "http://localhost:2000" });

            var builder = new WebHostBuilder()
                .UseConfiguration(config.Build())
                .UseEnvironment("SqlMapper")
                .ConfigureServices(serviceCollection =>
                {

                })
                .UseStartup(typeof(Startup))
                .UseKestrel();

            cmdApp.OnExecute(() =>
            {
                using (var webApp = builder.Build())
                {
                    var address = webApp.ServerFeatures.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
                    webApp.Start();
                    WriteLine($"Server started on port {address}");
                    var appLifeTime = webApp.Services.GetRequiredService<IApplicationLifetime>();
                    appLifeTime.ApplicationStopping.WaitHandle.WaitOne();
                }

                return 0;
            });

            cmdApp.Execute();
        }
    }
}