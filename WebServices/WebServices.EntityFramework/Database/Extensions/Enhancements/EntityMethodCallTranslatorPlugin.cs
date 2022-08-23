using System.Collections.Generic;
using System.Reflection;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace DotLogix.WebServices.EntityFramework.Database; 

public class EntityMethodCallTranslatorPlugin : IMethodCallTranslatorPlugin, Microsoft.EntityFrameworkCore.Query.IMethodCallTranslator {
    private readonly IReadOnlyCollection<IMethodCallTranslator> _translators;
    public IEnumerable<Microsoft.EntityFrameworkCore.Query.IMethodCallTranslator> Translators { get; }

    public EntityMethodCallTranslatorPlugin(IEnumerable<IMethodCallTranslator> translators) {
        _translators = translators.AsReadOnlyCollection();
        Translators = this.CreateArray();
    }

    SqlExpression Microsoft.EntityFrameworkCore.Query.IMethodCallTranslator.Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger) {
        if(_translators.Count == 0) {
            return default;
        }
        
        foreach(var translator in _translators) {
            if(translator.Translate(instance, method, arguments) is { } transpiled)
                return transpiled;
        }
        return default;
    }
}