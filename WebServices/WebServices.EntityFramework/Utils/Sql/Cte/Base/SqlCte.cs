using System;
using System.Linq;

namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public class SqlCte : ISqlCte {
    public Type Type { get; init; }
    public string Name { get; init; }
    public string Alias { get; init; }
    public bool Recursive { get; init; }
    public bool? Materialize { get; init; }
    
    public IQueryable Query { get; init; }
    public IQueryable Reference { get; init; }
}