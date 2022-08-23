using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace DotLogix.WebServices.EntityFramework.Database; 

public interface IMethodCallTranslator {
    SqlExpression Translate(SqlExpression instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments);
}