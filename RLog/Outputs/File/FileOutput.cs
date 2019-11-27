using System.IO;
using Microsoft.Extensions.Logging;

namespace RLog.Outputs.File
{
    public class FileOutput : ILogOutput
    {
        private readonly string _logPath;
        private readonly LogLevel _minLevel;

        public FileOutput(string logPath, LogLevel minLevel)
        {
            _logPath = logPath;
            _minLevel = minLevel;
        }

        public void Write(LogLevel logLevel, LogContext logContext, string msg)
        {
            if (logLevel >= _minLevel)
            {
                WriteToFile(logContext, msg);
            }
        }

        private void WriteToFile(LogContext logContext, string msg)
        {
            string transformedPath = logContext.Format(_logPath);
            using (FileStream file = new FileStream(transformedPath, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine(msg);
                }
            }
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLevel;
    }
}
