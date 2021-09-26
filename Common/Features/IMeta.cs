using System;

namespace DotLogix.Common.Features {
    public interface IMeta {
        Guid? CreatedByUserGuid { get; set; }
        DateTime? CreatedAtUtc { get; set; }
        Guid? ModifiedByUserGuid { get; set; }
        DateTime? ModifiedAtUtc { get; set; }
        Guid? DeletedByUserGuid { get; set; }
        DateTime? DeletedAtUtc { get; set; }
    }
}
