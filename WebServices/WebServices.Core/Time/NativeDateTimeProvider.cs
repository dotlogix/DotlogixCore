#region using
using System;
#endregion

namespace DotLogix.WebServices.Core.Time
{
    public class NativeDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
