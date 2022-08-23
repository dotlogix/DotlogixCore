using System;

namespace DotLogix.WebServices.Adapters.Endpoints; 

public class StaticWebServiceEndpoint : IWebServiceEndpoint
{
    public Uri Uri { get; }

    public StaticWebServiceEndpoint(Uri uri)
    {
        Uri = uri;
    }

    public StaticWebServiceEndpoint(string url) : this(new Uri(url))
    {
    }
}