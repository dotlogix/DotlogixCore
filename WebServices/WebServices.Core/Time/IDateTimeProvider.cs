#region using
using System;
#endregion

namespace DotLogix.WebServices.Core.Time
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
