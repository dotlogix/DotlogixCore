using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Adapters.Endpoints;
using DotLogix.WebServices.Adapters.Http;
using Newtonsoft.Json;

namespace DotLogix.WebServices.Adapters.Client; 

public class ApiClient
{
    public IHttpProvider Provider { get; }

    public ApiClient(IHttpProvider httpProvider, IWebServiceEndpoint endpoint, string relativeUri)
    {
        Provider = httpProvider;
        Endpoint = endpoint;
        RelativeUri = relativeUri is not null ? new Uri(relativeUri, UriKind.Relative) : null;
    }

    public IWebServiceEndpoint Endpoint { get; set; }
    public JsonSerializerSettings SerializerSettings { get; private set; }
    public Uri RelativeUri { get; }
    public Uri AbsoluteBaseUri => new(Endpoint.Uri, RelativeUri);


    protected Task<HttpResponseMessage> GetAsync(string relativeUrl = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        var requestUri = relativeUrl == null ? AbsoluteBaseUri : new Uri(AbsoluteBaseUri, relativeUrl);
        return GetAsync(requestUri, cancellationToken, timeout);
    }

    protected Task<TResponse> GetAsync<TResponse>(string relativeUrl = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        var requestUri = relativeUrl == null ? AbsoluteBaseUri : new Uri(AbsoluteBaseUri, relativeUrl);
        return GetAsync<TResponse>(requestUri, cancellationToken, timeout);
    }

    protected Task<HttpResponseMessage> GetAsync(Uri absoluteUri, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        return GetResponseAsync(HttpMethod.Get, absoluteUri, (object) null, cancellationToken, timeout);
    }

    protected Task<TResponse> GetAsync<TResponse>(Uri absoluteUri, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        return GetResponseAsync<TResponse>(HttpMethod.Get, absoluteUri, null, cancellationToken, timeout);
    }

    protected Task<HttpResponseMessage> PutAsync(object request, string relativeUrl = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        var requestUri = relativeUrl == null ? AbsoluteBaseUri : new Uri(AbsoluteBaseUri, relativeUrl);
        return PutAsync(request, requestUri, cancellationToken, timeout);
    }

    protected Task<TResponse> PutAsync<TResponse>(object request, string relativeUrl = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        var requestUri = relativeUrl == null ? AbsoluteBaseUri : new Uri(AbsoluteBaseUri, relativeUrl);
        return PutAsync<TResponse>(request, requestUri, cancellationToken, timeout);
    }

    protected Task<HttpResponseMessage> PutAsync(object request, Uri absoluteUri, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        return GetResponseAsync(HttpMethod.Put, absoluteUri, request, cancellationToken, timeout);
    }

    protected Task<TResponse> PutAsync<TResponse>(object request, Uri absoluteUri, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        return GetResponseAsync<TResponse>(HttpMethod.Put, absoluteUri, request, cancellationToken, timeout);
    }

    protected Task<HttpResponseMessage> PostAsync(object request, string relativeUrl = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        var requestUri = relativeUrl == null ? AbsoluteBaseUri : new Uri(AbsoluteBaseUri, relativeUrl);
        return PostAsync(request, requestUri, cancellationToken, timeout);
    }

    protected Task<TResponse> PostAsync<TResponse>(object request, string relativeUrl = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        var requestUri = relativeUrl == null ? AbsoluteBaseUri : new Uri(AbsoluteBaseUri, relativeUrl);
        return PostAsync<TResponse>(request, requestUri, cancellationToken, timeout);
    }

    protected Task<HttpResponseMessage> PostAsync(object request, Uri absoluteUri, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        return GetResponseAsync(HttpMethod.Post, absoluteUri, request, cancellationToken, timeout);
    }

    protected Task<TResponse> PostAsync<TResponse>(object request, Uri absoluteUri, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        return GetResponseAsync<TResponse>(HttpMethod.Post, absoluteUri, request, cancellationToken, timeout);
    }

    protected Task<HttpResponseMessage> PatchAsync(object request, string relativeUrl = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        var requestUri = relativeUrl == null ? AbsoluteBaseUri : new Uri(AbsoluteBaseUri, relativeUrl);
        return PatchAsync(request, requestUri, cancellationToken, timeout);
    }

    protected Task<TResponse> PatchAsync<TResponse>(object request, string relativeUrl = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        var requestUri = relativeUrl == null ? AbsoluteBaseUri : new Uri(AbsoluteBaseUri, relativeUrl);
        return PatchAsync<TResponse>(request, requestUri, cancellationToken, timeout);
    }

    protected Task<HttpResponseMessage> PatchAsync(object request, Uri absoluteUri, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        return GetResponseAsync(new HttpMethod("PATCH"), absoluteUri, request, cancellationToken, timeout);
    }

    protected Task<TResponse> PatchAsync<TResponse>(object request, Uri absoluteUri, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        return GetResponseAsync<TResponse>(new HttpMethod("PATCH"), absoluteUri, request, cancellationToken, timeout);
    }

    protected async Task<bool> DeleteAsync(string relativeUrl = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        var requestUri = relativeUrl == null ? AbsoluteBaseUri : new Uri(AbsoluteBaseUri, relativeUrl);
        var response = await DeleteAsync(requestUri, cancellationToken, timeout);
        return response.IsSuccessStatusCode;
    }

    protected Task<HttpResponseMessage> DeleteAsync(Uri absoluteUri, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        return DeleteAsync(null, absoluteUri, cancellationToken, timeout);
    }

    protected Task<HttpResponseMessage> DeleteAsync(object request, string relativeUrl = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        var requestUri = relativeUrl == null ? AbsoluteBaseUri : new Uri(AbsoluteBaseUri, relativeUrl);
        return DeleteAsync(request, requestUri, cancellationToken, timeout);
    }

    protected Task<HttpResponseMessage> DeleteAsync(object request, Uri absoluteUri, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        return GetResponseAsync(HttpMethod.Delete, absoluteUri, request, cancellationToken, timeout);
    }

    protected Task<TResponse> DeleteAsync<TResponse>(object request, string relativeUrl = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        var requestUri = relativeUrl == null ? AbsoluteBaseUri : new Uri(AbsoluteBaseUri, relativeUrl);
        return DeleteAsync<TResponse>(request, requestUri, cancellationToken, timeout);
    }

    protected Task<TResponse> DeleteAsync<TResponse>(object request, Uri absoluteUri, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        return GetResponseAsync<TResponse>(HttpMethod.Delete, absoluteUri, request, cancellationToken, timeout);
    }

    protected Task<ICollection<TResponse>> BatchAsync<TRequest, TResponse>(IEnumerable<TRequest> requests, Func<TRequest, Task<TResponse>> requestAsync, int maxDegreeOfParallelism = -1, CancellationToken cancellationToken = default)
    {
        return requests.TransformAsync(requestAsync, maxDegreeOfParallelism, cancellationToken).WithAggregateException();
    }
    protected Task BatchAsync<TRequest>(IEnumerable<TRequest> requests, Func<TRequest, Task> requestAsync, int maxDegreeOfParallelism = -1, CancellationToken cancellationToken = default) {
        return requests.BatchAsync(requestAsync, maxDegreeOfParallelism, cancellationToken).WithAggregateException();
    }

    protected async Task<TResponse> GetResponseAsync<TResponse>(HttpMethod method, Uri requestUri, object requestObject = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        using var content = await ToHttpContentAsync(requestObject);
        using var request = new HttpRequestMessage(method, requestUri) { Content = content };
        using var response = await GetResponseAsync(request, cancellationToken, timeout);
        return await FromHttpContentAsync<TResponse>(response.Content);
    }

    protected async Task<HttpResponseMessage> GetResponseAsync(HttpMethod method, Uri requestUri, object requestObject = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        var content = await ToHttpContentAsync(requestObject);
        return await GetResponseAsync(method, requestUri, content, cancellationToken, timeout);
    }

    protected async Task<HttpResponseMessage> GetResponseAsync(HttpMethod method, Uri requestUri, HttpContent content = null, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        using var request = new HttpRequestMessage(method, requestUri);
        if (content is not null)
        {
            request.Content = content;
        }
        return await GetResponseAsync(request, cancellationToken, timeout);
    }

    protected virtual Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request, CancellationToken cancellationToken = default, TimeSpan? timeout = null)
    {
        return Provider.SendAsync(request, cancellationToken, timeout);
    }

    protected virtual async Task<TResponse> FromHttpContentAsync<TResponse>(HttpContent content)
    {
        if (content == null)
        {
            return default;
        }

        if (content is TResponse typedResponse)
        {
            return typedResponse;
        }

        if (content.Headers?.ContentType?.MediaType != "application/json")
        {
            var responseType = typeof(TResponse);
            if (responseType == typeof(byte[]))
            {
                return (TResponse) (object) await content.ReadAsByteArrayAsync();
            }

            if (responseType == typeof(Stream))
            {
                return (TResponse) (object) await content.ReadAsStreamAsync();
            }
        }

        var jsonString = await content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(jsonString, SerializerSettings);
    }

    protected virtual Task<HttpContent> ToHttpContentAsync(object requestObject)
    {
        switch (requestObject)
        {
            case null:
                return Task.FromResult<HttpContent>(default);
            case HttpContent content:
                return Task.FromResult(content);
            case byte[] bytes:
                return Task.FromResult<HttpContent>(new ByteArrayContent(bytes));
            case Stream stream:
                return Task.FromResult<HttpContent>(new StreamContent(stream));
            default:
                var jsonString = JsonConvert.SerializeObject(requestObject, SerializerSettings);
                var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                return Task.FromResult<HttpContent>(jsonContent);
        }
    }

    protected virtual void UseSerializerSettings(JsonSerializerSettings serializerSettings)
    {
        SerializerSettings = serializerSettings;
    }
}