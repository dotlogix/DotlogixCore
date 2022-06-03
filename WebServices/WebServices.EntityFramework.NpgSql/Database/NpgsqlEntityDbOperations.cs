using System.Threading;
using System.Threading.Tasks;
using DotLogix.WebServices.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Database {
    public class NpgsqlEntityDbOperations : RelationalEntityDbOperations {
        public NpgsqlEntityDbOperations(DbContext dbContext) : base(dbContext) {
        }

        public override async Task MigrateAsync(CancellationToken cancellationToken = default) {
            await base.MigrateAsync(cancellationToken);
            await DbContext.Database.NpgSqlReloadTypesAsync();
        }
    }
}