using DotLogix.Core.Collections;
using DotLogix.Core.Rest.Services;

namespace DotLogix.Core.Rest.Server.Http {
    public class WebServiceCollection : KeyedCollection<string, IWebService> {
        /// <inheritdoc />
        public WebServiceCollection() : base(e => e.Name) { }
    }
}