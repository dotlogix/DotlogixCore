using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors.Authentication.Base;

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Bearer {
    public class BearerAuthenticationMethod : AuthenticationMethodBase
    {
        private readonly ValidateBearerAsyncCallback _callbackAsync;


        public BearerAuthenticationMethod(ValidateBearerAsyncCallback callbackAsync, params string[] supportedDataFormats) : base("Bearer", supportedDataFormats)
        {
            _callbackAsync = callbackAsync;
        }

        public BearerAuthenticationMethod(ValidateBearerAsyncCallback callbackAsync) : this(callbackAsync, "[token]")
        {
        }


        public override Task AuthenticateAsync(WebRequestResult webRequestResult, string data)
        {
            return _callbackAsync.Invoke(this, webRequestResult, data);
        }
    }
}