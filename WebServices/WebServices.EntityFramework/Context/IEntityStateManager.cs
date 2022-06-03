using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotLogix.WebServices.EntityFramework.Context; 

public interface IEntityStateManager {
    #region GetEntries
    /// <inheritdoc cref="DbContext.Entry" />
    EntityEntry GetEntry(object entity);

    /// <summary>
    ///     Gets an <see cref="EntityEntry" /> for the entity matching the provided key.
    ///     The entries provide access to change tracking information and operations for each entity.
    /// </summary>
    /// <returns> An entry for the entity matching the provided key. </returns>
    EntityEntry GetEntry(IKey dbKey, object key);

    /// <inheritdoc cref="DbContext.Entry{TEntity}" />
    EntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    ///     Gets an <see cref="EntityEntry{TEntity}" /> for the entity matching the provided key.
    ///     The entries provide access to change tracking information and operations for each entity.
    /// </summary>
    /// <typeparam name="TEntity"> The type of entities to get entries for. </typeparam>
    /// <returns> An entry for the entity matching the provided key. </returns>
    EntityEntry<TEntity> GetEntry<TEntity>(IKey dbKey, object key) where TEntity : class;

    /// <inheritdoc cref="ChangeTracker.Entries{TEntity}" />
    IEnumerable<EntityEntry<TEntity>> GetEntries<TEntity>() where TEntity : class;

    /// <inheritdoc cref="ChangeTracker.Entries" />
    IEnumerable<EntityEntry> GetEntries();

    /// <summary>
    ///     Gets an <see cref="EntityEntry" /> for all entities matching one of the provided keys.
    ///     The entries provide access to change tracking information and operations for each entity.
    /// </summary>
    /// <returns> An entry for each entity matching one of the provided keys. </returns>
    IEnumerable<EntityEntry> GetEntries(IKey dbKey, IEnumerable<object> keys);

    /// <inheritdoc cref="GetEntries(IKey, IEnumerable{object})" />
    IEnumerable<EntityEntry> GetEntries(IKey dbKey, IEnumerable<object> keys, out IReadOnlyCollection<object> missingKeys);

    /// <summary>
    ///     Gets an <see cref="EntityEntry" /> for all entities matching one of the provided keys.
    ///     The entries provide access to change tracking information and operations for each entity.
    /// </summary>
    /// <typeparam name="TKey"> The type of the key to get entries for. </typeparam>
    /// <returns> An entry for each entity matching one of the provided keys. </returns>
    IEnumerable<EntityEntry> GetEntries<TKey>(IKey dbKey, IEnumerable<TKey> keys);

    /// <inheritdoc cref="GetEntries{TKey}(IKey, IEnumerable{TKey})" />
    IEnumerable<EntityEntry> GetEntries<TKey>(IKey dbKey, IEnumerable<TKey> keys, out IReadOnlyCollection<TKey> missingKeys);

    /// <summary>
    ///     Gets an <see cref="EntityEntry{TEntity}" /> for all entities matching one of the provided keys.
    ///     The entries provide access to change tracking information and operations for each entity.
    /// </summary>
    /// <typeparam name="TEntity"> The type of entities to get entries for. </typeparam>
    /// <returns> An entry for each entity matching one of the provided keys.</returns>
    IEnumerable<EntityEntry<TEntity>> GetEntries<TEntity>(IKey dbKey, IEnumerable<object> keys) where TEntity : class;

    /// <inheritdoc cref="GetEntries{TEntity}(IKey, IEnumerable{object})" />
    IEnumerable<EntityEntry<TEntity>> GetEntries<TEntity>(IKey dbKey, IEnumerable<object> keys, out IReadOnlyCollection<object> missingKeys) where TEntity : class;

    /// <summary>
    ///     Gets an <see cref="EntityEntry{TEntity}" /> for all entities matching one of the provided keys.
    ///     The entries provide access to change tracking information and operations for each entity.
    /// </summary>
    /// <typeparam name="TEntity"> The type of entities to get entries for. </typeparam>
    /// <typeparam name="TKey"> The type of the key to get entries for. </typeparam>
    /// <returns> An entry for each entity matching one of the provided keys.</returns>
    IEnumerable<EntityEntry<TEntity>> GetEntries<TKey, TEntity>(IKey dbKey, IEnumerable<TKey> keys) where TEntity : class;

    /// <inheritdoc cref="GetEntries{TKey, TEntity}(IKey, IEnumerable{TKey})" />
    IEnumerable<EntityEntry<TEntity>> GetEntries<TKey, TEntity>(IKey dbKey, IEnumerable<TKey> keys, out IReadOnlyCollection<TKey> missingKeys) where TEntity : class;
    #endregion

    #region MarkModified
    /// <inheritdoc cref="DbContext.Update{TEntity}" />
    EntityEntry MarkModified(object entity);

    /// <inheritdoc cref="DbContext.UpdateRange(object[])" />
    IEnumerable<EntityEntry> MarkModified(params object[] entities);

    /// <inheritdoc cref="DbContext.UpdateRange(object[])" />
    IEnumerable<EntityEntry> MarkModified(IEnumerable<object> entities);
    #endregion

    #region MarkAdded
    /// <inheritdoc cref="DbContext.Add" />
    EntityEntry MarkAdded(object entity);

    /// <inheritdoc cref="DbContext.AddRange(object[])" />
    IEnumerable<EntityEntry> MarkAdded(params object[] entities);

    /// <inheritdoc cref="DbContext.AddRange(object[])" />
    IEnumerable<EntityEntry> MarkAdded(IEnumerable<object> entities);
    #endregion

    #region MarkRemoved
    /// <inheritdoc cref="DbContext.Remove" />
    EntityEntry MarkRemoved(object entity);

    /// <inheritdoc cref="DbContext.RemoveRange(object[])" />
    IEnumerable<EntityEntry> MarkRemoved(params object[] entities);

    /// <inheritdoc cref="DbContext.RemoveRange(object[])" />
    IEnumerable<EntityEntry> MarkRemoved(IEnumerable<object> entities);
    #endregion

    #region Attach
    /// <inheritdoc cref="DbContext.Attach" />
    EntityEntry Attach(object entity);

    /// <inheritdoc cref="DbContext.AttachRange(object[])" />
    IEnumerable<EntityEntry> Attach(params object[] entities);

    /// <inheritdoc cref="DbContext.AttachRange(object[])" />
    IEnumerable<EntityEntry> Attach(IEnumerable<object> entities);
    #endregion

    #region Detach
    /// <summary>
    ///     Detaches the given entity from the context.
    ///     All non committed changes will be discarded.
    /// </summary>
    /// <param name="entity"> The entity to detach.</param>
    EntityEntry Detach(object entity);

    /// <summary>
    ///     Detaches the given entities from the context.
    ///     All non committed changes will be discarded.
    /// </summary>
    /// <param name="entities"> The entities to detach.</param>
    IEnumerable<EntityEntry> Detach(params object[] entities);

    /// <inheritdoc cref="Detach(object[])" />
    IEnumerable<EntityEntry> Detach(IEnumerable<object> entities);
    #endregion
}
