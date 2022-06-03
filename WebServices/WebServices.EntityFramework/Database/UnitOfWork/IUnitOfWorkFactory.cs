namespace DotLogix.WebServices.EntityFramework.Database {
    /// <summary>
    /// An interface to represent factories for <see cref="Context.IEntityContext"/> modules
    /// </summary>
    public interface IUnitOfWorkFactory {
        IUnitOfWork Create();
    }
}