using System;
using DotLogix.Core.Diagnostics;
using NUnit.Framework;

namespace DotLogix.WebServices.Testing; 

public sealed class NUnitLogTarget : ILogTarget {
    private readonly LogMessageFormatter _formatter = new();
    public string Name => nameof(NUnitLogTarget);
    public void Log(LogMessage message) {
        switch(message.LogLevel) {
            case LogLevels.Trace:
            case LogLevels.Debug:
            case LogLevels.Info:
            case LogLevels.Warning:
                _formatter.FormatTo(TestContext.Progress, message);
                return;
            case LogLevels.Error:
            case LogLevels.Critical:
                _formatter.FormatTo(TestContext.Error, message);
                return;
            case LogLevels.Unknown:
            case LogLevels.Off:
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public void Flush() { }
    public void Dispose() { }
}