#region using
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace DotLogix.WebServices.Adapters.Http; 

public interface IHttpProvider : IDisposable
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default, TimeSpan? timeout = null);
}