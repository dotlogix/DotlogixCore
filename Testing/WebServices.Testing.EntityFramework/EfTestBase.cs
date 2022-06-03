using System;
using System.Threading.Tasks;
using DotLogix.WebServices.EntityFramework.Context;
using DotLogix.WebServices.EntityFramework.Options;
using DotLogix.WebServices.Testing.EntityFramework.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace DotLogix.WebServices.Testing.EntityFramework
{
    [TestFixture]
    public abstract class EfTestBase : TestBase
    {
        [OneTimeSetUp]
        public override async Task OneTimeSetUpAsync()
        {
            await base.OneTimeSetUpAsync();
            await EntityContext.Operations.RecreateAsync();
        }

        [SetUp]
        public override async Task SetUpAsync()
        {
            await base.SetUpAsync();
            
            await EntityContext.Operations.ClearAsync();
            await SeedDatabaseAsync();
            RenewTestScope();
        }

        [OneTimeTearDown]
        public override async Task OneTimeTearDownAsync()
        {
            await base.OneTimeTearDownAsync();
            await EntityContext.Operations.DeleteAsync();
        }

        protected IDatabaseServer DatabaseServer => GetService<IDatabaseServer>();
        protected IEntityContext EntityContext => GetRequiredService<IEntityContext>();
        protected DbOptions DbOptions => GetRequiredService<IOptions<DbOptions>>()?.Value;

        protected override void Configure(ConfigurationBuilder builder)
        {
            base.Configure(builder);
            builder.AddJsonFile("appSettings.Development.json");
        }

        protected override void ConfigureServices(ServiceCollection services)
        {
            base.ConfigureServices(services);

            // DbConfig
            services.AddSingleton(GetDatabaseConfiguration);

            // Database Test Adapter
            services.AddSingleton(GetDatabaseServer);
        }

        protected virtual IOptions<DbOptions> GetDatabaseConfiguration(IServiceProvider serviceProvider)
        {
            var config = new DbOptions();
            Configuration.GetSection("Database").Bind(config);
            config.ConnectionString ??= DatabaseServer?.ConnectionString;
            return new OptionsWrapper<DbOptions>(config);
        }

        protected virtual IDatabaseServer GetDatabaseServer(IServiceProvider serviceProvider)
        {
            return null;
        }

        protected virtual Task SeedDatabaseAsync()
        {
            return Task.CompletedTask;
        }
    }
}
