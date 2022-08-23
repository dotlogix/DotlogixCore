using System.Collections.Generic;

namespace DotLogix.WebServices.Adapters.Endpoints; 

public class WebServiceEndpointOptions
{
    private readonly Dictionary<EndpointType, string> _endpointUris;

    public WebServiceEndpointOptions(Dictionary<EndpointType, string> endpointUris)
    {
        _endpointUris = endpointUris;
    }

    public WebServiceEndpointOptions()
    {
        _endpointUris = new Dictionary<EndpointType, string>();
    }

    public string Production
    {
        get => GetUri(EndpointType.Production);
        set => SetUri(EndpointType.Production, value);
    }

    public string Sandbox
    {
        get => GetUri(EndpointType.Sandbox);
        set => SetUri(EndpointType.Sandbox, value);
    }

    public string Local
    {
        get => GetUri(EndpointType.Local);
        set => SetUri(EndpointType.Local, value);
    }

    public string GetUri(EndpointType endpointType)
    {
        return _endpointUris.TryGetValue(endpointType, out var uri) ? uri : null;
    }

    public void SetUri(EndpointType endpointType, string uri)
    {
        _endpointUris[endpointType] = uri;
    }

    public static implicit operator WebServiceEndpointOptions(Dictionary<EndpointType, string> endpoints)
    {
        return new WebServiceEndpointOptions(endpoints);
    }
}