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
        private ILogDistributor _logDistributor = null;

        public RLogConfigurator SetLoglevel(LogLevel logLevel)
        {
            _logLevel = logLevel;
            return this;
        }

        public RLogConfigurator AddConsoleOutput(LogLevel logLevel) => AddOutput(new ConsoleOutput(logLevel));

        public RLogConfigurator AddConsoleOutput() => AddConsoleOutput(_logLevel);

        public RLogConfigurator AddStaticFileOutput(LogLevel logLevel, string logPath) => AddOutput(new StaticFileOutput(logPath, logLevel));

        public RLogConfigurator AddStaticFileOutput(string logPath) => AddStaticFileOutput(_logLevel, logPath);

        public RLogConfigurator AddFileOutput(LogLevel logLevel, string logPath) => AddOutput(new FileOutput(logPath, logLevel));

        public RLogConfigurator AddFileOutput(string logPath) => AddFileOutput(_logLevel, logPath);

        public RLogConfigurator AddOutput(ILogOutput logOutput)
        {
            _logOutputs.Add(logOutput);
            return this;
        }

        public ILogDistributor GetLogDistributor() => _logDistributor ?? new SerialDistributor(_logOutputs);
    }
}
