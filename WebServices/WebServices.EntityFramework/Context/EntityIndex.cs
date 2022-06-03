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
using DotLogix.Core.Collections;
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.WebServices.EntityFramework.Context {
    /// <summary>
    /// A key indexed entity collection
    /// </summary>
    public class EntityIndex<TKey, TEntity> : KeyedCollection<TKey, TEntity> where TEntity : class, new() {
        /// <summary>
        /// Creates a new instance of <see cref="EntityIndex{TKey,TEntity}"/>
        /// </summary>
        public EntityIndex(Func<TEntity, TKey> keySelector) : base(keySelector) { }
        /// <summary>
        /// Creates a new instance of <see cref="EntityIndex{TKey,TEntity}"/>
        /// </summary>
        public EntityIndex(Func<TEntity, TKey> keySelector, IEnumerable<TEntity> values) : base(keySelector, values) { }

        /// <summary>
        /// Creates a new instance of <see cref="EntityIndex{TKey,TEntity}"/>
        /// </summary>
        public EntityIndex(Func<TEntity, TKey> keySelector, IEqualityComparer<TKey> equalityComparer) : base(keySelector, equalityComparer) { }
        /// <summary>
        /// Creates a new instance of <see cref="EntityIndex{TKey,TEntity}"/>
        /// </summary>
        public EntityIndex(Func<TEntity, TKey> keySelector, IEqualityComparer<TKey> equalityComparer, IEnumerable<TEntity> values) : base(keySelector, equalityComparer, values) { }
        /// <summary>
        /// Creates a new instance of <see cref="EntityIndex{TKey,TEntity}"/>
        /// </summary>
        public EntityIndex(DynamicAccessor accessor) : base(e => (TKey)accessor.GetValue(e)) {
        }

        /// <summary>
        /// Creates a new instance of <see cref="EntityIndex{TKey,TEntity}"/>
        /// </summary>
        public EntityIndex(DynamicAccessor accessor, IEnumerable<TEntity> values) : base(e => (TKey)accessor.GetValue(e), values) {
        }

        /// <summary>
        /// Creates a new instance of <see cref="EntityIndex{TKey,TEntity}"/>
        /// </summary>
        public EntityIndex(DynamicAccessor accessor, IEqualityComparer<TKey> equalityComparer) : base(e => (TKey)accessor.GetValue(e), equalityComparer) {
        }
        
        /// <inheritdoc />
        public EntityIndex(DynamicAccessor accessor, IEqualityComparer<TKey> equalityComparer, IEnumerable<TEntity> values) : base(e => (TKey)accessor.GetValue(e), equalityComparer, values) {
        }

        #region Get
        /// <summary>
        /// Get a range of entities by their key and also return all missing keys
        /// </summary>
        public bool TryGetRange(IEnumerable<TKey> keys, out IEnumerable<TEntity> foundEntities, out IEnumerable<TKey> missingKeys) {
            var found = new List<TEntity>();
            var missing = new List<TKey>();

            foreach (var key in keys) {
                if (InnerDictionary.TryGetValue(key, out var entity) == false)
                    missing.Add(key);
                else if (entity != null)
                    found.Add(entity);
            }

            foundEntities = found;
            missingKeys = missing;
            return missing.Count == 0;
        }
        #endregion

        #region Add
        /// <summary>
        /// Add or update a range of entities by their key and also return all missing keys
        /// </summary>
        public void AddOrUpdateRange(IEnumerable<TEntity> entities) {
            foreach(var entity in entities)
                AddOrUpdate(entity);
        }
        #endregion

        #region MarkNonPresent
        /// <summary>
        /// Mark a single entity as non present
        /// </summary>
        public void MarkNonPresent(TKey key) {
            InnerDictionary[key] = default;
        }

        /// <summary>
        /// Mark a range of entities as non present
        /// </summary>
        public void MarkRangeNonPresent(IEnumerable<TKey> keys) {
            foreach(var key in keys)
                InnerDictionary[key] = default;
        }
        #endregion

        #region GetKey
        /// <summary>
        /// Selects a key of an entity
        /// </summary>
        public TKey GetKey(TEntity entity) {
            return KeySelector.Invoke(entity);
        }
        #endregion

    }
}
