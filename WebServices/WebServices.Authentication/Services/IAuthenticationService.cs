using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace DotLogix.WebServices.Authentication.Services {
    public interface IAuthenticationService {
        public Task<AuthenticateResult> AuthenticateAsync(string scheme, string parameter);
        public Task<AuthenticateResult> AuthenticateAnonymousAsync();
    }
}
