using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace DotLogix.Core.Services.Commands {
    public class CommandContext : IDisposable, ICommandContext {
        private readonly IServiceProvider _serviceProvider;
        private IServiceScope _scope;

        public IReadOnlyDictionary<string, IConsoleCommand> AvailableCommands { get; }
        public IConsoleCommand Command { get; }
        public CommandArgs Arguments { get; }

        public bool HasResult { get; private set; }
        public int? ExitCode { get; private set; }
        public object Result { get; private set; }
        public Exception Exception { get; private set; }

        public CommandContext(IServiceProvider serviceProvider, IReadOnlyDictionary<string, IConsoleCommand> availableCommands, IConsoleCommand command, CommandArgs arguments) {
            _serviceProvider = serviceProvider;
            AvailableCommands = availableCommands;
            Command = command;
            Arguments = arguments;
        }

        public void SetResult(object result) {
            Result = result;
            Exception = null;
            HasResult = true;
        }

        public void SetException(Exception exception) {
            Result = null;
            Exception = exception;
            HasResult = true;
        }

        public void SetExitCode(int exitCode) {
            ExitCode = exitCode;
        }

        public void Reset() {
            Result = null;
            Exception = null;
            ExitCode = null;
            HasResult = false;
        }

        public void EnableScopedServices() {
            _scope ??= _serviceProvider?.CreateScope();
        }

        public object GetService(Type serviceType) {
            var serviceProvider = _scope?.ServiceProvider ?? _serviceProvider;
            return serviceProvider.GetService(serviceType);
        }

        public void Dispose() {
            _scope?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
