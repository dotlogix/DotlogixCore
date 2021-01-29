using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Utils.Naming;

namespace DotLogix.Core.Nodes.Schema {
    public class NamingStrategyProvider {
        private readonly object _lock = new object();
        private readonly List<INamingStrategy> _converterFactories = new List<INamingStrategy>();

        public IReadOnlyList<INamingStrategy> ConverterFactories => _converterFactories;
        
        public virtual INamingStrategy Get(Type factoryType) {
            lock(_lock) {
                return _converterFactories.FirstOrDefault(factoryType.IsInstanceOfType);
            }
        }
        
        public virtual bool Add(INamingStrategy factory) {
            lock(_lock) {
                if(_converterFactories.Contains(factory))
                    return false;
            
                _converterFactories.Add(factory);
            }
            return false;
        }

        public virtual bool Remove(INamingStrategy factory) {
            lock(_lock) {
                return _converterFactories.Remove(factory);
            }
        }
    }
}