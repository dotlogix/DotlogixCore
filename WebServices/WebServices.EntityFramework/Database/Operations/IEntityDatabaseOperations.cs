using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotLogix.WebServices.EntityFramework.Database {
    public interface IEntityDatabaseOperations {
        Task CreateAsync(CancellationToken cancellationToken = default);
        Task ClearAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(CancellationToken cancellationToken = default);
        Task RecreateAsync(CancellationToken cancellationToken = default);
        
        Task MigrateAsync(CancellationToken cancellationToken = default);
        Task MigrateAsync(DatabaseMigration migration, CancellationToken cancellationToken = default);
        Task<ICollection<DatabaseMigration>> GetMigrationsAsync(CancellationToken cancellationToken = default);
        Task<ICollection<DatabaseMigration>> GetPendingMigrationsAsync(CancellationToken cancellationToken = default);
        Task<ICollection<DatabaseMigration>> GetAppliedMigrationsAsync(CancellationToken cancellationToken = default);

        Task RunCommandAsync(DatabaseCommand command, CancellationToken cancellationToken = default);
        Task<ICollection<DatabaseCommand>> GetCommandsAsync(CancellationToken cancellationToken = default);
    }
}