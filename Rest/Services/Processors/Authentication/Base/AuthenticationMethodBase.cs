using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.State;
using DotLogix.Core.Rest.Services.Exceptions;

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Base {
    public abstract class AuthenticationMethodBase : IAuthenticationMethod
    {
        protected AuthenticationMethodBase(string name, params string[] supportedDataFormats)
        {
            Name = name;
            SupportedDataFormats = supportedDataFormats;
        }

        public string Name { get; }
        public string[] SupportedDataFormats { get; }
        public abstract Task AuthenticateAsync(WebRequestResult webRequestResult, string data);

        public void SetUnauthorizedException(WebRequestResult webRequestResult, string message)
        {
            webRequestResult.SetException(new RestException(HttpStatusCodes.ClientError.Unauthorized, message));
        }

        public void SetInvalidFormatException(WebRequestResult webRequestResult)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("The data you provided for authorization is in an invalid format.");
            messageBuilder.Append(SupportedDataFormats.Length != 1 ? "Supported formats are:" : "The supported format is:");

            foreach (var supportedDataFormat in SupportedDataFormats)
                messageBuilder.Append($"\n\t{Name} {supportedDataFormat}");

            SetUnauthorizedException(webRequestResult, messageBuilder.ToString());
        }
    }
}