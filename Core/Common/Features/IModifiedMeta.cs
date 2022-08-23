using System;

namespace DotLogix.Common.Features; 

public interface IModifiedMeta {
    /// <summary>
    ///     The user who last modified this entity
    /// </summary>
    Guid? ModifiedByUserGuid { get; set; }

    /// <summary>
    ///     The utc timestamp at which this entity was last modified
    /// </summary>
    DateTime? ModifiedAtUtc { get; set; }
}