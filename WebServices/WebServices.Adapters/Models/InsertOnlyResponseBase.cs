using DotLogix.Common.Features;

namespace DotLogix.WebServices.Adapters.Models; 

public class InsertOnlyResponseBase : ResponseBase, IInsertOnly {
    public bool IsActive { get; set; }
}