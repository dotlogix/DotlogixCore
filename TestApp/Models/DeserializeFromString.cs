using BenchmarkDotNet.Attributes;
using DotLogix.Core.Nodes;
using Newtonsoft.Json;

namespace Benchmarks.Serializers
{
    public class DeserializeFromString<T> where  T : class, new()
    {

        private T _instance;
        private string _json;
        private SerializeToString<T> _serializer;

        public void Setup()
        {
            _serializer = new SerializeToString<T>();
            _serializer.Setup();
            _instance = new T();
        }

        
        [GlobalSetup(Target = nameof(RunNewtonsoft))]
        public void NewtonsoftSetup()
        {
            Setup(); 
            _json = _serializer.RunNewtonsoft();
        }
        [Benchmark]
        public T RunNewtonsoft()
        {
            return JsonConvert.DeserializeObject<T>(_json);
        }
        
        [GlobalSetup(Target = nameof(RunNodes))]
        public void ServiceStackSetup()
        {
            Setup();
            _json = _serializer.RunNodes();
        }
        [Benchmark]
        public T RunNodes()
        {
            return JsonNodes.FromJson<T>(_json);
        }

        
    }
}
