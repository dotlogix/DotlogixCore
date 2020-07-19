using System;
using DotLogix.Architecture.Infrastructure.EntityContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    /// <summary>
    /// An interface to represent an entity framework entity context
    /// </summary>
    public interface IEfEntityContext : IEntityContext {
        /// <summary>
        /// The current <see cref="DbContext"/>
        /// </summary>
        DbContext DbContext { get; }

        /// <summary>
        /// The global hooks to the ef store system
        /// </summary>
        EfEntityHooks GlobalHooks { get; }

        /// <summary>
        /// Gets the event hooks for a specific entity type
        /// </summary>
        EfEntityHooks GetEntityHooks<TEntity>() where TEntity : class, new();

        /// <summary>
        /// Gets the event hooks for a specific entity type
        /// </summary>
        EfEntityHooks GetEntityHooks(Type entityType);
    }
}