using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RLog.Outputs;
using RLog.Outputs.Console;
using RLog.Outputs.Distribution;
using RLog.Outputs.File;

namespace RLog
{
    public class RLogConfigurator
    {
        private LogLevel _logLevel = LogLevel.Information;
        private readonly ICollection<ILogOutput> _logOutputs = new List<ILogOutput>();
        private ILogDistributor? _logDistributor;
        private string _messageTemplate = Logger.DefaultTemplate;
        private readonly LogContext _globalContext = new LogContext(Guid.NewGuid().ToString());

        public RLogConfigurator SetLoglevel(LogLevel logLevel)
        {
            _logLevel = logLevel;
            return this;
        }

        public RLogConfigurator SetMessageTemplate(string messageTemplate)
        {
            _messageTemplate = messageTemplate;
            return this;
        }

        public RLogConfigurator AddConsoleOutput(LogLevel logLevel, bool colorize) => AddOutput(new ConsoleOutput(logLevel, colorize));

        public RLogConfigurator AddConsoleOutput(bool colorize = true) => AddConsoleOutput(_logLevel, colorize);

        public RLogConfigurator AddStaticFileOutput(LogLevel logLevel, string logPath) => AddOutput(new StaticFileOutput(_globalContext, logPath, logLevel));

        public RLogConfigurator AddStaticFileOutput(string logPath) => AddStaticFileOutput(_logLevel, logPath);

        public RLogConfigurator AddFileOutput(LogLevel logLevel, string logPath) => AddOutput(new FileOutput(_globalContext, logPath, logLevel));

        public RLogConfigurator AddFileOutput(string logPath) => AddFileOutput(_logLevel, logPath);

        public RLogConfigurator AddOutput(ILogOutput logOutput)
        {
            _logOutputs.Add(logOutput);
            return this;
        }

        public RLogConfigurator WithParameter(string parameter, Func<string> parameterValue)
        {
            _globalContext.SetParameter(parameter, parameterValue);
            return this;
        }

        public ILogDistributor GetLogDistributor() => _logDistributor ?? new SerialDistributor(_logOutputs);

        public string GetMessageTemplate() => _messageTemplate;

        public LogContext GetGlobalContext() => _globalContext;
    }
}
