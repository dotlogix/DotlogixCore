#region
using System.Diagnostics.CodeAnalysis;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.EntityFramework.Conventions;
using DotLogix.WebServices.EntityFramework.Extensions;
using DotLogix.WebServices.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
#endregion

namespace DotLogix.WebServices.EntityFramework.Database; 

[SuppressMessage("ReSharper", "EF1001")]
public class DbContextBase : DbContext {
    private readonly IOptions<DbOptions> _options;
    private IDbNamingStrategy _namingStrategy;
        
    public virtual string DefaultSchema => GetType().Name.TrimEnd(nameof(DbContext));
    public virtual string MigrationSchema => DefaultSchema;
    public virtual string MigrationTableName => "MigrationHistory";
    public IDbNamingStrategy NamingStrategy => _namingStrategy ??= this.GetService<IDbNamingStrategy>();
    public DbOptions Options => _options.Value;

    protected DbContextBase(IOptions<DbOptions> options) {
        _options = options;
    }

    public DbContextBase(DbContextOptions contextOptions, IOptions<DbOptions> options) : base(contextOptions) {
        _options = options;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        base.OnConfiguring(optionsBuilder);
            
        optionsBuilder.UseQueryExtensions(options => {
            options.UseMethodCallRewriter<SearchTermRewriter>();
            options.UseMethodCallRewriter<RangeTermRewriter>();
            options.UseMethodCallRewriter<ManyTermRewriter>();
            options.UseMethodCallRewriter<LambdaEvaluateRewriter>();
        });
            
        optionsBuilder.UseNamingConventions();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        if(DefaultSchema is not null) {
            modelBuilder.HasDefaultSchema(NamingStrategy.RewriteSchemaName(DefaultSchema));
        }
        base.OnModelCreating(modelBuilder);
    }

    protected virtual EntityTypeBuilder<T> ConfigureEntity<T>(ModelBuilder modelBuilder) where T : class {
        var typeBuilder = modelBuilder.Entity<T>();
        typeBuilder.UseUtcDateTimes();
        return typeBuilder;
    }
}