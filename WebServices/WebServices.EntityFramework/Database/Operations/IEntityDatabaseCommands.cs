using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.WebServices.EntityFramework.Database; 

public interface IEntityDatabaseCommands {
    Task RunAsync(DatabaseCommand command, CancellationToken cancellationToken = default);
    Task<ICollection<DatabaseCommand>> GetCommandsAsync(CancellationToken cancellationToken = default);
}