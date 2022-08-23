namespace DotLogix.WebServices.EntityFramework.Utils.Sql; 

public interface ISqlCteBuilder<out TBuilder>
    where TBuilder : ISqlCteBuilder<TBuilder>
{
    TBuilder UseName(string name);
    TBuilder UseMaterialize(bool? materialize = true);
    
    ISqlCte Build();
}