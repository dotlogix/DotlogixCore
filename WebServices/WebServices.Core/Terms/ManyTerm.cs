#region
using System.Collections;
using System.Collections.Generic;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Serialization;
using Newtonsoft.Json;
#endregion

namespace DotLogix.WebServices.Core.Terms {
    [JsonConverter(typeof(ManyTermJsonConverter<>))]
    public class ManyTerm<T> : IReadOnlyList<T> {
        public int Count => Values.Count;
        public T this[int index] => Values[index];
        public IReadOnlyList<T> Values { get; set; }
        
        public ManyTerm() {
        }
        public ManyTerm(IEnumerable<T> values) {
            Values = values.AsReadOnlyList();
        }
        public ManyTerm(T value) {
            Values = value.CreateArray();
        }

        
        public IEnumerator<T> GetEnumerator() => Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Values).GetEnumerator();

        public static implicit operator ManyTerm<T>(T value) => new ManyTerm<T>(value);
        public static implicit operator ManyTerm<T>(T[] values) => new ManyTerm<T>(values);
        public static implicit operator ManyTerm<T>(List<T> values) => new ManyTerm<T>(values);
    }
}
