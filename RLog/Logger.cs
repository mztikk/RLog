using System;
using Microsoft.Extensions.Logging;
using RFReborn;
using RLog.Outputs.Distribution;

namespace RLog
{
    public class Logger : ILogger
    {
        private readonly LogContext _globalContext;
        private readonly LogContext? _logContext;

        private readonly ILogDistributor _logDistributor;
        private readonly string _messageTemplate;
        private readonly StringParameterizer _logParameters;

        // 5 spaces after new line for log level string indentation (eg. info: )
        public static readonly string s_defaultTemplate = "{LogLevel}: {LogContext}[{LogEventID}]" + Environment.NewLine + "      {LogMessage}";

        public Logger(LogContext globalContext, LogContext? logContext, ILogDistributor logDistributor, string messageTemplate)
        {
            _globalContext = globalContext;
            _logContext = logContext;
            _logDistributor = logDistributor;
            _messageTemplate = messageTemplate;
            _logParameters = CreateLoggerParameters();
        }

        public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
        public bool IsEnabled(LogLevel logLevel) => _logDistributor.IsEnabled(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // set all parameters specific to a message
            _logParameters["LogLevel"] = () => GetLogLevelString(logLevel);
            _logParameters["LogEventID"] = () => eventId.ToString();
            _logParameters["LogMessage"] = () => formatter(state, exception);

            string msg = _logParameters.Make(_messageTemplate);

            if (_logContext is { })
            {
                msg = _logContext.Format(msg);
            }

            msg = _globalContext.Format(msg);

            _logDistributor.Push(logLevel, _logContext, msg);
        }

        private StringParameterizer CreateLoggerParameters()
        {
            StringParameterizer rtn = new StringParameterizer();

            // set all parameters used throughout the logger
            rtn["LogContext"] = () => _logContext is null ? string.Empty : _logContext.Context;

            return rtn;
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
