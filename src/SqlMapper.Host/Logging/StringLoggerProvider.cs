using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace SqlMapper.Host.Logging
{
    public class StringLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new StringLogger();
        }

        public void Dispose() { }
    }

    public class StringLogger : ILogger
    {
        private readonly StringBuilder _source = new StringBuilder();

        public IDisposable BeginScope<TState>(TState state)
        {
            return new FakeDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var text = state.ToString();
            //_source.AppendLine(formatter.)
        }
    }

    public class FakeDisposable : IDisposable
    {
        public void Dispose() { }
    }
}