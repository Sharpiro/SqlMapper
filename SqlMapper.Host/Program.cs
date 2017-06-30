using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SqlMapper.Host
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
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
                    webApp.Start();
                    var appLifeTime = webApp.Services.GetRequiredService<IApplicationLifetime>();
                    appLifeTime.ApplicationStopping.WaitHandle.WaitOne();
                }

                return 0;
            });

            cmdApp.Execute();
        }
    }
}