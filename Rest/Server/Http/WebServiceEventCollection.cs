using DotLogix.Core.Collections;

namespace DotLogix.Core.Rest.Server.Http {
    public class WebServiceEventCollection : KeyedCollection<string, IWebServiceEvent> {
        public WebServiceEventCollection() : base(e => e.Name) { }
    }
}