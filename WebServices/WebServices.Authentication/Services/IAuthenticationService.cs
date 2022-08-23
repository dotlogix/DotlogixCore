using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DotLogix.WebServices.Authentication.Services; 

public interface IAuthenticationService {
    public Task<AuthenticateResult> AuthenticateAsync(HttpContext httpContext);
}