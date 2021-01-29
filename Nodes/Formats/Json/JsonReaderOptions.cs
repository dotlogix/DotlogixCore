#region
using System;
#endregion

namespace DotLogix.Core.Nodes.Formats.Json {
    [Flags]
    public enum JsonReaderOptions {
        None = 0,
        Tolerant = 1, // << 0
    }
}
