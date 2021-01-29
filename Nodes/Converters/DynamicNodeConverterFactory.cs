using System;
using DotLogix.Core.Nodes.Schema;

namespace DotLogix.Core.Nodes.Converters {
    public class DynamicNodeConverterFactory : NodeConverterFactoryBase {
        private readonly Func<INodeConverterResolver, TypeSettings, INodeConverter> _factoryFunc;
        private readonly Func<INodeConverterResolver, TypeSettings, bool> _typeFilterFunc;

        public DynamicNodeConverterFactory(Func<INodeConverterResolver, TypeSettings, INodeConverter> factoryFunc, Func<INodeConverterResolver, TypeSettings, bool> typeFilterFunc) {
            _factoryFunc = factoryFunc;
            _typeFilterFunc = typeFilterFunc;
        }


        public override bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out INodeConverter converter) {
            if(_typeFilterFunc != null && _typeFilterFunc.Invoke(resolver, typeSettings) == false) {
                converter = default;
                return false;
            }
            
            converter = _factoryFunc.Invoke(resolver, typeSettings);
            return true;
        }
    }
}