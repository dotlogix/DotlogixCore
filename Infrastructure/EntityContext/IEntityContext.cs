using System;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Entities;

namespace DotLogix.Architecture.Infrastructure.EntityContext
{
    public interface IEntityContext : IDisposable {
        Task CompleteAsync();
    }
}
