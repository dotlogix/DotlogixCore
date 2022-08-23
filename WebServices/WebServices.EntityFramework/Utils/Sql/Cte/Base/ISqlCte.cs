using System;
using System.Linq;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public interface ISqlCte {
    public Type Type { get; }
    public string Name { get; }
    public string Alias { get; }
    public bool Recursive { get; }
    public bool? Materialize { get; }
    public IQueryable Query { get; }
    public IQueryable Reference { get; }
}