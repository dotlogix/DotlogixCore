#region using
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace DotLogix.WebServices.Adapters.Http
{
    public class HttpProvider : IHttpProvider {
        protected TimeSpan DefaultTimeout { get; }
        protected HttpClient HttpClient { get; }
        protected IAuthenticationTokenProvider TokenProvider { get; }

        public HttpProvider(HttpClient httpClient, IAuthenticationTokenProvider tokenProvider = null, TimeSpan? defaultTimeout = null) {
            HttpClient = httpClient;
            TokenProvider = tokenProvider;
            DefaultTimeout = defaultTimeout ?? TimeSpan.FromSeconds(100);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default, TimeSpan? timeout = null) {
            HttpClient.Timeout = timeout ?? DefaultTimeout;
            if(request.Headers.Authorization is null && TokenProvider?.GetAuthenticationToken(request.RequestUri) is { } token) {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            }
            return await HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        protected virtual void Dispose(bool disposing) {
            if(!disposing) return;
            HttpClient?.Dispose();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}