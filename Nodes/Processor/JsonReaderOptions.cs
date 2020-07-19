using System;

namespace DotLogix.Core.Nodes.Processor {
    [Flags]
    public enum JsonReaderOptions {
        None = 0,
        Tolerant = 1// << 0
    }
}