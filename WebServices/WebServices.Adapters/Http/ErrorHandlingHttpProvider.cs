using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.WebServices.Core.Errors;
using Newtonsoft.Json;

namespace DotLogix.WebServices.Adapters.Http
{
    public class ErrorHandlingHttpProvider : IHttpProvider
    {
        private readonly IHttpProvider _innerProvider;

        public ErrorHandlingHttpProvider(IHttpProvider innerProvider)
        {
            _innerProvider = innerProvider;
        }

        public virtual async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken = default,
            TimeSpan? timeout = null
        ) {
            var response = await _innerProvider.SendAsync(request, cancellationToken, timeout);

            if (!response.IsSuccessStatusCode)
            {
                await OnHttpErrorAsync(response);
            }

            return response;
        }


        protected async Task OnHttpErrorAsync(HttpResponseMessage response)
        {
            var apiError = await ParseApiErrorAsync(response);
            throw new ApiClientException(apiError, response);
        }


        protected static async Task<ApiError> ParseApiErrorAsync(HttpResponseMessage message)
        {
            if (message.IsSuccessStatusCode)
            {
                return null;
            }

            var headers = message.Content.Headers;
            var contentType = headers.ContentType?.MediaType;
            const string jsonMediaType = "application/json";
            const string textMediaType = "text/plain";

            var canParse = headers.ContentLength is > 0 && contentType is jsonMediaType or textMediaType;
            if (canParse == false)
            {
                return GetDefaultError(message);
            }

            var messageText = await message.Content.ReadAsStringAsync();

            ApiError apiError = null;
            if (contentType == jsonMediaType)
            {
                apiError = GetJsonError(messageText);
            }

            return apiError ?? GetPlainError(message, messageText);
        }

        protected static ApiError GetJsonError(string jsonMessage)
        {
            try {
                return JsonConvert.DeserializeObject<ApiError>(jsonMessage);
            } catch {
                return null;
            }
        }

        protected static ApiError GetPlainError(HttpResponseMessage message, string messageText)
        {
            var error = GetDefaultError(message);
            error.Message = string.Concat(error.Message, "\n\n", messageText);
            return error;
        }

        protected static ApiError GetDefaultError(HttpResponseMessage message) {
            var statusCode = message.StatusCode;
            var reasonPhrase = message.ReasonPhrase ?? statusCode.ToString();
            var errorKind = GetFallbackErrorKind(statusCode);
            var requestUri = message.RequestMessage.RequestUri;
            return new ApiError
            {
                Kind = errorKind,
                Message = $"Http request failed with status code {statusCode:D} {reasonPhrase} at url {requestUri}"
            };
        }

        protected static string GetFallbackErrorKind(HttpStatusCode statusCode) {
            return statusCode switch {
                HttpStatusCode.BadRequest => ApiErrorKinds.Validation,
                HttpStatusCode.Conflict => ApiErrorKinds.Conflict,
                _ => statusCode.ToString()
            };
        }

        protected virtual void Dispose(bool disposing) {
            if(!disposing) return;
            _innerProvider?.Dispose();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
