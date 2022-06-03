using System;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.WebServices.AspCore.Services;
using DotLogix.WebServices.Core.Errors;
using NUnit.Framework;

namespace DotLogix.WebServices.Testing.AspCore.Crud
{
    public abstract class ReadOnlyManagerTest<TEntity, TModel, TFilter, TService, TStartup> : WebServiceTestBase<TStartup>
        where TEntity : class, IGuid
        where TModel : class, IGuid
        where TService : IReadOnlyDomainService<TModel, TFilter>
        where TStartup : class
    {
        protected TService DomainService => CreateDomainService();
        protected TEntity Entity { get; set; }

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

            var exception = Assert.CatchAsync<ApiException>(() => DomainService.GetAsync(guid));
            Assert.That(exception!.Kind, Is.EqualTo(ApiErrorKinds.KeyNotFound));
        }

        [Test]
        public async Task GetAsync_Existing_ReturnsExpected()
        {
            var guid = Entity.Guid;

            var result = await DomainService.GetAsync(guid);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetRangeAsync_NonExisting_ThrowsNotFound()
        {
            var entities = await DomainService.GetRangeAsync(new[] { Guid.NewGuid() });
            Assert.That(entities, Is.Empty);
        }

        [Test]
        public async Task GetRangeAsync_Existing_ReturnsExpected()
        {
            var result = await DomainService.GetRangeAsync(new[] { Entity.Guid });
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task GetAllAsync_Existing_ReturnsExpected()
        {
            var result = await DomainService.GetAllAsync();
            Assert.That(result, Is.Not.Empty);
        }

        protected virtual TService CreateDomainService()
        {
            return GetRequiredService<TService>();
        }

        protected abstract TEntity NewEntity(Guid? guid = default);
    }
}