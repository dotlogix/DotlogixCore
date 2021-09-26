#region
using DotLogix.Core.Caching;
using DotLogix.Infrastructure.EntityFramework;
using DotLogix.WebServices.Core;
#endregion

namespace DotLogix.WebServices.EntityFramework.Context {
    public interface IWebServiceEntityContext : IEfEntityContext {
        ICache<object, object> EntityCache { get; }
    }
}
