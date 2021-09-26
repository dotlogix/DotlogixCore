using System;
using DotLogix.Common.Features;

namespace DotLogix.WebServices.Adapters.Models {
    /// <summary>
    /// A base class for models with additional meta information
    /// </summary>
    public class MetaModelBase : ModelBase, IMeta {
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
