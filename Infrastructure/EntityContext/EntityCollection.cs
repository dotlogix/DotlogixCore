// ==================================================
// Copyright 2018(C) , DotLogix
// File:  EntityCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  04.04.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Architecture.Infrastructure.Entities;
using DotLogix.Core.Collections;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public class EntityIndex<TEntity> where TEntity : ISimpleEntity {
        public EntityCollection<int, TEntity> ById { get; }
        public EntityCollection<Guid, TEntity> ByGuid { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public EntityIndex(EntityCollection<int, TEntity> byId, EntityCollection<Guid, TEntity> byGuid) {
            ById = byId ?? throw new ArgumentNullException(nameof(byId));
            ByGuid = byGuid ?? throw new ArgumentNullException(nameof(byGuid));
        }

        public EntityIndex(IEnumerable<TEntity> entities) {
            if(entities == null)
                throw new ArgumentNullException(nameof(entities));
            entities = entities.AsCollection();
            ById = new EntityCollection<int, TEntity>(e => e.Id, entities);
            ByGuid = new EntityCollection<Guid, TEntity>(e => e.Guid, entities);
        }

        public EntityIndex() {
            ById = new EntityCollection<int, TEntity>(e => e.Id);
            ByGuid = new EntityCollection<Guid, TEntity>(e => e.Guid);
        }
    }


    public class EntityCollection<TKey, TEntity> : KeyedCollection<TKey, TEntity> {
        public EntityCollection(Func<TEntity, TKey> keySelector) : base(keySelector) { }
        public EntityCollection(Func<TEntity, TKey> keySelector, IEnumerable<TEntity> values) : base(keySelector, values) { }
        public EntityCollection(Func<TEntity, TKey> keySelector, IEqualityComparer<TKey> equalityComparer) : base(keySelector, equalityComparer) { }
        public EntityCollection(Func<TEntity, TKey> keySelector, IEqualityComparer<TKey> equalityComparer, IEnumerable<TEntity> values) : base(keySelector, equalityComparer, values) { }


        #region Get
        public bool TryGetRange(IEnumerable<TKey> keys, out IEnumerable<TEntity> foundEntities, out IEnumerable<TKey> missingKeys) {
            var found = new List<TEntity>();
            var missing = new List<TKey>();

            foreach(var key in keys) {
                if(InnerDictionary.TryGetValue(key, out var entity) == false)
                    missing.Add(key);
                else if(entity != null)
                    found.Add(entity);
            }

            foundEntities = found;
            missingKeys = missing;
            return missing.Count == 0;
        }
        #endregion

        #region Add
        public void AddOrUpdateRange(IEnumerable<TEntity> entities) {
            foreach(var entity in entities)
                AddOrUpdate(entity);
        }
        #endregion

        #region Remove
        public bool TryRemoveRange(IEnumerable<TEntity> entities) {
            return entities.All(TryRemove);
        }
        #endregion

        #region MarkNonPresent
        public void MarkNonPresent(TKey key) {
            InnerDictionary[key] = default;
        }

        public void MarkRangeNonPresent(IEnumerable<TKey> keys) {
            foreach(var key in keys)
                InnerDictionary[key] = default;
        }
        #endregion
    }

    public class EntityCollection<TEntity> : EntityCollection<Guid, TEntity> where TEntity : class, ISimpleEntity {
        public EntityCollection() : base(GetKey) { }
        public EntityCollection(IEnumerable<TEntity> values) : base(GetKey, values) { }

        private static Guid GetKey(TEntity entity) {
            return entity.Guid;
        }
    }
}
