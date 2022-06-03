using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotLogix.WebServices.EntityFramework.Context {
    public interface IEntityTypeManager {
        /// <inheritdoc cref="ModelExtensions.FindEntityType(Microsoft.EntityFrameworkCore.Metadata.IModel,System.Type)"/>
        IEntityType GetEntityType<TEntity>() where TEntity : class;
        /// <inheritdoc cref="ModelExtensions.FindEntityType(Microsoft.EntityFrameworkCore.Metadata.IModel,System.Type)"/>
        IEntityType GetEntityType(Type entityType);
        /// <inheritdoc cref="IModel.GetEntityTypes"/>
        IEnumerable<IEntityType> GetEntityTypes();
        /// <inheritdoc cref="ModelExtensions.GetEntityTypes(Microsoft.EntityFrameworkCore.Metadata.IModel,System.Type)"/>
        IEnumerable<IEntityType> GetEntityTypes<TEntity>() where TEntity : class;
        /// <inheritdoc cref="ModelExtensions.GetEntityTypes(Microsoft.EntityFrameworkCore.Metadata.IModel,System.Type)"/>
        IEnumerable<IEntityType> GetEntityTypes(Type entityType);
    }
}