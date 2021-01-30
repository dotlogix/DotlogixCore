using System;
using System.Collections.Generic;
using DotLogix.Core.Collections;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    /// <summary>
    /// A simple guid and id indexed entity collection
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class EntityLookup<TKey, TEntity> : KeyedLookup<TKey, TEntity> where TEntity : class, new() {
        /// <inheritdoc />
        public EntityLookup(Func<TEntity, TKey> keySelector) : base(keySelector)
        {
        }

        /// <inheritdoc />
        public EntityLookup(Func<TEntity, TKey> keySelector, IEnumerable<TEntity> values) : base(keySelector, values)
        {
        }

        /// <inheritdoc />
        public EntityLookup(Func<TEntity, TKey> keySelector, IEqualityComparer<TKey> equalityComparer) : base(keySelector, equalityComparer)
        {
        }

        /// <inheritdoc />
        public EntityLookup(Func<TEntity, TKey> keySelector, IEqualityComparer<TKey> equalityComparer, IEnumerable<TEntity> values) : base(keySelector, equalityComparer, values)
        {
        }       /// <inheritdoc />
        public EntityLookup(DynamicAccessor accessor) : base(e => (TKey)accessor.GetValue(e))
        {
        }

        /// <inheritdoc />
        public EntityLookup(DynamicAccessor accessor, IEnumerable<TEntity> values) : base(e => (TKey)accessor.GetValue(e), values)
        {
        }

        /// <inheritdoc />
        public EntityLookup(DynamicAccessor accessor, IEqualityComparer<TKey> equalityComparer) : base(e => (TKey)accessor.GetValue(e), equalityComparer)
        {
        }

        /// <inheritdoc />
        public EntityLookup(DynamicAccessor accessor, IEqualityComparer<TKey> equalityComparer, IEnumerable<TEntity> values) : base(e => (TKey)accessor.GetValue(e), equalityComparer, values)
        {
        }



        #region Get
        /// <summary>
        /// Get a range of entities by their key and also return all missing keys
        /// </summary>
        public bool TryGetRange(IEnumerable<TKey> keys, out IEnumerable<KeyValuePair<TKey, IEnumerable<TEntity>>> foundEntities, out IEnumerable<TKey> missingKeys) {
            var found = new List<KeyValuePair<TKey, IEnumerable<TEntity>>>();
            var missing = new List<TKey>();

            foreach (var key in keys) {
                if (InnerLookup.TryGetValue(key, out var entities) == false)
                    missing.Add(key);
                else if (entities != null)
                    found.Add(new KeyValuePair<TKey, IEnumerable<TEntity>>(key, entities));
            }

            foundEntities = found;
            missingKeys = missing;
            return missing.Count == 0;
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