using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace EntityFramework
{
    class MyLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger();
        }

        public void Dispose() { }
        private class MyLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state) => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                File.AppendAllText("log.txt", "Date & Time: " + DateTime.Now.ToString() + "\n");
                File.AppendAllText("log.txt", formatter(state, exception) + "\n");
                Console.WriteLine(formatter(state, exception));
            }
        }
    }
}
