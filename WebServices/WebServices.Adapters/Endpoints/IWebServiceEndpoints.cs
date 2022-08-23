namespace DotLogix.WebServices.Adapters.Endpoints; 

public interface IWebServiceEndpoints
{
    EndpointType CurrentEndpointType
    {
        get;
#if DEBUG
        set;
#endif
    }

    IWebServiceEndpoint CreateEndpoint(WebServiceEndpointOptions options);
}