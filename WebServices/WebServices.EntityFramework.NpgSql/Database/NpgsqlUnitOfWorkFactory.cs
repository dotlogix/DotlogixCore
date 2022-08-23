using System.Threading;
using System.Threading.Tasks;
using DotLogix.WebServices.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Database; 

public class NpgsqlUnitOfWorkFactory<TDbContext> : UnitOfWorkFactory<TDbContext> where TDbContext : DbContext {
    public NpgsqlUnitOfWorkFactory(IDbContextFactory<TDbContext> dbContextFactory) : base(dbContextFactory) {
    }

    protected override IEntityDatabaseOperations CreateDatabaseOperations(TDbContext dbContext) {
        return new NpgsqlRelationalEntityDbOperations(dbContext);
    }
}

public class NpgsqlRelationalEntityDbOperations : RelationalEntityDbOperations {
    public NpgsqlRelationalEntityDbOperations(DbContext dbContext) : base(dbContext) {
    }

    public override async Task CreateAsync(CancellationToken cancellationToken = default) {
        await base.CreateAsync(cancellationToken);
        await DbContext.Database.NpgSqlReloadTypesAsync();
    }

    public override async Task ClearAsync(CancellationToken cancellationToken = default) {
        await base.ClearAsync(cancellationToken);
        await DbContext.Database.NpgSqlReloadTypesAsync();
    }

    public override async Task DeleteAsync(CancellationToken cancellationToken = default) {
        await base.DeleteAsync(cancellationToken);
        await DbContext.Database.NpgSqlReloadTypesAsync();
    }

    public override async Task RecreateAsync(CancellationToken cancellationToken = default) {
        await base.CreateAsync(cancellationToken);
        await DbContext.Database.NpgSqlReloadTypesAsync();
    }

    public override async Task MigrateAsync(CancellationToken cancellationToken = default) {
        await base.RecreateAsync(cancellationToken);
        await DbContext.Database.NpgSqlReloadTypesAsync();
    }

    public override async Task RunCommandAsync(DatabaseCommand command, CancellationToken cancellationToken = default) {
        await base.RunCommandAsync(command, cancellationToken);
        await DbContext.Database.NpgSqlReloadTypesAsync();
    }
}