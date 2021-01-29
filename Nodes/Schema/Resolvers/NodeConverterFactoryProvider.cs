using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Nodes.Converters;

namespace DotLogix.Core.Nodes.Schema {
    public class NodeConverterFactoryProvider {
        private readonly object _lock = new object();
        private readonly List<INodeConverterFactory> _converterFactories = new List<INodeConverterFactory>();

        public IReadOnlyList<INodeConverterFactory> ConverterFactories => _converterFactories;
        
        public virtual INodeConverterFactory Get(Type factoryType) {
            lock(_lock) {
                return _converterFactories.FirstOrDefault(factoryType.IsInstanceOfType);
            }
        }
        
        public virtual bool Add(INodeConverterFactory factory) {
            lock(_lock) {
                if(_converterFactories.Contains(factory))
                    return false;
            
                _converterFactories.Add(factory);
            }
            return false;
        }

        public virtual bool Remove(INodeConverterFactory factory) {
            lock(_lock) {
                return _converterFactories.Remove(factory);
            }
        }
    }
}