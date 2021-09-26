using System;
using DotLogix.Common.Features;

namespace DotLogix.WebServices.EntityFramework.Entities {
    /// <summary>
    /// A base class for entities with additional meta information
    /// </summary>
    public abstract class MetaEntityBase : EntityBase, IMeta {
        /// <inheritdoc />
        public Guid? CreatedByUserGuid { get; set; }
        /// <inheritdoc />
        public DateTime? CreatedAtUtc { get; set; }
        /// <inheritdoc />
        public Guid? ModifiedByUserGuid { get; set; }
        /// <inheritdoc />
        public DateTime? ModifiedAtUtc { get; set; }
        /// <inheritdoc />
        public Guid? DeletedByUserGuid { get; set; }
        /// <inheritdoc />
        public DateTime? DeletedAtUtc { get; set; }
    }
}
