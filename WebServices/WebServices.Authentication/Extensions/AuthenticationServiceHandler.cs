using System.Linq;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using IAuthenticationService = DotLogix.WebServices.Authentication.Services.IAuthenticationService;

namespace DotLogix.WebServices.Authentication.Extensions {
    public class AuthenticationServiceHandler : AuthenticationHandler<AuthenticationServiceOptions> { 
        public AuthenticationServiceHandler(IOptionsMonitor<AuthenticationServiceOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock) {
        }
        
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            var authService = Context.RequestServices.GetService<IAuthenticationService>();
            if(authService == null) {
                return AuthenticateResult.NoResult();
            }
            
            if(Context.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() != null) {
                return await authService.AuthenticateAnonymousAsync();
            }
            
            var authHeaderStr = Request.Headers[HeaderNames.Authorization];
            if (AuthenticationHeaderValue.TryParse(authHeaderStr.FirstOrDefault(), out var authHeader) == false)
            {
                return AuthenticateResult.NoResult();
            }
            
            return await authService.AuthenticateAsync(authHeader.Scheme, authHeader.Parameter);
        }
    }
}