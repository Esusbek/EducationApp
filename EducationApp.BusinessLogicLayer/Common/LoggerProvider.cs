using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace EducationApp.BusinessLogicLayer.Common
{
    public class LoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new Logger();
        }

        public void Dispose()
        {
        }

        private class Logger : Interfaces.ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;

            }

            public void Log<TState>(LogLevel logLevel, EventId eventId,
                    TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                File.AppendAllText("log.txt", formatter(state, exception));
            }
        }
    }
}
