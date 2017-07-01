using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlMapper.Host.Middleware;
using SqlMapper.Host.Logging;

namespace SqlMapper.Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMvc(options => options.MapRoute("Api", "Api/{controller}/{action}"));
        }
    }
}
