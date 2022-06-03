using System;

namespace DotLogix.Common.Features {
    public interface IMeta {
        /// <summary>
        /// The user who created this entity
        /// </summary>
        Guid? CreatedByUserGuid { get; set; }
        /// <summary>
        /// The utc timestamp at which this entity was created
        /// </summary>
        DateTime? CreatedAtUtc { get; set; }
        
        /// <summary>
        /// The user who last modified this entity
        /// </summary>
        Guid? ModifiedByUserGuid { get; set; }
        /// <summary>
        /// The utc timestamp at which this entity was last modified
        /// </summary>
        DateTime? ModifiedAtUtc { get; set; }
        
        /// <summary>
        /// The user who deleted this entity
        /// </summary>
        Guid? DeletedByUserGuid { get; set; }
        /// <summary>
        /// The utc timestamp at which this entity was deleted
        /// </summary>
        DateTime? DeletedAtUtc { get; set; }
    }
}
