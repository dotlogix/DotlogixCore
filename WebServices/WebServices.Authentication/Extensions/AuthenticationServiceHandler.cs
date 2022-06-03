using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IAuthenticationService = DotLogix.WebServices.Authentication.Services.IAuthenticationService;

namespace DotLogix.WebServices.Authentication.Extensions; 

public class AuthenticationServiceHandler<TService> : AuthenticationHandler<AuthenticationSchemeOptions>
    where TService : IAuthenticationService {
    public AuthenticationServiceHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
    ) : base(options, logger, encoder, clock) {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
        var authService = Context.RequestServices.GetService<TService>();
        if(authService == null) {
            return AuthenticateResult.NoResult();
        }

        return await authService.AuthenticateAsync(Context);
    }
}
