using System;
using DotLogix.Common.Features;

namespace DotLogix.WebServices.Adapters.Models; 

public class MetaResponseBase : ResponseBase, IMeta {
    public Guid? CreatedByUserGuid { get; set; }
    public DateTime? CreatedAtUtc { get; set; }
    public Guid? ModifiedByUserGuid { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }
    public Guid? DeletedByUserGuid { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
}