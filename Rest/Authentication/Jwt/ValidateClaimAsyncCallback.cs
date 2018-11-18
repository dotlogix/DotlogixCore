using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;

namespace DotLogix.Core.Rest.Authentication.Jwt {
    public delegate Task ValidateClaimAsyncCallback<TClaim>(JwtAuthenticationMethod<TClaim> authenticationMethod, WebRequestResult webRequestResult, TClaim token);
}