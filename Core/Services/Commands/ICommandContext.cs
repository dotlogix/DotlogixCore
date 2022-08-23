using System;
using System.Collections.Generic;

namespace DotLogix.Core.Services.Commands; 

public interface ICommandContext : IServiceProvider {
    IReadOnlyDictionary<string, IConsoleCommand> AvailableCommands { get; }
    IConsoleCommand Command { get; }
    CommandArgs Arguments { get; }
    bool HasResult { get; }
    int? ExitCode { get; }
    object Result { get; }
    Exception Exception { get; }
    void SetResult(object result);
    void SetException(Exception exception);
    void SetExitCode(int exitCode);
    void Reset();
    void EnableScopedServices();
}