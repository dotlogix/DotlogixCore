using System;
using DotLogix.Core.Utils.Builders;

namespace DotLogix.Core.Services.Commands {
    public interface ICommandProcessorBuilder : IBuilder<ICommandProcessor> {
        ICommandProcessorBuilder UseServiceProvider(IServiceProvider serviceProvider);
        ICommandProcessorBuilder UseCommand(Action<CommandBuilder> configure);
    }
}
