namespace DotLogix.Architecture.Domain.UoW
{
    public interface IUnitOfWorkContextFactory
    {
        IUnitOfWorkContext BeginContext();
    }
}
