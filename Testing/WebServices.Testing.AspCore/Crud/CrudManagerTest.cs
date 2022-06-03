using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.WebServices.AspCore.Services;
using DotLogix.WebServices.Core.Errors;
using DotLogix.WebServices.Testing.NUnit;
using NUnit.Framework;

namespace DotLogix.WebServices.Testing.AspCore.Crud
{
    [TestFixture]
    [NonParallelizable]
    public abstract class CrudManagerTest<TEntity, TModel, TEnsure, TFilter, TService, TStartup>
        : CrudManagerTest<TEntity, TModel, TEnsure, TEnsure, TEnsure, TFilter, TService, TStartup>
        where TEntity : class, IGuid
        where TModel : class, IGuid
        where TEnsure : class, IGuid
        where TService : IDomainService<TModel, TEnsure, TEnsure, TEnsure, TFilter>
        where TStartup : class {
    }

    [TestFixture]
    [NonParallelizable]
    public abstract class CrudManagerTest<TEntity, TModel, TCreate, TEnsure, TPatch, TFilter, TService, TStartup>
        : ReadOnlyManagerTest<TEntity, TModel, TFilter, TService, TStartup>
        where TEntity : class, IGuid
        where TModel : class, IGuid
        where TCreate : class, IGuid
        where TPatch : class, IGuid
        where TEnsure : class, IGuid
        where TService : IDomainService<TModel, TCreate, TEnsure, TPatch, TFilter>
        where TStartup : class {
        [Test]
        public async Task CreateAsync_NonExisting_ReturnsExpected()
        {
            // setup
            var guid = Guid.NewGuid();
            var request = NewCreateRequest(guid);

            // create entity
            var model = await DomainService.CreateAsync(request);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());

            await EntityContext.CompleteAsync();
            RenewTestScope();

            // assert result
            model = await DomainService.GetAsync(guid);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());
        }

        [Test]
        public async Task CreateAsync_EmptyGuid_ReturnsExpected()
        {
            // setup
            var request = NewCreateRequest(Guid.Empty);

            // create entity
            var model = await DomainService.CreateAsync(request);
            request.Guid = model.Guid; // remember we created it with an empty guid
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());

            await EntityContext.CompleteAsync();
            RenewTestScope();

            // assert result
            model = await DomainService.GetAsync(model.Guid);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());
        }

        [Test]
        public void CreateAsync_Existing_ThrowsConflict()
        {
            // setup
            var guid = Entity.Guid;
            var request = NewCreateRequest(guid);

            // assert conflict
            var exception = Assert.CatchAsync<ApiException>(() => DomainService.CreateAsync(request));
            Assert.That(exception!.Kind, Is.EqualTo(ApiErrorKinds.Conflict));
        }

        [Test]
        public async Task CreateRangeAsync_NonExisting_ReturnsExpected()
        {
            // setup
            var guids = Enumerable.Range(0, 10)
                .Select(_ => Guid.NewGuid())
                .ToList();

            var requestMap = guids.ToDictionary(g => g, g => NewCreateRequest(g));

            // create entities
            var models = await DomainService.CreateRangeAsync(requestMap.Values);
            Assert.That(models.Count, Is.EqualTo(guids.Count));

            await EntityContext.CompleteAsync();
            RenewTestScope();

            // assert result
            models = await DomainService.GetRangeAsync(guids);
            Assert.That(models.Select(m => m.Guid), Is.EquivalentTo(requestMap.Keys));

            foreach (var model in models)
            {
                var request = requestMap[model.Guid];
                Assert.That(model, Is.EqualTo(request).ByJsonDiff());
            }
        }

        [Test]
        public async Task CreateRangeAsync_IncludesExisting_ThrowsConflict()
        {
            // setup
            var guids = Enumerable.Range(0, 10)
                .Select(_ => Guid.NewGuid())
                .ToList();
            guids.Add(Entity.Guid);

            var requestMap = guids.ToDictionary(g => g, g => NewCreateRequest(g));

            // assert conflict
            var previousModel = await DomainService.GetAsync(Entity.Guid);
            var exception = Assert.CatchAsync<ApiException>(() => DomainService.CreateRangeAsync(requestMap.Values));
            Assert.That(exception!.Kind, Is.EqualTo(ApiErrorKinds.Conflict));

            await EntityContext.CompleteAsync();
            RenewTestScope();

            // assert no changes
            var models = await DomainService.GetRangeAsync(guids);
            Assert.That(models.Count, Is.EqualTo(1));
            Assert.That(models.First(), Is.EqualTo(previousModel).ByJsonDiff());
        }

        [Test]
        public void PatchAsync_NonExisting_ThrowsNotFound()
        {
            // setup
            var guid = Guid.NewGuid();
            var request = NewPatchRequest(guid);

            // assert not found
            var exception = Assert.CatchAsync<ApiException>(() => DomainService.PatchAsync(request));
            Assert.That(exception!.Kind, Is.EqualTo(ApiErrorKinds.KeyNotFound));
        }

        [Test]
        public async Task PatchAsync_Existing_ReturnsExpected()
        {
            // setup
            var guid = Entity.Guid;
            var request = NewPatchRequest(guid);

            // patch entity
            var model = await DomainService.PatchAsync(request);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());

            await EntityContext.CompleteAsync();
            RenewTestScope();

            // assert result
            model = await DomainService.GetAsync(guid);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());
        }

        [Test]
        public void PatchRangeAsync_NonExisting_ThrowsNotFound()
        {
            // setup
            var guids = Enumerable.Range(0, 10)
                .Select(_ => Guid.NewGuid())
                .ToList();

            var requests = guids
                .Select(g => NewPatchRequest(g))
                .ToList();

            // assert not found
            var exception = Assert.CatchAsync<ApiException>(() => DomainService.PatchRangeAsync(requests));
            Assert.That(exception!.Kind, Is.EqualTo(ApiErrorKinds.KeyNotFound));
        }

        [Test]
        public async Task PatchRangeAsync_IncludesExisting_ThrowsNotFound()
        {
            // setup
            var guids = Enumerable.Range(0, 10)
                .Select(_ => Guid.NewGuid())
                .ToList();
            guids.Add(Entity.Guid);

            var requests = guids
                .Select(g => NewPatchRequest(g))
                .ToList();

            // assert not found
            var previousModel = await DomainService.GetAsync(Entity.Guid);
            var exception = Assert.CatchAsync<ApiException>(() => DomainService.PatchRangeAsync(requests));
            Assert.That(exception!.Kind, Is.EqualTo(ApiErrorKinds.KeyNotFound));

            await EntityContext.CompleteAsync();
            RenewTestScope();

            // assert no changes
            var models = await DomainService.GetRangeAsync(guids);
            Assert.That(models.Count, Is.EqualTo(1));
            Assert.That(models.First(), Is.EqualTo(previousModel).ByJsonDiff());
        }


        [Test]
        public async Task PatchRangeAsync_Existing_ReturnsExpected()
        {
            // setup
            var guids = new List<Guid> { Entity.Guid };

            var requests = guids
                .Select(g => NewPatchRequest(g))
                .ToList();

            // assert result
            var models = await DomainService.PatchRangeAsync(requests);
            Assert.That(models.Count, Is.EqualTo(1));
            Assert.That(models.First(), Is.EqualTo(requests.First()).ByJsonDiff());
        }

        [Test]
        public async Task EnsureAsync_NonExisting_ReturnsExpected()
        {
            // setup
            var guid = Guid.NewGuid();
            var request = NewEnsureRequest(guid);

            // ensure entity
            var model = await DomainService.EnsureAsync(request);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());

            await EntityContext.CompleteAsync();
            RenewTestScope();

            // assert result
            model = await DomainService.GetAsync(guid);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());
        }

        [Test]
        public async Task EnsureAsync_EmptyGuid_ReturnsExpected()
        {
            // setup
            var request = NewEnsureRequest(Guid.Empty);

            // ensure entity
            var model = await DomainService.EnsureAsync(request);
            request.Guid = model.Guid; // remember we created it with an empty guid
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());

            await EntityContext.CompleteAsync();
            RenewTestScope();

            // assert result
            model = await DomainService.GetAsync(model.Guid);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());
        }


        [Test]
        public async Task EnsureAsync_Existing_ReturnsExpected()
        {
            // setup
            var guid = Entity.Guid;
            var request = NewEnsureRequest(guid);

            // ensure entity
            var model = await DomainService.EnsureAsync(request);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());

            await EntityContext.CompleteAsync();
            RenewTestScope();

            // assert result
            model = await DomainService.GetAsync(guid);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());
        }

        [Test]
        public async Task EnsureRangeAsync_NonExisting_ReturnsExpected()
        {
            // setup
            var guids = Enumerable.Range(0, 10)
                .Select(_ => Guid.NewGuid())
                .ToList();

            var requestMap = guids.ToDictionary(g => g, g => NewEnsureRequest(g));

            // ensure entities
            var models = await DomainService.EnsureRangeAsync(requestMap.Values);
            Assert.That(models.Count, Is.EqualTo(guids.Count));

            await EntityContext.CompleteAsync();
            RenewTestScope();

            // assert result
            models = await DomainService.GetRangeAsync(guids);
            foreach (var model in models)
            {
                var request = requestMap[model.Guid];
                Assert.That(model, Is.EqualTo(request).ByJsonDiff());
            }
        }

        [Test]
        public async Task EnsureRangeAsync_IncludesExisting_ReturnsExpected()
        {
            // setup
            var guids = Enumerable.Range(0, 10)
                .Select(_ => Guid.NewGuid())
                .ToList();
            guids.Add(Entity.Guid);

            var requestMap = guids.ToDictionary(g => g, g => NewEnsureRequest(g));

            // ensure entities
            var models = await DomainService.EnsureRangeAsync(requestMap.Values);
            Assert.That(models.Count, Is.EqualTo(guids.Count));

            await EntityContext.CompleteAsync();
            RenewTestScope();

            // assert result
            models = await DomainService.GetRangeAsync(guids);
            foreach (var model in models)
            {
                var request = requestMap[model.Guid];
                Assert.That(model, Is.EqualTo(request).ByJsonDiff());
            }
        }

        [Test]
        public async Task RemoveAsync_NonExisting_ThrowsNotFound()
        {
            // setup
            var guid = Guid.NewGuid();

            // Remove entity
            var result = await DomainService.RemoveAsync(guid);

            // assert not found
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RemoveAsync_Existing_ReturnsExpected()
        {
            // setup
            var guid = Entity.Guid;

            // Remove entity
            var result = await DomainService.RemoveAsync(guid);

            // assert result
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task RemoveRangeAsync_NonExisting_ThrowsNotFound()
        {
            // setup
            var guid = Guid.NewGuid();

            // Remove entities
            var result = await DomainService.RemoveRangeAsync(new[] { guid });

            // assert not found
            Assert.That(result, Is.Zero);
        }

        [Test]
        public async Task RemoveRangeAsync_Existing_ReturnsExpected()
        {
            // setup
            var guid = Entity.Guid;

            // Remove entities
            var result = await DomainService.RemoveRangeAsync(new[] { guid });

            // assert result
            Assert.That(result, Is.EqualTo(1));
        }

        protected abstract TCreate NewCreateRequest(Guid? guid = default);
        protected abstract TPatch NewPatchRequest(Guid? guid = default);
        protected abstract TEnsure NewEnsureRequest(Guid? guid = default);
    }
}