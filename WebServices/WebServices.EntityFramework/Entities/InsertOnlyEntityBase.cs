using DotLogix.Common.Features;

namespace DotLogix.WebServices.EntityFramework.Entities; 

public abstract class InsertOnlyEntityBase : EntityBase, IInsertOnly
{
    public bool IsActive { get; set; }
}