using System;
using Microsoft.Extensions.Logging;
using RFReborn;
using RLog.Outputs.Distribution;

namespace RLog
{
    public class Logger : ILogger
    {
        private readonly GlobalContext _globalContext;
        private readonly LogContext _logContext;

        private readonly ILogDistributor _logDistributor;
        private readonly string _messageTemplate;
        private StringParameterizer _logParameters;

        public const string DefaultTemplate = "{LogLevel}: {LogContext}[{LogEventID}] {LogMessage}";

        public Logger(GlobalContext globalContext, LogContext logContext, ILogDistributor logDistributor, string messageTemplate)
        {
            _globalContext = globalContext;
            _logContext = logContext;
            _logDistributor = logDistributor;
            _messageTemplate = messageTemplate;
            CreateLoggerParameters();
        }

        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
        public bool IsEnabled(LogLevel logLevel) => _logDistributor.IsEnabled(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // set all parameters specific to a message
            _logParameters["LogLevel"] = () => GetLogLevelString(logLevel);
            _logParameters["LogEventID"] = () => eventId.ToString();
            _logParameters["LogMessage"] = () => formatter(state, exception);

            string context = _logContext.Format(_logParameters.Make(_messageTemplate));
            string msg = _globalContext.Format(context);
            _logDistributor.Push(logLevel, _logContext, msg);
        }

        private void CreateLoggerParameters()
        {
            if (_logParameters is null)
            {
                _logParameters = new StringParameterizer();
            }

            // set all parameters used throughout the logger
            _logParameters["LogContext"] = () => _logContext.Context;
        }

        public static void AddParameterForContext<Context>(string parameterName, Func<string> parameterValue) => LogContextProvider.Instance.CreateLogContext(typeof(Context).ToString()).AddParameter(parameterName, parameterValue);
        public static void TryAddParameterForContext<Context>(string parameterName, Func<string> parameterValue) => LogContextProvider.Instance.CreateLogContext(typeof(Context).ToString()).TryAddParameter(parameterName, parameterValue);
        public static void SetParameterForContext<Context>(string parameterName, Func<string> parameterValue) => LogContextProvider.Instance.CreateLogContext(typeof(Context).ToString()).SetParameter(parameterName, parameterValue);

        public static string GetLogLevelString(LogLevel logLevel) => logLevel switch
        {
            LogLevel.Trace => "trce",
            LogLevel.Debug => "dbug",
            LogLevel.Information => "info",
            LogLevel.Warning => "warn",
            LogLevel.Error => "fail",
            LogLevel.Critical => "crit",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel)),
        };
    }
}
