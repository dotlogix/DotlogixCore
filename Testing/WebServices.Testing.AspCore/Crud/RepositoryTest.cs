using System;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.WebServices.EntityFramework.Repositories;
using DotLogix.WebServices.Testing.EntityFramework;
using DotLogix.WebServices.Testing.NUnit;
using NUnit.Framework;

namespace DotLogix.WebServices.Testing.AspCore.Crud; 

[TestFixture]
public abstract class RepositoryTest<TEntity, TRepository> : EfTestBase
    where TRepository : IRepository<Guid, TEntity>
    where TEntity : class, IGuid, IIdentity, new()
{
    protected TRepository Repository => GetRequiredService<TRepository>();

    [Test]
    public virtual async Task GetAsync_NonExisting_ReturnsNull()
    {
        var result = await Repository.GetAsync(Guid.NewGuid());
        Assert.That(result, Is.Null);
    }

    [Test]
    public virtual async Task GetAsync_Existing_ReturnsExpected()
    {
        var entity = CreateEntity();
        EntityContext.Add(entity);
        await EntityContext.CompleteAsync();

        var result = await Repository.GetAsync(entity.Guid);
        Assert.That(result, Is.EqualTo(entity).ByJsonDiff());
    }

    [Test]
    public virtual async Task GetRangeAsync_NonExisting_ReturnsNull()
    {
        var guids = Enumerable.Range(0, 10).Select(_ => Guid.NewGuid()).ToList();
        var result = await Repository.GetRangeAsync(guids);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public virtual async Task GetRangeAsync_Existing_ReturnsExpected()
    {
        var entities = Enumerable.Range(0, 10).Select(_ => CreateEntity()).ToList();
        var guids = entities.Select(e => e.Guid).ToList();

        EntityContext.AddRange(entities);
        await EntityContext.CompleteAsync();

        var result = await Repository.GetRangeAsync(guids);
        Assert.That(entities.OrderBy(e => e.Id), Is.EqualTo(result.OrderBy(e => e.Id)).ByJsonDiff());
    }

    [Test]
    public virtual async Task AddAsync_ReturnsExpected()
    {
        var entity = CreateEntity();

        Repository.Add(entity);
        await EntityContext.CompleteAsync();

        var result = await Repository.GetAsync(entity.Guid);
        Assert.That(result, Is.EqualTo(entity).ByJsonDiff());
    }

    [Test]
    public virtual async Task AddRangeAsync_ReturnsExpected()
    {
        var entities = Enumerable.Range(0, 10).Select(_ => CreateEntity()).ToList();
        var guids = entities.Select(e => e.Guid).ToList();

        Repository.AddRange(entities);
        await EntityContext.CompleteAsync();

        var result = await Repository.GetRangeAsync(guids);
        Assert.That(entities.OrderBy(e => e.Id), Is.EqualTo(result.OrderBy(e => e.Id)).ByJsonDiff());
    }

    [Test]
    public virtual async Task RemoveAsync_ReturnsExpected()
    {
        var entity = CreateEntity();

        Repository.Add(entity);
        await EntityContext.CompleteAsync();

        Repository.Remove(entity);
        await EntityContext.CompleteAsync();

        var result = await Repository.GetAsync(entity.Guid);
        Assert.That(result, Is.Null);
    }

    [Test]
    public virtual async Task RemoveRangeAsync_ReturnsExpected()
    {
        var guids = Enumerable.Range(0, 10).Select(_ => Guid.NewGuid()).ToList();
        var entities = guids.Select(g => CreateEntity(g)).ToList();

        Repository.AddRange(entities);
        await EntityContext.CompleteAsync();

        Repository.RemoveRange(entities);
        await EntityContext.CompleteAsync();

        var result = await Repository.GetRangeAsync(guids);
        Assert.That(result, Is.Empty);
    }

    protected virtual TEntity CreateEntity(Guid? guid = null)
    {
        return new() { Guid = guid ?? Guid.NewGuid() };
    }
}