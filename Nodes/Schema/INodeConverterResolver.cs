using System;
using System.Reflection;
using DotLogix.Core.Nodes.Factories;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Utils.Naming;

namespace DotLogix.Core.Nodes.Schema {
    public interface INodeConverterResolver {
        bool TryGet(Type type, out INamingStrategy value);
        bool TryGet(Type type, out INodeConverterFactory value);

        bool Add(INamingStrategy namingStrategy);
        bool Add(INodeConverterFactory factory);

        void Replace(INamingStrategy namingStrategy);
        void Replace(INodeConverterFactory factory);

        bool Remove(INamingStrategy namingStrategy);
        bool Remove(INodeConverterFactory factory);

        bool TryResolve(Type type, out TypeSettings settings);
        bool TryResolve(TypeSettings typeSettings, MemberInfo memberInfo, out MemberSettings settings);
        bool TryResolve(TypeSettings typeSettings, DynamicAccessor dynamicAccessor, out MemberSettings settings);
    }
}