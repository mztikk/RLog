using System;
using RFReborn;

namespace RLog
{
    public class LogContext
    {
        public readonly string Context;

        private readonly StringParameterizer _parameterizer;

        public LogContext(string context)
        {
            Context = context;

            _parameterizer = new StringParameterizer();
        }

        public bool TryAddParameter(string parameter, Func<string> parameterValue) => _parameterizer.TryAdd(parameter, parameterValue);
        public void AddParameter(string parameter, Func<string> parameterValue) => _parameterizer.Add(parameter, parameterValue);
        public void SetParameter(string parameter, Func<string> parameterValue) => _parameterizer[parameter] = parameterValue;

        public string Format(string input) => _parameterizer.Make(input);
    }
}
