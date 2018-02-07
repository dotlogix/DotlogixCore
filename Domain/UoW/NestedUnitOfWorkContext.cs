using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Repositories;

namespace DotLogix.Architecture.Domain.UoW {
    public class NestedUnitOfWorkContext : IUnitOfWorkContext {
        private readonly IUnitOfWorkContext _parentContext;

        public NestedUnitOfWorkContext(IUnitOfWorkContext parentContext) {
            _parentContext = parentContext;
        }

        public IUnitOfWorkContext BeginContext() {
            return _parentContext.BeginContext();
        }

        public TRepo UseRepository<TRepo>() where TRepo : IRepository {
            return _parentContext.UseRepository<TRepo>();
        }

        public Task CompleteAsync() {
            return Task.CompletedTask;
        }

        public void Dispose() { }
    }
}