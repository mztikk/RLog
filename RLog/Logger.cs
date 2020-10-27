using System;
using System.Collections.Generic;
using System.Text;
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

        // spaces after new line for timestamp and log level string indentation (eg. info: )
        public static readonly string s_defaultTemplate = "<{Timestamp HH:mm:ss}> {LogLevel}: {LogContext}[{LogEventID}]" + Environment.NewLine + "                 {LogMessage}";

        public Logger(LogContext globalContext, LogContext? logContext, ILogDistributor logDistributor, string messageTemplate)
        {
            _globalContext = globalContext;
            _logContext = logContext;
            _logDistributor = logDistributor;
            _messageTemplate = messageTemplate;
            _logParameters = CreateLoggerParameters();
        }

        private readonly List<object> _scopeStates = new List<object>();
        private readonly object _stateLock = new object();

        public IDisposable BeginScope<TState>(TState state)
        {
            Action action;
            if (state is { })
            {
                lock (_stateLock)
                {
                    _scopeStates.Add(state);
                }

                action = () =>
                {
                    lock (_stateLock)
                    {
                        _scopeStates.Remove(state);
                    }
                };
            }
            else
            {
                action = () => { };
            }

            return new DisposableAction(action);
        }

        public bool IsEnabled(LogLevel logLevel) => _logDistributor.IsEnabled(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            StringBuilder sb = new StringBuilder();
            lock (_stateLock)
            {
                foreach (object scope in _scopeStates)
                {
                    if (scope.GetType() == typeof(TState))
                    {
                        sb.Append(formatter((TState)scope, exception));
                    }
                    else
                    {
                        sb.Append(scope.ToString());
                    }
                }
            }
            sb.Append(formatter(state, exception));

            // set all parameters specific to a message
            _logParameters["LogLevel"] = () => GetLogLevelString(logLevel);
            _logParameters["LogEventID"] = () => eventId.ToString();
            _logParameters["LogMessage"] = () => sb.ToString();

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
