using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotLogix.WebServices.EntityFramework.Context;
using DotLogix.WebServices.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

[SuppressMessage("Usage", "EF1001:Internal EF Core API usage.")]
public class SqlQueryBuilder : ISqlQueryBuilder {
    private static readonly Regex ParameterNameRegex = new("@(?:(?:__(\\w+?)_\\d+?)|(.+?))\\b");
    private static readonly Regex ParameterRegex = new("@.+?\\b");
    private static readonly Regex ReferenceRegex = new("<<[0-9A-Za-z]{32}>>");
    
    private readonly IEntityContext _entityContext;
    private readonly Dictionary<string, ISqlCte> _referenceMap = new();
    private readonly List<ISqlCte> _references = new();

    public SqlQueryBuilder(IEntityContext entityContext) {
        _entityContext = entityContext;
    }

    public ISqlQueryBuilder UseCte(Func<ISqlQueryContext, ISqlCte> cteFunc) {
        var context = new SqlQueryContext(_entityContext, _referenceMap);
        var cte = cteFunc.Invoke(context);
        _referenceMap.Add(cte.Name, cte);
        _references.Add(cte);
        return this;
    }

    public IQueryable<TResult> UseQuery<TResult>(Func<ISqlQueryContext, IQueryable<TResult>> queryFunc) where TResult : class {
        var context = new SqlQueryContext(_entityContext, _referenceMap);
        return Build(queryFunc.Invoke(context));
    }

    private IQueryable<TResult> Build<TResult>(IQueryable<TResult> query) where TResult : class {
        if(_references.Count == 0) {
            return query;
        }

        var recursive = _references.Any(r => r.Recursive);
        var parameters = new List<DbParameter>();

        var sb = new StringBuilder();
        var parameterNameUsages = new Dictionary<string, int>();
        var aliasMap = new Dictionary<string, string>();

        sb.Append("WITH ");
        if(recursive) sb.Append("RECURSIVE ");

        var first = true;
        foreach(var reference in _references) {
            aliasMap.Add(reference.Alias, reference.Name);

            if(first == false) sb.Append(", ");
            first = false;
            sb.Append(reference.Name);
            sb.AppendLine(" AS (");
            AppendCommand(sb, reference.Query, parameters, parameterNameUsages, aliasMap);
            sb.AppendLine().Append(')');
        }

        sb.AppendLine();
        AppendCommand(sb, query, parameters, parameterNameUsages, aliasMap);

        return _entityContext.Query<TResult>(sb.ToString(), parameters.ToArray<object>());
    }

    private static void AppendCommand(
        StringBuilder commandBuilder,
        IQueryable query,
        List<DbParameter> parameters,
        Dictionary<string, int> parameterNameMap,
        Dictionary<string, string> aliasMap
    ) {
        var parameterMap = new Dictionary<string, string>();
        var command = GetCommand(query);
        foreach(DbParameter parameter in command.Parameters) {
            var parameterName = parameter.ParameterName;
            var baseName = GetRawParameterName(parameterName);
            var offset = parameterNameMap.GetValueOrDefault(baseName);
            var name = $"@__{baseName}_{offset}";

            parameterNameMap[baseName] = offset + 1;
            parameters.Add(parameter);

            if(name == parameterName) continue;
            parameterMap[parameterName] = name;
            parameter.ParameterName = name;
        }

        var commandText = command.CommandText;
        commandText = ParameterRegex.Replace(commandText, m => parameterMap.GetValueOrDefault(m.Value, m.Value));
        commandText = ReferenceRegex.Replace(commandText, m => aliasMap.GetValueOrDefault(m.Value, m.Value));
        commandBuilder.Append(commandText);
    }

    private static DbCommand GetCommand(IQueryable query) {
        var enumerable = query.Provider.Execute<IEnumerable>(query.Expression);
        return ((IRelationalQueryingEnumerable)enumerable).CreateDbCommand();
    }

    private static string GetRawParameterName(string name) {
        var match = ParameterNameRegex.Match(name);
        return match.Groups[1].Value;
    }

    public static ISqlQueryBuilder Create(IEntityContext entityContext) => new SqlQueryBuilder(entityContext);
}