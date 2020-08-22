using System;
using DotLogix.Core.Rest.Http;
using DotLogix.Core.Rest.Http.Headers;

namespace DotLogix.Core.Rest.Services {
    public class RedirectWebServiceResult : IRedirectWebServiceResult {
        /// <inheritdoc />
        public string RedirectTo { get; set; }

        /// <inheritdoc />
        public bool Permanent { get; set; }

        /// <inheritdoc />
        public bool PreserveMethod { get; set; }

        /// <inheritdoc />
        public HttpStatusCode StatusCode => CalculateStatusCode();

        private HttpStatusCode CalculateStatusCode() {
            if(PreserveMethod) {
                return Permanent
                           ? HttpStatusCodes.Redirect.PermanentRedirect
                           : HttpStatusCodes.Redirect.TemporaryRedirect;
            }

            return Permanent
                       ? HttpStatusCodes.Redirect.MovedPermanently
                       : HttpStatusCodes.Redirect.FoundMovedTemporarily;


        }

        /// <inheritdoc />
        public MimeType ContentType => null;

        /// <inheritdoc />
        public IWebServiceResultWriter ResultWriter { get; } = RedirectResultWriter.Instance;

        /// <inheritdoc />
        public Optional<Exception> Exception => null;
    }
}