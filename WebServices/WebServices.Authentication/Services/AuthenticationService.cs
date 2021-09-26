using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using Microsoft.AspNetCore.Authentication;

namespace DotLogix.WebServices.Authentication.Services {
    public class AuthenticationService : IAuthenticationService {
        private readonly Dictionary<string, DynamicInvoke> _handlers = new Dictionary<string, DynamicInvoke>();
        public virtual async Task<AuthenticateResult> AuthenticateAsync(string scheme, string parameter) {
            if(_handlers.TryGetValue(scheme, out var handler) == false) {
                const BindingFlags flags = BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public;
                var paramTypes = typeof(string).CreateArray();
                handler = GetType()
                         .GetMethod($"Authenticate{scheme}Async", flags, default, default, paramTypes, default)
                        ?.CreateDynamicInvoke();
                _handlers.Add(scheme, handler);
            }

            return handler != null
                       ? await (Task<AuthenticateResult>)handler.Invoke(this, parameter)
                       : AuthenticateResult.NoResult();
        }

        public virtual Task<AuthenticateResult> AuthenticateAnonymousAsync() {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}