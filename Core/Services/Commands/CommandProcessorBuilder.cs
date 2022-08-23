using System;
using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Services.Commands; 

public sealed class CommandProcessorBuilder : ICommandProcessorBuilder {
    private readonly List<CommandBuilder> _commands = new();
    private IServiceProvider _serviceProvider;

    public ICommandProcessorBuilder UseServiceProvider(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        return this;
    }

    public ICommandProcessorBuilder UseCommand(Action<CommandBuilder> configure) {
        var builder = new CommandBuilder();
        configure(builder);
        _commands.Add(builder);
        return this;
    }

    public ICommandProcessor Build() {
        if(_serviceProvider == null) throw new InvalidOperationException("A service provider is required to build a command processor");
        return new CommandProcessor(_serviceProvider, _commands.Select(c => c.Build()));
    }
}