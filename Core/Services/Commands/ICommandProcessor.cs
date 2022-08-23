using System.Threading.Tasks;

namespace DotLogix.Core.Services.Commands; 

public interface ICommandProcessor {
    Task<int?> ProcessAsync(string text);
}