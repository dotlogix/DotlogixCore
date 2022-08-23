namespace DotLogix.WebServices.Adapters.Endpoints; 

public class WebServicesEndpoints : IWebServiceEndpoints
{
    private EndpointType _currentEndpointType = EndpointType.Production;

    public EndpointType CurrentEndpointType
    {
        get => _currentEndpointType;
#if DEBUG
        set => SetCurrentEndpointType(value);
#endif
    }

    protected void SetCurrentEndpointType(EndpointType endpointType)
    {
        _currentEndpointType = endpointType;
    }

    public IWebServiceEndpoint CreateEndpoint(WebServiceEndpointOptions options)
    {
        return new WebServiceEndpoint(options, this);
    }
}