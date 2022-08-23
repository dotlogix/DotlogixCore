using DotLogix.WebServices.Adapters.Endpoints;

namespace DotLogix.WebServices.Testing.AspCore.Mocks; 

public class MockWebServiceEndpoints : IWebServiceEndpoints
{
    public EndpointType CurrentEndpointType { get; set; } = EndpointType.Local;

    public IWebServiceEndpoint CreateEndpoint(WebServiceEndpointOptions options)
    {
        return new WebServiceEndpoint(options, this);
    }
}