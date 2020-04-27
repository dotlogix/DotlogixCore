using DotLogix.Core.Collections;

namespace DotLogix.Core.Rest.Services {
    public class WebServiceCollection : KeyedCollection<string, IWebService> {
        /// <inheritdoc />
        public WebServiceCollection() : base(e => e.Name) { }
    }
}