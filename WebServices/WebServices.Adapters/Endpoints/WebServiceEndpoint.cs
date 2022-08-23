using System;

namespace DotLogix.WebServices.Adapters.Endpoints; 

public class WebServiceEndpoint : IWebServiceEndpoint
{
    private readonly WebServiceEndpointOptions _options;
    private readonly IWebServiceEndpoints _endpoints;

    public WebServiceEndpoint(WebServiceEndpointOptions options, IWebServiceEndpoints endpoints)
    {
        _options = options;
        _endpoints = endpoints;
    }

    public Uri Uri => new Uri(_options.GetUri(_endpoints.CurrentEndpointType), UriKind.Absolute);
}