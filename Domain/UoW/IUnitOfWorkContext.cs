using System;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Repositories;

namespace DotLogix.Architecture.Domain.UoW
{
    public interface IUnitOfWorkContext : IUnitOfWorkContextFactory, IDisposable {
        TRepo UseRepository<TRepo>() where TRepo : IRepository;
        Task CompleteAsync();
    }
}
