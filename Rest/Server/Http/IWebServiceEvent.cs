using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Routes;

namespace DotLogix.Core.Rest.Server.Http {
    public interface IWebServiceEvent {
        string Name { get; }

        void Subscribe(IAsyncHttpContext context, IWebServiceRoute route, AsyncWebRequestRouter router);
        Task TriggerAsync(object sender, WebServiceEventArgs eventArgs);
    }
}