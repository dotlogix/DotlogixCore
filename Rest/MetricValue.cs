using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Rest {
    public class MetricValue {
        public List<double> Values { get; set; }
        public double Max => Values.Max();
        public double Min => Values.Min();
        public double Avg => Values.Average();
        public double Current => Values.LastOrDefault();
    }
}