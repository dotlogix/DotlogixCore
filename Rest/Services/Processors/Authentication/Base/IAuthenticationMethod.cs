using System.Threading.Tasks;
using DotLogix.Core.Rest.Server.Http;

namespace DotLogix.Core.Rest.Services.Processors.Authentication.Base
{
    public interface IAuthenticationMethod
    {
        string Name { get; }
        string[] SupportedDataFormats { get; }

        Task AuthenticateAsync(WebRequestResult webRequestResult, string authorizationData);
    }
}
