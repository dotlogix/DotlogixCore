namespace DotLogix.Architecture.Domain.UoW {
    public interface IUnitOfWorkFactory {
        IUnitOfWork Create();
    }
}