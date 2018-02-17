using System;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Token {
    public delegate Task ValidateTokenAsyncCallback(TokenAuthenticationMethod authenticationMethod, WebRequestResult webRequestResult, Guid token);
}