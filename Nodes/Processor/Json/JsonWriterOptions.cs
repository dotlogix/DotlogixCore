using System;

namespace DotLogix.Core.Nodes.Processor {
    [Flags]
    public enum JsonWriterOptions {
        None = 0,
        Tolerant = 1, // << 0
        Sync = 1 << 1
    }
}