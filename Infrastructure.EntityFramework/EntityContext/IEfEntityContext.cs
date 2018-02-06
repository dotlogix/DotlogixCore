using DotLogix.Architecture.Infrastructure.EntityContext;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.Architecture.Infrastructure.EntityFramework.EntityContext {
    public interface IEfEntityContext : IEntityContext {
        DbContext DbContext { get; }
    }
}