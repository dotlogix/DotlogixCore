using System.Threading.Tasks;
using DotLogix.Core.Extensions;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors.Authentication.Base;

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Basic {
    public class BasicAuthenticationMethod : AuthenticationMethodBase {
        private readonly ValidateUserAsyncCallback _callbackAsync;


        public BasicAuthenticationMethod(ValidateUserAsyncCallback callbackAsync) : base("Basic", "[username]:[password] encoded in bas64") {
            _callbackAsync = callbackAsync;
        }
        public override Task AuthenticateAsync(WebRequestResult webRequestResult, string data) {
            var dataSplit = data.FromBase64String().Split(':');
            if(dataSplit.Length == 2)
                return _callbackAsync.Invoke(this, webRequestResult, dataSplit[0], dataSplit[1]);

            SetInvalidFormatException(webRequestResult);
            return Task.CompletedTask;
        }
    }
}