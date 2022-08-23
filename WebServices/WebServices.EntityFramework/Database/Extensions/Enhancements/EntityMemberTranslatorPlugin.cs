using System;
using System.Collections.Generic;
using System.Reflection;
using DotLogix.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace DotLogix.WebServices.EntityFramework.Database; 

public class EntityMemberTranslatorPlugin : IMemberTranslatorPlugin, Microsoft.EntityFrameworkCore.Query.IMemberTranslator {
    private readonly IReadOnlyCollection<IMemberTranslator> _translators;
    public IEnumerable<Microsoft.EntityFrameworkCore.Query.IMemberTranslator> Translators { get; }

    public EntityMemberTranslatorPlugin(IEnumerable<IMemberTranslator> translator) {
        _translators = translator.AsReadOnlyCollection();
        Translators = this.CreateArray();
    }

    SqlExpression Microsoft.EntityFrameworkCore.Query.IMemberTranslator.Translate(SqlExpression instance, MemberInfo member, Type returnType, IDiagnosticsLogger<DbLoggerCategory.Query> logger) {
        if(_translators.Count == 0) {
            return default;
        }
        
        foreach(var translator in _translators) {
            if(translator.Translate(instance, member) is { } transpiled)
                return transpiled;
        }
        return default;
    }
}