#region
using System;
#endregion

namespace DotLogix.Core.Nodes.Formats.Json {
    [Flags]
    public enum JsonWriterOptions {
        None = 0,
        Tolerant = 1, // << 0
    }
}
