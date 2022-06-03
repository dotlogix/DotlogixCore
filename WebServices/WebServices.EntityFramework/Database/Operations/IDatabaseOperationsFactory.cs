using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Database {
    /// <summary>
    ///     Defines a factory for creating <see cref="IEntityDatabaseOperations" /> instances.
    /// </summary>
    public interface IDatabaseOperationsFactory
    {
        /// <summary>
        /// Creates a new database operation instance
        /// </summary>
        IEntityDatabaseOperations Create(DbContext dbContext);
    }
}