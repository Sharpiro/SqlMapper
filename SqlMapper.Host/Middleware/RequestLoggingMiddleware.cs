using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SqlMapper.Host.Middleware
{
    class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Stopwatch _stopWatch = new Stopwatch();

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            _stopWatch.Start();
            await _next(context);
            var message = $"'{context.Request.Path}' completed after '{_stopWatch.ElapsedMilliseconds}' ms";
            _stopWatch.Reset();
            Console.WriteLine(message);
        }
    }
}