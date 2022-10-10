using System;
using System.Reflection;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Nodes.Factories;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Nodes {
    public interface INodeConverterResolver {
        bool TryGet(Type type, out INamingStrategy value);
        bool TryGet(Type type, out IAsyncNodeConverter value);
        bool TryGet(Type type, out INodeConverterFactory value);

        bool Register(INamingStrategy namingStrategy);
        bool Register(INodeConverterFactory factory);
        bool Register(IAsyncNodeConverter converter);

        void Replace(INamingStrategy namingStrategy);
        void Replace(INodeConverterFactory factory);
        void Replace(IAsyncNodeConverter converter);

        bool Unregister(INamingStrategy namingStrategy);
        bool Unregister(INodeConverterFactory factory);
        bool Unregister(IAsyncNodeConverter converter);

        bool TryResolve(Type type, out TypeSettings settings);
        bool TryResolve(TypeSettings typeSettings, MemberInfo memberInfo, out MemberSettings settings);
        bool TryResolve(TypeSettings typeSettings, DynamicAccessor dynamicAccessor, out MemberSettings settings);
    }
}