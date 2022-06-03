using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DotLogix.WebServices.EntityFramework.Database.Extensions;

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
public static class DbContextExtensions {
    public static IEntityQueryCompiler GetEntityQueryCompiler(this DbContext dbContext) {
        return dbContext.GetService<IQueryCompiler>() as IEntityQueryCompiler;
    }
    public static IStateManager GetStateManager(this DbContext dbContext) {
        return dbContext.GetDependencies().StateManager;
    }
}