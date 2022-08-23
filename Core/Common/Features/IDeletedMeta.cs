using System;

namespace DotLogix.Common.Features; 

public interface IDeletedMeta {
    /// <summary>
    ///     The user who deleted this entity
    /// </summary>
    Guid? DeletedByUserGuid { get; set; }

    /// <summary>
    ///     The utc timestamp at which this entity was deleted
    /// </summary>
    DateTime? DeletedAtUtc { get; set; }
}