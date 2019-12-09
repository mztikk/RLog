using System;
using Microsoft.Extensions.Logging;

namespace RLog.Outputs.Console
{
    public class ConsoleOutput : ILogOutput
    {
        private readonly LogLevel _minLevel;
        private readonly bool _colorize;

        public ConsoleOutput(LogLevel minLevel, bool colorize)
        {
            _minLevel = minLevel;
            _colorize = colorize;
        }

        public void Write(LogLevel logLevel, LogContext? logContext, string msg)
        {
            if (logLevel >= _minLevel)
            {
                if (_colorize)
                {
                    string levelString = Logger.GetLogLevelString(logLevel);
                    int logLevelIndex = msg.IndexOf(levelString);
                    if (logLevelIndex == -1)
                    {
                        PlainWrite(msg);
                        return;
                    }
                    string pre = msg.Substring(0, logLevelIndex);
                    int start = logLevelIndex + levelString.Length;
                    int end = msg.Length - start;
                    string post = msg.Substring(start, end);
                    System.Console.Write(pre);
                    ConsoleColors colors = GetLogLevelConsoleColors(logLevel);

                    if (colors.Background.HasValue)
                    {
                        System.Console.BackgroundColor = colors.Background.Value;
                    }
                    if (colors.Foreground.HasValue)
                    {
                        System.Console.ForegroundColor = colors.Foreground.Value;
                    }

                    System.Console.Write(levelString);

                    System.Console.ResetColor();

                    System.Console.WriteLine(post);
                }
                else
                {
                    PlainWrite(msg);
                }
            }
        }

        private void PlainWrite(string msg) => System.Console.WriteLine(msg);

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLevel;

        private ConsoleColors GetLogLevelConsoleColors(LogLevel logLevel) => logLevel switch
        {
            LogLevel.Critical => new ConsoleColors(ConsoleColor.White, ConsoleColor.Red),
            LogLevel.Error => new ConsoleColors(ConsoleColor.Black, ConsoleColor.Red),
            LogLevel.Warning => new ConsoleColors(ConsoleColor.Yellow, ConsoleColor.Black),
            LogLevel.Information => new ConsoleColors(ConsoleColor.DarkGreen, ConsoleColor.Black),
            LogLevel.Debug => new ConsoleColors(ConsoleColor.Gray, ConsoleColor.Black),
            LogLevel.Trace => new ConsoleColors(ConsoleColor.Gray, ConsoleColor.Black),
            _ => throw new ArgumentOutOfRangeException(),
        };

        private readonly struct ConsoleColors
        {
            public ConsoleColors(ConsoleColor? foreground, ConsoleColor? background)
            {
                Foreground = foreground;
                Background = background;
            }

            public ConsoleColor? Foreground { get; }

            public ConsoleColor? Background { get; }
        }
    }
}
