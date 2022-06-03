using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.WebServices.Adapters.Client;
using DotLogix.WebServices.Core.Errors;
using DotLogix.WebServices.Testing.NUnit;
using NUnit.Framework;

namespace DotLogix.WebServices.Testing.AspCore.Crud
{
    [TestFixture]
    [NonParallelizable]
    public abstract class CrudIntegrationTest<TEntity, TModel, TEnsure, TFilter, TClient, TStartup>
        : CrudIntegrationTest<TEntity, TModel, TEnsure, TEnsure, TEnsure, TFilter, TClient, TStartup>
        where TEntity : class, IGuid
        where TModel : class, IGuid
        where TEnsure : class, IGuid, new()
        where TClient : ICrudApiClient<TModel, TEnsure, TEnsure, TEnsure, TFilter>
        where TStartup : class {
    }

    [TestFixture]
    [NonParallelizable]
    public abstract class CrudIntegrationTest<TEntity, TModel, TCreate, TEnsure, TPatch, TFilter, TClient, TStartup>
        : ReadOnlyIntegrationTest<TEntity, TModel, TFilter, TClient, TStartup>
        where TEntity : class, IGuid
        where TModel : class, IGuid
        where TCreate : class, IGuid, new()
        where TPatch : class, IGuid, new()
        where TEnsure : IGuid
        where TClient : ICrudApiClient<TModel, TCreate, TEnsure, TPatch, TFilter>
        where TStartup : class {
        [Test]
        public async Task CreateAsync_NonExisting_ReturnsExpected()
        {
            // setup
            var guid = Guid.NewGuid();
            var request = NewCreateRequest(guid);

            // create entity
            var model = await ApiClient.CreateAsync(request);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());

            RenewTestScope();

            // assert result
            model = await ApiClient.GetAsync(guid);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());
        }

        [Test]
        public async Task CreateAsync_EmptyGuid_ReturnsExpected()
        {
            // setup
            var request = NewCreateRequest(Guid.Empty);

            // create entity
            var model = await ApiClient.CreateAsync(request);
            request.Guid = model.Guid; // remember we created it with an empty guid
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());

            RenewTestScope();

            // assert result
            model = await ApiClient.GetAsync(model.Guid);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());
        }

        [Test]
        public void CreateAsync_Existing_ThrowsConflict()
        {
            // setup
            var guid = Entity.Guid;
            var request = NewCreateRequest(guid);

            // assert conflict
            var exception = Assert.CatchAsync<ApiClientException>(() => ApiClient.CreateAsync(request));
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
            var models = await ApiClient.CreateRangeAsync(requestMap.Values);
            Assert.That(models.Count, Is.EqualTo(guids.Count));

            RenewTestScope();

            // assert result
            models = await ApiClient.GetRangeAsync(guids);
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
            var previousModel = await ApiClient.GetAsync(Entity.Guid);
            var exception = Assert.CatchAsync<ApiClientException>(() => ApiClient.CreateRangeAsync(requestMap.Values));
            Assert.That(exception!.Kind, Is.EqualTo(ApiErrorKinds.Conflict));

            RenewTestScope();

            // assert no changes
            var models = await ApiClient.GetRangeAsync(guids);
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
            var exception = Assert.CatchAsync<ApiClientException>(() => ApiClient.PatchAsync(request));
            Assert.That(exception!.Kind, Is.EqualTo(ApiErrorKinds.KeyNotFound));
        }

        [Test]
        public async Task PatchAsync_Existing_ReturnsExpected()
        {
            // setup
            var guid = Entity.Guid;
            var request = NewPatchRequest(guid);

            // patch entity
            var model = await ApiClient.PatchAsync(request);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());

            RenewTestScope();

            // assert result
            model = await ApiClient.GetAsync(guid);
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
            var exception = Assert.CatchAsync<ApiClientException>(() => ApiClient.PatchRangeAsync(requests));
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
            var previousModel = await ApiClient.GetAsync(Entity.Guid);
            var exception = Assert.CatchAsync<ApiClientException>(() => ApiClient.PatchRangeAsync(requests));
            Assert.That(exception!.Kind, Is.EqualTo(ApiErrorKinds.KeyNotFound));

            RenewTestScope();

            // assert no changes
            var models = await ApiClient.GetRangeAsync(guids);
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
            var models = await ApiClient.PatchRangeAsync(requests);
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
            var model = await ApiClient.EnsureAsync(request);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());

            RenewTestScope();

            // assert result
            model = await ApiClient.GetAsync(guid);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());
        }

        [Test]
        public async Task EnsureAsync_EmptyGuid_ReturnsExpected()
        {
            // setup
            var request = NewEnsureRequest(Guid.Empty);

            // ensure entity
            var model = await ApiClient.EnsureAsync(request);
            request.Guid = model.Guid; // remember we created it with an empty guid
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());

            RenewTestScope();

            // assert result
            model = await ApiClient.GetAsync(model.Guid);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());
        }


        [Test]
        public async Task EnsureAsync_Existing_ReturnsExpected()
        {
            // setup
            var guid = Entity.Guid;
            var request = NewEnsureRequest(guid);

            // ensure entity
            var model = await ApiClient.EnsureAsync(request);
            Assert.That(model, Is.EqualTo(request).ByJsonDiff());

            RenewTestScope();

            // assert result
            model = await ApiClient.GetAsync(guid);
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
            var models = await ApiClient.EnsureRangeAsync(requestMap.Values);
            Assert.That(models.Count, Is.EqualTo(guids.Count));

            RenewTestScope();

            // assert result
            models = await ApiClient.GetRangeAsync(guids);
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
            var models = await ApiClient.EnsureRangeAsync(requestMap.Values);
            Assert.That(models.Count, Is.EqualTo(guids.Count));

            RenewTestScope();

            // assert result
            models = await ApiClient.GetRangeAsync(guids);
            foreach (var model in models)
            {
                var request = requestMap[model.Guid];
                Assert.That(model, Is.EqualTo(request).ByJsonDiff());
            }
        }

        [Test]
        public async Task DeleteAsync_NonExisting_ThrowsNotFound()
        {
            // setup
            var guid = Guid.NewGuid();

            // delete entity
            var result = await ApiClient.DeleteAsync(guid);

            // assert not found
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAsync_Existing_ReturnsExpected()
        {
            // setup
            var guid = Entity.Guid;

            // delete entity
            var result = await ApiClient.DeleteAsync(guid);

            // assert not found
            Assert.That(result, Is.True);
        }

        protected abstract TCreate NewCreateRequest(Guid? guid = default);
        protected abstract TPatch NewPatchRequest(Guid? guid = default);
        protected abstract TEnsure NewEnsureRequest(Guid? guid = default);
    }
}