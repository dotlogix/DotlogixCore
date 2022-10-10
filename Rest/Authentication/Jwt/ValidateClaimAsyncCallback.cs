using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Context;

namespace DotLogix.Core.Rest.Authentication.Jwt {
    public delegate Task ValidateClaimAsyncCallback<TClaim>(JwtAuthenticationMethod<TClaim> authenticationMethod, WebServiceContext context, TClaim token);
}