using System;

namespace DotLogix.Architecture.Infrastructure.UoW {
    public interface IUnitOfWorkFactory {
        IUnitOfWork Create();
    }
}