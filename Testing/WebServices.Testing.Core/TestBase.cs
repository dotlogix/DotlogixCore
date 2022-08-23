using System;
using System.IO;
using System.Threading.Tasks;
using DotLogix.WebServices.Adapters.Http;
using DotLogix.WebServices.Core.Time;
using DotLogix.WebServices.Testing.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DotLogix.WebServices.Testing; 

public abstract class TestBase
{
    private IServiceScope _scope;
    protected IConfiguration Configuration { get; private set; }
    protected IServiceProvider Services { get; private set; }
    protected IServiceProvider ScopedServices => _scope?.ServiceProvider;

    protected MockDateTimeProvider DateTimeProvider { get; } = new MockDateTimeProvider();
    protected MockAuthenticationTokenProvider AuthenticationTokenProvider { get; } = new MockAuthenticationTokenProvider();

    [OneTimeSetUp]
    public virtual Task OneTimeSetUpAsync()
    {
        Configuration = CreateConfiguration();
        Services = CreateServiceProvider();
        RenewTestScope();
        return Task.CompletedTask;
    }

    [SetUp]
    public virtual Task SetUpAsync()
    {
        RenewTestScope();

        DateTimeProvider.Reset();
        AuthenticationTokenProvider.Reset();
        return Task.CompletedTask;
    }

    [TearDown]
    public virtual Task TearDownAsync()
    {
        return Task.CompletedTask;
    }

    [OneTimeTearDown]
    public virtual Task OneTimeTearDownAsync()
    {
        RenewTestScope();
        return Task.CompletedTask;
    }

    protected virtual IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        return services.BuildServiceProvider();
    }

    protected virtual IConfiguration CreateConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();
        Configure(configurationBuilder);
        return configurationBuilder.Build();
    }

    protected virtual void Configure(ConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory());
    }

    protected virtual void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider>(DateTimeProvider);
        services.AddSingleton<IAuthenticationTokenProvider>(AuthenticationTokenProvider);
    }

    protected void RenewTestScope()
    {
        _scope?.Dispose();
        _scope = CreateTestScope();
    }

    protected IServiceScope CreateTestScope()
    {
        return Services.CreateScope();
    }

    protected T GetRequiredService<T>()
    {
        return ScopedServices.GetRequiredService<T>();
    }

    protected T GetService<T>(bool required = false)
    {
        return required ? ScopedServices.GetRequiredService<T>() : ScopedServices.GetService<T>();
    }
}