// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  NpgsqlNamingConventionHistoryRepository.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 14.05.2022 19:31
// LastEdited:  14.05.2022 19:31
// ==================================================

using System.Diagnostics.CodeAnalysis;
using DotLogix.WebServices.EntityFramework.Conventions;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations.Internal;

namespace DotLogix.WebServices.EntityFramework.Database;

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
public class NpgsqlNamingConventionHistoryRepository : NpgsqlHistoryRepository {
    private readonly IDbNamingStrategy _namingStrategy;
    protected override string TableName => _namingStrategy.RewriteTableName(base.TableName);
    protected override string TableSchema => _namingStrategy.RewriteSchemaName(base.TableSchema);

    public NpgsqlNamingConventionHistoryRepository(HistoryRepositoryDependencies dependencies, IDbNamingStrategy namingStrategy) : base(dependencies) {
        _namingStrategy = namingStrategy;
    }
}
