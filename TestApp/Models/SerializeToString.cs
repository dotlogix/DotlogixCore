using BenchmarkDotNet.Attributes;
using DotLogix.Core.Nodes;
using Newtonsoft.Json;

namespace Benchmarks.Serializers {
    public class SerializeToString<T> where T : class, new() {
        private T _instance;

        [GlobalSetup]
        public void Setup() {
            _instance = _instance ?? new T();
        }

        [Benchmark]
        public string RunNewtonsoft() {
            return JsonConvert.SerializeObject(_instance);
        }

        [Benchmark]
        public string RunNodes() {
            return JsonNodes.ToJson(_instance);
        }
    }
}
