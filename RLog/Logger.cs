using System;
using Microsoft.Extensions.Logging;
using RLog.Outputs.Distribution;

namespace RLog
{
    public class Logger : ILogger
    {
        private readonly LogContext _logContext;

        private readonly ILogDistributor _logDistributor;

        public Logger(LogContext logContext, ILogDistributor logDistributor)
        {
            _logContext = logContext;
            _logDistributor = logDistributor;
        }

        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
        public bool IsEnabled(LogLevel logLevel) => _logDistributor.IsEnabled(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string msg = formatter(state, exception);
            string context = _logContext.Format(msg);
            _logDistributor.Push(logLevel, _logContext, context);
        }

        public static void AddParameterForContext<Context>(string parameterName, Func<string> parameterValue) => LogContextProvider.Instance.CreateLogContext(typeof(Context).ToString()).AddParameter(parameterName, parameterValue);
        public static void TryAddParameterForContext<Context>(string parameterName, Func<string> parameterValue) => LogContextProvider.Instance.CreateLogContext(typeof(Context).ToString()).TryAddParameter(parameterName, parameterValue);
        public static void SetParameterForContext<Context>(string parameterName, Func<string> parameterValue) => LogContextProvider.Instance.CreateLogContext(typeof(Context).ToString()).SetParameter(parameterName, parameterValue);
    }
}
