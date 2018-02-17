using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Bearer {
    public delegate Task ValidateBearerAsyncCallback(BearerAuthenticationMethod authenticationMethod, WebRequestResult webRequestResult, string token);
}