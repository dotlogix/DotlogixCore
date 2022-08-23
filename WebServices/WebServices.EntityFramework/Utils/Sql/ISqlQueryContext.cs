using System.Collections.Generic;
using DotLogix.WebServices.EntityFramework.Context;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public interface ISqlQueryContext {
    IEntityContext EntityContext { get; }
    IReadOnlyDictionary<string, ISqlCte> References { get; }
}