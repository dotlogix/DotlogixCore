using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotLogix.WebServices.EntityFramework.Context {
    public class EntityTypeManager : IEntityTypeManager {
        private readonly IModel _model;

        public EntityTypeManager(IModel model) {
            _model = model;
        }

        /// <inheritdoc />
        public IEntityType GetEntityType<TEntity>() where TEntity : class {
            return GetEntityType(typeof(TEntity));
        }

        /// <inheritdoc />
        public IEntityType GetEntityType(Type entityType) {
            return _model.FindEntityType(entityType);
        }

        /// <inheritdoc />
        public IEnumerable<IEntityType> GetEntityTypes<TEntity>() where TEntity : class {
            return GetEntityTypes();
        }

        /// <inheritdoc />
        public IEnumerable<IEntityType> GetEntityTypes(Type entityType) {
            return _model.FindEntityTypes(entityType);
        }

        /// <inheritdoc />
        public IEnumerable<IEntityType> GetEntityTypes() {
            return _model.GetEntityTypes();
        }
    }
}