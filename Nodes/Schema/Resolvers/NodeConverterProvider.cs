using System;
using System.Collections.Concurrent;
using DotLogix.Core.Nodes.Converters;

namespace DotLogix.Core.Nodes.Schema {
    public class NodeConverterProvider {
        protected ConcurrentDictionary<Type, INodeConverter> ConverterMap { get; } = new ConcurrentDictionary<Type, INodeConverter>();
        
        public virtual bool TryGet(Type objectType, out INodeConverter value) {
            return ConverterMap.TryGetValue(objectType, out value);
        }

        public virtual bool Add(Type objectType, INodeConverter converter) {
            return ConverterMap.TryAdd(converter.GetType(), converter);
        }
        
        public virtual void Replace(Type objectType, INodeConverter converter) {
            ConverterMap.AddOrUpdate(objectType, converter, (t, nodeConverter) => converter);
        }

        public virtual bool Remove(Type objectType) {
            return ConverterMap.TryRemove(objectType, out _);
        }
    }
}