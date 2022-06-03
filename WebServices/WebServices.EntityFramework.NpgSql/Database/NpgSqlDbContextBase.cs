#region
using System.Diagnostics.CodeAnalysis;
using DotLogix.Core.Utils.Naming;
using DotLogix.WebServices.EntityFramework.Expressions;
using DotLogix.WebServices.EntityFramework.Expressions.Rewriters;
using DotLogix.WebServices.EntityFramework.Extensions;
using DotLogix.WebServices.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using Npgsql;
#endregion

namespace DotLogix.WebServices.EntityFramework.Database {
    [SuppressMessage("ReSharper", "EF1001")]
    public abstract class NpgSqlDbContextBase : DbContextBase {
        protected NpgSqlDbContextBase(IOptions<DbOptions> options) : base(options) {
        }

        protected NpgSqlDbContextBase(DbContextOptions contextOptions, IOptions<DbOptions> options) : base(contextOptions, options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseQueryExtensions(options => {
                options.UseMethodCallRewriter<SearchTermRewriter, NpgSqlSearchTermRewriter>();
            });

            optionsBuilder.ReplaceService<IHistoryRepository, NpgsqlNamingConventionHistoryRepository>();
            optionsBuilder.UseNamingConventions(options => {
                options.UseNamingStrategy(NamingStrategies.SnakeCase);
            });

            var builder = new NpgsqlConnectionStringBuilder(Options.ConnectionString);
            builder.Password ??= Options.Password;

            optionsBuilder.UseNpgsql(builder.ConnectionString,
                o => {
                    o.MigrationsHistoryTable(MigrationTableName, MigrationSchema);

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
