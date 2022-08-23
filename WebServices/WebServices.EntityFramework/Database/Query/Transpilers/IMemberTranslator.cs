using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
namespace DotLogix.WebServices.EntityFramework.Database; 

public interface IMemberTranslator {
    SqlExpression Translate(SqlExpression instance, MemberInfo member);
}