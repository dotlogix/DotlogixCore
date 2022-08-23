using System;

namespace DotLogix.Common.Features {
    public interface ICreatedMeta {
        /// <summary>
        ///     The user who created this entity
        /// </summary>
        Guid? CreatedByUserGuid { get; set; }

        /// <summary>
        ///     The utc timestamp at which this entity was created
        /// </summary>
        DateTime? CreatedAtUtc { get; set; }
    }
}
