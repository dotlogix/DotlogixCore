#region
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DotLogix.Core.Utils.Naming;
using DotLogix.Infrastructure.EntityFramework;
using DotLogix.Infrastructure.EntityFramework.Conventions;
using DotLogix.WebServices.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
#endregion

namespace DotLogix.WebServices.EntityFramework.Context {
    [SuppressMessage("ReSharper", "EF1001")]
    public abstract class NpgSqlDbContextBase : DbContextBase {
        private readonly IOptions<DbOptions> _options;
        public DbOptions Options => _options.Value;

        protected NpgSqlDbContextBase(IOptions<DbOptions> options) {
            _options = options;
            NamingStrategy = new SanitizingDbNamingStrategy(NamingStrategies.SnakeCase);
        }


        protected override void OnConfiguringDefault(DbContextOptionsBuilder optionsBuilder) {
            var migrationSchema = NamingStrategy.RewriteSchemaName(MigrationSchema ?? DefaultSchema);
            var migrationTableName = NamingStrategy.RewriteTableName(MigrationTableName);
            
            optionsBuilder.UseNpgsql(Options.ConnectionString,
                                     o => {
                                         o.MigrationsHistoryTable(migrationTableName, migrationSchema);
                                         o.ProvidePasswordCallback((host, port, database, username) => Options.Password);

                                         if(Options.CommandTimeout.HasValue) {
                                             o.CommandTimeout(Options.CommandTimeout);
                                         }
                                         
                                         if(Options.MaxRetries.HasValue) {
                                             o.EnableRetryOnFailure(Options.MaxRetries.Value);
                                         }
                                     }
                                    );
        }
    }
}
