using System.Threading.Tasks;
using DotLogix.Core.Collections;

namespace DotLogix.Core.Services.Commands;

public interface ICommandProcessor {
    IKeyedCollection<string, IConsoleCommand> Commands { get; }
    Task ProcessAsync(string text);
}