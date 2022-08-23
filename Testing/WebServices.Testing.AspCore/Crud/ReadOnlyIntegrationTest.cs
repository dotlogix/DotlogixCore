using System;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.WebServices.Adapters.Client;
using DotLogix.WebServices.Core.Errors;
using NUnit.Framework;

namespace DotLogix.WebServices.Testing.AspCore.Crud; 

[TestFixture]
[NonParallelizable]
public abstract class ReadOnlyIntegrationTest<TEntity, TModel, TFilter, TClient, TStartup> : WebServiceTestBase<TStartup>
    where TEntity : class, IGuid
    where TModel : class, IGuid
    where TClient : IReadOnlyApiClient<TModel, TFilter>
    where TStartup : class {
    public override async Task SetUpAsync()
    {
        await base.SetUpAsync();
        ApiClient = CreateApiClient();
    }

    protected TClient ApiClient { get; private set; }
    protected TEntity Entity { get; set; }
    protected Guid UserGuid { get; set; } = Guid.NewGuid();

    protected override async Task SeedDatabaseAsync()
    {
        await base.SeedDatabaseAsync();
        Entity = NewEntity();

        await EntityContext.CompleteAsync();
    }

    [Test]
    public void GetAsync_NonExisting_ThrowsNotFound()
    {
        var guid = Guid.NewGuid();

        var exception = Assert.CatchAsync<ApiClientException>(() => ApiClient.GetAsync(guid));
        Assert.That(exception.Kind, Is.EqualTo(ApiErrorKinds.KeyNotFound));
    }


    [Test]
    public async Task GetAsync_Existing_ReturnsExpected()
    {
        var guid = Entity.Guid;

        var result = await ApiClient.GetAsync(guid);
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task GetRangeAsync_NonExisting_ThrowsNotFound()
    {
        var entities = await ApiClient.GetRangeAsync(new[] { Guid.NewGuid() });
        Assert.That(entities, Is.Empty);
    }

    [Test]
    public async Task GetRangeAsync_Existing_ReturnsExpected()
    {
        var result = await ApiClient.GetRangeAsync(new[] { Entity.Guid });
        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    public async Task GetAllAsync_Existing_ReturnsExpected()
    {
        var result = await ApiClient.GetAllAsync();
        Assert.That(result, Is.Not.Empty);
    }


    protected abstract TClient CreateApiClient();
    protected abstract TEntity NewEntity(Guid? guid = default);
}