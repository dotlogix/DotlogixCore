namespace DotLogix.WebServices.Core.Terms; 

public class PaginationTerm {
    public int? Page { get; set; }
    public int PageSize { get; set; } = 100;
    public ManyTerm<OrderingKey> OrderBy { get; set; }
}