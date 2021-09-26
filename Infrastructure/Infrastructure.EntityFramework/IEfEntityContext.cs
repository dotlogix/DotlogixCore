using DotLogix.Infrastructure.EntityFramework.Hooks;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.Infrastructure.EntityFramework {
    /// <summary>
    ///     An interface to represent an entity framework entity context
    /// </summary>
    public interface IEfEntityContext : IEntityContext {
        /// <summary>
        ///     The <see cref="DbContext" /> of the entity context
        /// </summary>
        DbContext DbContext { get; }

        /// <summary>
        ///     The event manager of the entity context
        /// </summary>
        IEfEventManager EventManager { get; }
    }
}