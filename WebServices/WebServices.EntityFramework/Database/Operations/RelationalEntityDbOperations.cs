using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace DotLogix.WebServices.EntityFramework.Database {
    public class RelationalEntityDbOperations : IEntityDatabaseOperations {
        protected DbContext DbContext { get; }

        public RelationalEntityDbOperations(DbContext dbContext) {
            DbContext = dbContext;
        }

        /// <inheritdoc />
        public virtual Task CreateAsync(CancellationToken cancellationToken = default) {
            return MigrateAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task ClearAsync(CancellationToken cancellationToken = default) {
            return RecreateAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task DeleteAsync(CancellationToken cancellationToken = default) {
            await DbContext.Database.EnsureDeletedAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task RecreateAsync(CancellationToken cancellationToken = default) {
            await DeleteAsync(cancellationToken);
            await MigrateAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task MigrateAsync(CancellationToken cancellationToken = default) {
            return MigrateAsync(default, cancellationToken);
        }

        /// <inheritdoc />
        public async Task MigrateAsync(DatabaseMigration migration, CancellationToken cancellationToken = default) {
            var migrator = DbContext.Database.GetService<IMigrator>();
            await migrator.MigrateAsync(migration?.FullName, cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<ICollection<DatabaseMigration>> GetMigrationsAsync(CancellationToken cancellationToken = default) {
            var migrationNames = DbContext.Database.GetMigrations();
            var migrations = migrationNames
                            .Select(name => new DatabaseMigration(name))
                            .ToCollection();
            return Task.FromResult(migrations);
        }

        /// <inheritdoc />
        public virtual async Task<ICollection<DatabaseMigration>> GetPendingMigrationsAsync(CancellationToken cancellationToken = default) {
            var migrationNames = await DbContext.Database.GetPendingMigrationsAsync(cancellationToken);
            var migrations = migrationNames
                            .Select(name => new DatabaseMigration(name))
                            .ToCollection();
            return migrations;
        }

        /// <inheritdoc />
        public virtual async Task<ICollection<DatabaseMigration>> GetAppliedMigrationsAsync(CancellationToken cancellationToken = default) {
            var migrationNames = await DbContext.Database.GetAppliedMigrationsAsync(cancellationToken);
            var migrations = migrationNames
                            .Select(name => new DatabaseMigration(name))
                            .ToCollection();
            return migrations;
        }

        /// <inheritdoc />
        public virtual async Task RunCommandAsync(DatabaseCommand command, CancellationToken cancellationToken = default) {
            var serviceProvider = DbContext.GetInfrastructure();
            var dbCommand = serviceProvider
                          .GetRequiredService<IEnumerable<IDatabaseCommandExecutor>>()
                          .First(c => c.Name == command.Name);
            await dbCommand.RunAsync(serviceProvider, serviceProvider.GetRequiredService<IEntityContext>(), cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task<ICollection<DatabaseCommand>> GetCommandsAsync(CancellationToken cancellationToken = default) {
            var serviceProvider = DbContext.GetInfrastructure();
            var commands = serviceProvider
                          .GetRequiredService<IEnumerable<IDatabaseCommandExecutor>>()
                          .Select(c => new DatabaseCommand(c.Name))
                          .ToCollection();
            return Task.FromResult(commands);
        }
    }
    
    public interface IDatabaseCommandExecutor {
        string Name { get; }
        Task RunAsync(IServiceProvider serviceProvider, IEntityContext entityContext, CancellationToken cancellationToken = default);
    }
}