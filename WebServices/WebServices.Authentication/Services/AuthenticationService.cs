using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DotLogix.WebServices.Authentication.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace DotLogix.WebServices.Authentication.Services; 

public class AuthenticationService : IAuthenticationService {
    private readonly IOptions<AuthOptions> _options;
    protected AuthOptions Options => _options?.Value;

    public AuthenticationService(IOptions<AuthOptions> options) {
        _options = options;
    }

    public Task<AuthenticateResult> AuthenticateAsync(HttpContext httpContext) {
        var request = httpContext.Request;
        if(request.Headers.TryGetValue(HeaderNames.Authorization, out var authHeaderString) == false) {
            return AuthenticateAnonymousAsync(httpContext);
        }
            
        if(AuthenticationHeaderValue.TryParse(authHeaderString.FirstOrDefault(), out var authHeader) == false) {
            return AuthenticateAsync(httpContext, authHeader!.Scheme, authHeader!.Parameter);
        }
        return Task.FromResult(AuthenticateResult.NoResult());
    }

    protected virtual Task<AuthenticateResult> AuthenticateAsync(HttpContext httpContext, string scheme, string parameter) {
        return Task.FromResult(AuthenticateResult.NoResult());
    }

    protected virtual Task<AuthenticateResult> AuthenticateAnonymousAsync(HttpContext httpContext) {
        return Task.FromResult(AuthenticateResult.NoResult());
    }
}