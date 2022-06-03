using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.WebServices.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotLogix.WebServices.EntityFramework.Extensions {
    public static class EntityStateManagerExtensions {
        public static bool HasChanges(this EntityEntry entry) {
            switch(entry.State) {
                case EntityState.Added:
                case EntityState.Modified:
                case EntityState.Deleted:
                    return true;
                case EntityState.Detached:
                case EntityState.Unchanged:
                    return false;
                default:    
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public static object GetValue(this IEntityStateManager stateManager, IKey dbKey, object key) {
            return stateManager.GetEntry(dbKey, key)?.Entity;
        }
        public static TEntity GetValue<TEntity>(this IEntityStateManager stateManager, IKey dbKey, object key) where TEntity : class {
            return stateManager.GetEntry<TEntity>(dbKey, key)?.Entity;
        }
        public static IEnumerable<object> GetValues(this IEntityStateManager stateManager) {
            return stateManager.GetEntries().Select(e => e.Entity); 
        }
        public static IEnumerable<TEntity> GetValues<TEntity>(this IEntityStateManager stateManager) where TEntity : class {
            return stateManager.GetEntries<TEntity>().Select(e => e.Entity);
        }
        public static IEnumerable<object> GetValues(this IEntityStateManager stateManager, IKey dbKey, IEnumerable<object> keys) {
            return stateManager.GetEntries(dbKey, keys).Select(e => e.Entity);
        }
        public static IEnumerable<TEntity> GetValues<TEntity>(this IEntityStateManager stateManager, IKey dbKey, IEnumerable<object> keys) where TEntity : class {
            return stateManager.GetEntries<TEntity>(dbKey, keys).Select(e => e.Entity);
        }
        public static IEnumerable<TEntity> GetValues<TEntity>(this IEntityStateManager stateManager, IKey dbKey, IEnumerable<object> keys, out IReadOnlyCollection<object> missingKeys) where TEntity : class {
            return stateManager.GetEntries<TEntity>(dbKey, keys, out missingKeys).Select(e => e.Entity);
        }
        public static IEnumerable<object> GetValues<TKey>(this IEntityStateManager stateManager, IKey dbKey, IEnumerable<TKey> keys) {
            return stateManager.GetEntries(dbKey, keys).Select(e => e.Entity);
        }
        public static IEnumerable<TEntity> GetValues<TKey, TEntity>(this IEntityStateManager stateManager, IKey dbKey, IEnumerable<TKey> keys) where TEntity : class {
            return stateManager.GetEntries<TKey, TEntity>(dbKey, keys).Select(e => e.Entity);
        }
        public static IEnumerable<TEntity> GetValues<TKey, TEntity>(this IEntityStateManager stateManager, IKey dbKey, IEnumerable<TKey> keys, out IReadOnlyCollection<TKey> missingKeys) where TEntity : class {
            return stateManager.GetEntries<TKey, TEntity>(dbKey, keys, out missingKeys).Select(e => e.Entity);
        }
    }
}