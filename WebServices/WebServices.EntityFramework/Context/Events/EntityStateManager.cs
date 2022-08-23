using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotLogix.WebServices.EntityFramework.Context.Events; 

[SuppressMessage("ReSharper", "EF1001")]
public class EntityStateManager : IEntityStateManager {
    private readonly IStateManager _stateManager;

    public EntityStateManager(IStateManager stateManager) {
        _stateManager = stateManager;
    }

    #region GetEntries
    /// <inheritdoc />
    public EntityEntry GetEntry(object entity) {
        return new(_stateManager.GetOrCreateEntry(entity));
    }

    /// <inheritdoc />
    public EntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class {
        return new(_stateManager.GetOrCreateEntry(entity));
    }

    /// <inheritdoc />
    public EntityEntry GetEntry(IKey dbKey, object key) {
        var internalEntityEntry = _stateManager.TryGetEntry(dbKey, new[] {key});
        return internalEntityEntry?.ToEntityEntry();
    }

    /// <inheritdoc />
    public EntityEntry<TEntity> GetEntry<TEntity>(IKey dbKey, object key) where TEntity : class {
        var keyValues = key is object[] compositeKey ? compositeKey : new[] {key};
        var internalEntityEntry = _stateManager.TryGetEntry(dbKey, keyValues);
        return internalEntityEntry is not null
            ? new EntityEntry<TEntity>(internalEntityEntry)
            : default;
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> GetEntries() {
        return _stateManager.Entries.Select(e => e.ToEntityEntry());
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> GetEntriesToSave(bool cascadeChanges = true) {
        return _stateManager.GetEntriesToSave(cascadeChanges).Select(e => e.ToEntityEntry());
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> GetEntriesForState(bool added = false, bool modified = false, bool deleted = false, bool unchanged = false) {
        return _stateManager.GetEntriesForState(added, modified, deleted, unchanged).Select(e => e.ToEntityEntry());
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry<TEntity>> GetEntries<TEntity>() where TEntity : class {
        return _stateManager.Entries
           .Where(e => e is TEntity)
           .Select(e => new EntityEntry<TEntity>(e));
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> GetEntries(IKey dbKey, IEnumerable<object> keys) {
        return GetEntries<object>(dbKey, keys, out _);
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> GetEntries(IKey dbKey, IEnumerable<object> keys, out IReadOnlyCollection<object> missingKeys) {
        return GetEntries<object>(dbKey, keys, out missingKeys);
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> GetEntries<TKey>(IKey dbKey, IEnumerable<TKey> keys) {
        return GetEntries(dbKey, keys, out _);
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> GetEntries<TKey>(IKey dbKey, IEnumerable<TKey> keys, out IReadOnlyCollection<TKey> missingKeys) {
        var missing = new List<TKey>();
        var existing = new List<EntityEntry>();
        foreach(var key in keys) {
            var keyValues = key is object[] compositeKey ? compositeKey : new object[] {key};
            var internalEntityEntry = _stateManager.TryGetEntry(dbKey, keyValues);
            if(internalEntityEntry == null) {
                missing.Add(key);
            } else {
                existing.Add(internalEntityEntry.ToEntityEntry());
            }
        }
        missingKeys = missing;
        return existing;
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry<TEntity>> GetEntries<TEntity>(IKey dbKey, IEnumerable<object> keys) where TEntity : class {
        return GetEntries<object, TEntity>(dbKey, keys, out _);
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry<TEntity>> GetEntries<TEntity>(IKey dbKey, IEnumerable<object> keys, out IReadOnlyCollection<object> missingKeys) where TEntity : class {
        return GetEntries<object, TEntity>(dbKey, keys, out missingKeys);
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry<TEntity>> GetEntries<TKey, TEntity>(IKey dbKey, IEnumerable<TKey> keys) where TEntity : class {
        return GetEntries<TKey, TEntity>(dbKey, keys, out _);
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry<TEntity>> GetEntries<TKey, TEntity>(IKey dbKey, IEnumerable<TKey> keys, out IReadOnlyCollection<TKey> missingKeys) where TEntity : class {
        var missing = new List<TKey>();
        var existing = new List<EntityEntry<TEntity>>();
        foreach(var key in keys) {
            var keyValues = key is object[] compositeKey ? compositeKey : new object[] {key};
            var internalEntityEntry = _stateManager.TryGetEntry(dbKey, keyValues);
            if(internalEntityEntry == null) {
                missing.Add(key);
            } else {
                existing.Add(new EntityEntry<TEntity>(internalEntityEntry));
            }
        }
        missingKeys = missing;
        return existing;    
    }
    #endregion

    #region MarkModified
    /// <inheritdoc />
    public EntityEntry MarkModified(object entity) {
        var entry = _stateManager.GetOrCreateEntry(entity);
        entry.SetEntityState(EntityState.Modified, forceStateWhenUnknownKey: EntityState.Added);
        return entry.ToEntityEntry();
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> MarkModified(params object[] entities) {
        return entities.Select(MarkModified);
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> MarkModified(IEnumerable<object> entities) {
        return entities.Select(MarkModified);
    }
    #endregion

    #region MarkAdded
    /// <inheritdoc />
    public EntityEntry MarkAdded(object entity) {
        var entry = _stateManager.GetOrCreateEntry(entity);
        entry.SetEntityState(EntityState.Added);
        return entry.ToEntityEntry();
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> MarkAdded(params object[] entities) {
        return entities.Select(MarkAdded);
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> MarkAdded(IEnumerable<object> entities) {
        return entities.Select(MarkAdded);
    }
    #endregion

    #region MarkRemoved
    /// <inheritdoc />
    public EntityEntry MarkRemoved(object entity) {
        var entry = _stateManager.GetOrCreateEntry(entity);
        entry.SetEntityState(EntityState.Deleted, forceStateWhenUnknownKey: EntityState.Detached);
        return entry.ToEntityEntry();
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> MarkRemoved(params object[] entities) {
        return entities.Select(MarkRemoved);
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> MarkRemoved(IEnumerable<object> entities) {
        return entities.Select(MarkRemoved);
    }
    #endregion

    #region Attach
    /// <inheritdoc />
    public EntityEntry Attach(object entity) {
        var entry = _stateManager.GetOrCreateEntry(entity);
        entry.SetEntityState(EntityState.Unchanged, forceStateWhenUnknownKey: EntityState.Added);
        return entry.ToEntityEntry();
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> Attach(params object[] entities) {
        return entities.Select(MarkAdded);
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> Attach(IEnumerable<object> entities) {
        return entities.Select(MarkAdded);
    }
    #endregion

    #region Detach
    /// <inheritdoc />
    public EntityEntry Detach(object entity) {
        var entry = _stateManager.TryGetEntry(entity);
        if(entry == null) {
            return default;
        }
            
        entry.SetEntityState(EntityState.Detached);
        return entry.ToEntityEntry();
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> Detach(params object[] entities) {
        return entities.Select(Detach);
    }

    /// <inheritdoc />
    public IEnumerable<EntityEntry> Detach(IEnumerable<object> entities) {
        return entities.Select(Detach);
    }
    #endregion
}