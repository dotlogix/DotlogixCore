using System.Threading.Tasks;
using DotLogix.Core.Rest.Services;

namespace DotLogix.Core.Rest.Authentication.Jwt {
    public delegate Task ValidateClaimAsyncCallback<TClaim>(JwtAuthenticationMethod<TClaim> authenticationMethod, WebServiceContext context, TClaim token);
}