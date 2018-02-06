using DotLogix.Architecture.Domain.Services;

namespace DotLogix.Architecture.Application.Context {
    public interface IApplicationContext
    {
        TService UseService<TService>() where TService : class, IDomainService;
    }
}