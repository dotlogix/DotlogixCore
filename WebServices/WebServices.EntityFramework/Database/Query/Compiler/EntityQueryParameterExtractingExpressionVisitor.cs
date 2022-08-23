// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  EntityQueryParameterExtractingExpressionVisitor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 25.06.2022 02:42
// LastEdited:  25.06.2022 02:42
// ==================================================

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DotLogix.WebServices.EntityFramework.Database; 

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
public class EntityQueryParameterExtractingExpressionVisitor : ParameterExtractingExpressionVisitor {
    private readonly IEntityQueryRewriterPlugin _rewriter;

    public EntityQueryParameterExtractingExpressionVisitor(
        IEntityQueryRewriterPlugin rewriter,
        IEvaluatableExpressionFilter evaluatableExpressionFilter,
        IParameterValues parameterValues,
        Type contextType,
        IModel model,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger,
        bool parameterize,
        bool generateContextAccessors
    ) : base(evaluatableExpressionFilter, parameterValues, contextType, model, logger, parameterize, generateContextAccessors) {
        _rewriter = rewriter;
    }

    public override Expression ExtractParameters(Expression expression) {
        expression = _rewriter.Rewrite(expression);
        return base.ExtractParameters(expression);
    }
}