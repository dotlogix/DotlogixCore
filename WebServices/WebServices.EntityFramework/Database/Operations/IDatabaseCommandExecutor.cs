using System;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.WebServices.EntityFramework.Context;

namespace DotLogix.WebServices.EntityFramework.Database; 

public interface IDatabaseCommandExecutor {
    string Name { get; }
    Task RunAsync(IServiceProvider serviceProvider, IEntityContext entityContext, CancellationToken cancellationToken = default);
}