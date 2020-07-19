using System;
using System.Collections.Generic;

namespace DotLogix.Core.Rest {
    public class AsyncWebRequestMetrics {
        public DateTime FromUtc { get; set; }
        public DateTime UntilUtc { get; set; }

        public TimeSpan PreProcessingDuration { get; set; }
        public TimeSpan ExecutionDuration { get; set; }
        public TimeSpan PostProcessingDuration { get; set; }
        public TimeSpan WriteResponseDuration { get; set; }
        public TimeSpan TotalDuration => PreProcessingDuration + ExecutionDuration + PostProcessingDuration + WriteResponseDuration;

        public long BodyLength { get; set; }
        public long ContentLength { get; set; }
        public Dictionary<string, object> MetaInfo { get; set; }
    }
}