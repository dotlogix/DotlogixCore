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
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public class EntityCollection<TEntity> where TEntity : class, ISimpleEntity {
        private readonly Dictionary<Guid, TEntity> _entities = new Dictionary<Guid, TEntity>();

        public int Count => _entities.Count;

        #region Get
        public bool TryGet(int id, out TEntity entity) {
            entity = _entities.Values.FirstOrDefault(e => e.Id == id);
            return entity != null;
        }

        public bool TryGetRange(IEnumerable<int> ids, out IEnumerable<TEntity> foundEntities, out IEnumerable<int> missingIds) {
            var missing = new HashSet<int>(ids);
            foundEntities = _entities.Values.Where(value => (value != null) && missing.Remove(value.Id)).ToList();
            missingIds = missing;
            return missing.Count == 0;
        }

        public bool TryGet(Guid guid, out TEntity entity) {
            return _entities.TryGetValue(guid, out entity);
        }

        public bool TryGetRange(IEnumerable<Guid> guids, out IEnumerable<TEntity> foundEntities, out IEnumerable<Guid> missingGuids) {
            var found = new List<TEntity>();
            var missing = new List<Guid>();

            foreach(var guid in guids) {
                if(_entities.TryGetValue(guid, out var entity) == false)
                    missing.Add(guid);
                else if(entity != null)
                    found.Add(entity);
            }
            foundEntities = found;
            missingGuids = missing;
            return missing.Count == 0;
        }

        public IEnumerable<TEntity> GetAllPresent() {
            return _entities.Values;
        }
        #endregion

        #region Add
        public void AddOrUpdate(TEntity entity) {
            _entities[entity.Guid] = entity;
        }

        public void AddOrUpdateRange(IEnumerable<TEntity> entities) {
            foreach(var entity in entities)
                _entities[entity.Guid] = entity;
        }
        #endregion

        #region Remove
        public void Remove(TEntity entity) {
            _entities[entity.Guid] = null;
        }

        public void RemoveRange(IEnumerable<TEntity> entities) {
            foreach(var entity in entities)
                _entities[entity.Guid] = null;
        }
        #endregion

        #region MarkNonPresent
        public void MarkNonPresent(Guid guid) {
            _entities[guid] = null;
        }

        public void MarkRangeNonPresent(IEnumerable<Guid> guids) {
            foreach(var guid in guids)
                _entities[guid] = null;
        }
        #endregion
    }
}
