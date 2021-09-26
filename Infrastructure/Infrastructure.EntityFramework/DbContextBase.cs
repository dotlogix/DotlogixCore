#region
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DotLogix.Core.Utils.Naming;
using DotLogix.Infrastructure.EntityFramework.Conventions;
using DotLogix.Infrastructure.EntityFramework.Hooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query.Internal;
#endregion

namespace DotLogix.Infrastructure.EntityFramework {
    [SuppressMessage("ReSharper", "EF1001")]
    public abstract class DbContextBase : DbContext {
        public bool UseInMemory { get; set; }
        public string DefaultSchema { get; set; } 
        public string MigrationSchema { get; set; }
        public string MigrationTableName { get; set; }
        public IDbNamingStrategy NamingStrategy { get; set; }

        protected DbContextBase() {
            NamingStrategy = new DbNamingStrategy(NamingStrategies.None);
            MigrationTableName = "MigrationHistory";
            DefaultSchema = GetType().Name;
        }

        protected DbContextBase(DbContextOptions options) : base(options) {
            MigrationTableName = "MigrationHistory";
            NamingStrategy = new DbNamingStrategy(NamingStrategies.None);
            DefaultSchema = GetType().Name;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.ReplaceService<IQueryCompiler, EfEventQueryCompiler>();
            
            if(UseInMemory) {
                OnConfiguringInMemory(optionsBuilder);
            } else {
                OnConfiguringDefault(optionsBuilder);
            }
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            if(UseInMemory) {
                return;
            }

            if(DefaultSchema != null) {
                var schemaName = NamingStrategy.RewriteSchemaName(DefaultSchema);
                modelBuilder.HasDefaultSchema(schemaName);
            }

            foreach(var entityType in modelBuilder.Model.GetEntityTypes()) {
                entityType.SetTableName(NamingStrategy.GetTableName(entityType));

                foreach(var property in entityType.GetProperties()) {
                    property.SetColumnName(NamingStrategy.GetColumnName(property));
                }
                
                foreach(var key in entityType.GetKeys()) {
                    key.SetName(NamingStrategy.GetKeyName(key));
                }

                foreach(var foreignKey in entityType.GetForeignKeys()) {
                    foreignKey.SetConstraintName(NamingStrategy.GetForeignKeyName(foreignKey));
                }

                foreach(var index in entityType.GetIndexes()) {
                    index.SetDatabaseName(NamingStrategy.GetIndexName(index));
                }
            }
        }

        protected virtual void OnConfiguringInMemory(DbContextOptionsBuilder optionsBuilder) {
            var schemaName = NamingStrategy.RewriteSchemaName(DefaultSchema);
            optionsBuilder.UseInMemoryDatabase(schemaName);
        }

        protected abstract void OnConfiguringDefault(DbContextOptionsBuilder optionsBuilder);
        

        protected virtual EntityTypeBuilder<T> ConfigureEntity<T>(ModelBuilder modelBuilder) where T : class {
            var entityType = typeof(T);
            var typeBuilder = modelBuilder.Entity<T>();

            foreach(var property in entityType.GetProperties()) {
                if(property.IsDefined(typeof(NotMappedAttribute))) {
                    continue;
                }

                var propertyBuilder = typeBuilder.Property(property.Name);
                if((property.PropertyType == typeof(DateTime)) || (property.PropertyType == typeof(DateTime?))) {
                    propertyBuilder.HasConversion(DateTimeKindSpecifyConverter.Utc);
                }
            }

            return typeBuilder;
        }
    }
}
