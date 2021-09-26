using DotLogix.Core;
using DotLogix.WebServices.Core.Serialization;
using Newtonsoft.Json;

namespace DotLogix.WebServices.Core.Terms {
    [JsonConverter(typeof(RangeFilterJsonConverter<>))]
    public class RangeTerm<T>
    {
        public Optional<T> Min { get; set; }
        public Optional<T> Max { get; set;}
        

        public static implicit operator RangeTerm<T>(T value) {
            return new RangeTerm<T> {
                Min = value,
                Max = value
            };
        }
    }
}