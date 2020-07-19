using System;

namespace DotLogix.Core.Rest {
    public class AsyncWebServerMetrics {
        public int CurrentRequestCount { get; set; }



        public DateTime? LastRequestAt { get; }
        public DateTime? LastErrorAt { get; }
    }
}