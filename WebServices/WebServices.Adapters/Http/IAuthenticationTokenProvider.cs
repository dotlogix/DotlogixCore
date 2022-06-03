using System;

namespace DotLogix.WebServices.Adapters.Http
{
    public interface IAuthenticationTokenProvider
    {
        string GetAuthenticationToken(Uri url);
    }
}