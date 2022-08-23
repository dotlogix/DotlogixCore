using System;
using System.Threading.Tasks;
using DotLogix.Core.Utils.Builders;

namespace DotLogix.Core.Services.Commands; 

public interface ICommandBuilder : IBuilder<IConsoleCommand> {
    CommandBuilder UseName(string name);
    CommandBuilder UseDescription(string description);
    CommandBuilder UseHelpText(string helpText);
    CommandBuilder UseCallback(Func<CommandContext, Task> callback);
    CommandBuilder UseCallback(Action<CommandContext> callback);
}