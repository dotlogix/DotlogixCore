// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  NpgsqlRelationalEntityDbOperations.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 20.11.2021 01:04
// LastEdited:  20.11.2021 01:04
// ==================================================

using System.Threading;
using System.Threading.Tasks;
using DotLogix.WebServices.EntityFramework.Database;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Extensions; 

public class RelationalEntityDbOperations : NpgsqlEntityDbOperations {
    public RelationalEntityDbOperations(DbContext dbContext)
        : base(dbContext) {
    }

    public override Task CreateAsync(CancellationToken cancellationToken = default) {
        return base.CreateAsync(cancellationToken)
           .ContinueWith(_ => DbContext.Database.NpgSqlReloadTypesAsync(), cancellationToken);
    }

    public override Task ClearAsync(CancellationToken cancellationToken = default) {
        return base.ClearAsync(cancellationToken)
           .ContinueWith(_ => DbContext.Database.NpgSqlReloadTypesAsync(), cancellationToken);
    }

    public override Task DeleteAsync(CancellationToken cancellationToken = default) {
        return base.DeleteAsync(cancellationToken)
           .ContinueWith(_ => DbContext.Database.NpgSqlReloadTypesAsync(), cancellationToken);
    }

    public override Task RecreateAsync(CancellationToken cancellationToken = default) {
        return base.RecreateAsync(cancellationToken)
           .ContinueWith(_ => DbContext.Database.NpgSqlReloadTypesAsync(), cancellationToken);
    }

    public override Task MigrateAsync(CancellationToken cancellationToken = default) {
        return base.MigrateAsync(cancellationToken)
           .ContinueWith(_ => DbContext.Database.NpgSqlReloadTypesAsync(), cancellationToken);
    }
}