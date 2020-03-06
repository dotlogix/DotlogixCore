using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Nodes.Factories;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes {
    public class NodeConverterResolver : INodeConverterResolver {
        protected static IDictionary<Type, TypeSettings> Primitives { get; } = CreatePrimitiveTypeSettings();


        protected ConcurrentDictionary<Type, INamingStrategy> NamingStrategiesMap { get; } = new ConcurrentDictionary<Type, INamingStrategy>();
        protected ConcurrentDictionary<Type, IAsyncNodeConverter> ConvertersMap { get; } = new ConcurrentDictionary<Type, IAsyncNodeConverter>();
        protected ConcurrentDictionary<Type, INodeConverterFactory> ConverterFactoriesMap { get; } = new ConcurrentDictionary<Type, INodeConverterFactory>();
        protected IList<INodeConverterFactory> ConverterFactories { get; } = new List<INodeConverterFactory>();


        protected ConcurrentDictionary<Type, TypeSettings> TypeSettings { get; } = new ConcurrentDictionary<Type, TypeSettings>();
        protected ConcurrentDictionary<MemberInfo, MemberSettings> MemberSettings { get; } = new ConcurrentDictionary<MemberInfo, MemberSettings>();

        public virtual bool TryGet(Type type, out INamingStrategy value) {
            return NamingStrategiesMap.TryGetValue(type, out value);
        }

        public virtual bool TryGet(Type type, out IAsyncNodeConverter value) {
            return ConvertersMap.TryGetValue(type, out value);
        }

        public virtual bool TryGet(Type type, out INodeConverterFactory value) {
            return ConverterFactoriesMap.TryGetValue(type, out value);
        }

        public virtual bool Register(INamingStrategy namingStrategy) {
            return NamingStrategiesMap.TryAdd(namingStrategy.GetType(), namingStrategy);
        }

        public virtual bool Register(INodeConverterFactory factory) {
            if(ConverterFactoriesMap.TryAdd(factory.GetType(), factory)) {
                ConverterFactories.Add(factory);
                return true;
            }

            return false;
        }

        public virtual bool Register(IAsyncNodeConverter converter) {
            return ConvertersMap.TryAdd(converter.GetType(), converter);
        }

        public virtual void Replace(INamingStrategy namingStrategy) {
            NamingStrategiesMap[namingStrategy.GetType()] = namingStrategy;
        }

        public virtual void Replace(INodeConverterFactory factory) {
            ConverterFactoriesMap.AddOrUpdate(factory.GetType(),
                                              factory,
                                              (t, o) => {
                                                  var index = ConverterFactories.IndexOf(o);
                                                  ConverterFactories[index] = factory;
                                                  return factory;
                                              });
        }

        public virtual void Replace(IAsyncNodeConverter converter) {
            ConvertersMap[converter.GetType()] = converter;
        }

        public bool Unregister(INodeConverterFactory factory) {
            return ConverterFactoriesMap.TryRemove(factory.GetType(), out _) && ConverterFactories.Remove(factory);
        }

        public bool Unregister(IAsyncNodeConverter converter) {
            return ConvertersMap.TryRemove(converter.GetType(), out _);
        }

        public bool Unregister(INamingStrategy namingStrategy) {
            return NamingStrategiesMap.TryRemove(namingStrategy.GetType(), out _);
        }

        public virtual bool TryResolve(Type type, out TypeSettings settings) {
            if(TypeSettings.TryGetValue(type, out settings))
                return true;

            if(TryCreateSettings(type, out settings) == false) {
                if(Primitives.TryGetValue(type, out settings) == false)
                    return false;
            }

            settings = TypeSettings.GetOrAdd(type, settings);
            return true;
        }

        public virtual bool TryResolve(TypeSettings typeSettings, MemberInfo memberInfo, out MemberSettings settings) {
            if(MemberSettings.TryGetValue(memberInfo, out settings))
                return true;

            if(TryCreateSettings((DynamicAccessor)typeSettings.DynamicType.Resolve(memberInfo), out settings) == false)
                return true;

            settings = MemberSettings.GetOrAdd(memberInfo, settings);
            return true;
        }

        public virtual bool TryResolve(TypeSettings typeSettings, DynamicAccessor accessor, out MemberSettings settings) {
            if(MemberSettings.TryGetValue(accessor.MemberInfo, out settings))
                return true;

            if(TryCreateSettings(accessor, out settings) == false)
                return true;

            settings = MemberSettings.GetOrAdd(accessor.MemberInfo, settings);
            return true;
        }

        protected virtual bool TryCreateSettings(DynamicAccessor accessor, out MemberSettings settings) {
            if(TryResolve(accessor.ValueType, out var memberTypeSettings) == false) {
                settings = null;
                return false;
            }

            settings = new MemberSettings {
                                          Accessor = accessor
                                          };
            settings.Apply(memberTypeSettings);

            var propertyAttribute = accessor.MemberInfo.GetCustomAttribute<NodePropertyAttribute>();
            if((propertyAttribute != null) && (ApplyPropertyOverrides(settings, propertyAttribute) == false)) {
                settings = null;
                return false;
            }

            return ApplyChildSettings(accessor.MemberInfo, settings);
        }

        private bool ApplyChildSettings(MemberInfo memberInfo, TypeSettings settings) {
            var childType = settings.DataType.ElementType ?? settings.DataType.UnderlayingType;
            if(childType == null) {
                if(settings.DataType.Type.IsAssignableTo(typeof(IOptional)))
                    childType = settings.DataType.Type.GenericTypeArguments[0];

                if(childType == null)
                    return true;
            }

            var childAttribute = memberInfo.GetCustomAttribute<NodeChildAttribute>();

            if(TryResolve(childType, out var elementTypeSettings) == false) {
                return false;
            }

            var elementSettings = new TypeSettings();
            elementSettings.Apply(elementTypeSettings);
            settings.ChildSettings = elementSettings;

            if((childAttribute != null) && (ApplyElementOverrides(elementSettings, childAttribute) == false)) {
                return false;
            }

            return true;
        }

        private bool ApplyElementOverrides(TypeSettings elementSettings, NodeChildAttribute childAttribute) {
            elementSettings.Settings.Apply(childAttribute.Settings);

            if(childAttribute.ConverterFactory != null) {
                var nodeConverterFactory = ConverterFactoriesMap.GetOrAdd(childAttribute.ConverterFactory, TypeExtension.Instantiate<INodeConverterFactory>);
                if(nodeConverterFactory.TryCreateConverter(this, elementSettings, out var converter))
                    elementSettings.Converter = converter;
                else
                    return false;
            }

            return true;
        }

        private bool ApplyPropertyOverrides(MemberSettings settings, NodePropertyAttribute propertyAttribute) {
            settings.Name = propertyAttribute.Name;
            settings.Order = propertyAttribute.Order;

            settings.Settings.Apply(propertyAttribute.Settings);

            if(propertyAttribute.NamingStrategy != null)
                settings.NamingStrategy = NamingStrategiesMap.GetOrAdd(propertyAttribute.NamingStrategy, TypeExtension.Instantiate<INamingStrategy>);

            if(propertyAttribute.ConverterFactory == null)
                return true;

            var nodeConverterFactory = ConverterFactoriesMap.GetOrAdd(propertyAttribute.ConverterFactory, TypeExtension.Instantiate<INodeConverterFactory>);
            if(nodeConverterFactory.TryCreateConverter(this, settings, out var converter) == false)
                return false;

            settings.Converter = converter;
            return true;
        }

        protected virtual bool TryCreateSettings(Type type, out TypeSettings settings) {
            var dataType = type.ToDataType();
            var nodeType = Nodes.GetNodeType(dataType);

            settings = new TypeSettings {
                                        DataType = dataType,
                                        NodeType = nodeType,
                                        DynamicType = type.CreateDynamicType()
                                        };

            var typeAttribute = type.GetCustomAttribute<NodeTypeAttribute>();
            if(typeAttribute != null) {
                settings.Settings.Apply(typeAttribute.Settings);

                if(typeAttribute.NamingStrategy != null)
                    settings.NamingStrategy = NamingStrategiesMap.GetOrAdd(typeAttribute.NamingStrategy, TypeExtension.Instantiate<INamingStrategy>);

                if(typeAttribute.ConverterFactory != null) {
                    var nodeConverterFactory = ConverterFactoriesMap.GetOrAdd(typeAttribute.ConverterFactory, TypeExtension.Instantiate<INodeConverterFactory>);
                    if(nodeConverterFactory.TryCreateConverter(this, settings, out var converter))
                        settings.Converter = converter;
                    else {
                        settings = null;
                        return false;
                    }
                }
            }

            if(ApplyChildSettings(type, settings) == false) {
                settings = null;
                return false;
            }


            if(settings.Converter == null) {
                for(var i = ConverterFactories.Count - 1; i >= 0; i--) {
                    var converterFactory = ConverterFactories[i];
                    if(converterFactory.TryCreateConverter(this, settings, out var converter) == false)
                        continue;

                    settings.Converter = converter;
                    return true;
                }
            }

            settings = null;
            return false;
        }

        private static IDictionary<Type, TypeSettings> CreatePrimitiveTypeSettings() {
            TypeSettings CreatePrimitiveTypeSettings(DataType dataType) {
                var typeSettings = new TypeSettings { DataType= dataType, NodeType = NodeTypes.Value };
                typeSettings.Converter = new ValueNodeConverter(typeSettings);
                return typeSettings;
            }

            return DataTypeConverter.Instance.Primitives.ToDictionary(kv => kv.Key, kv => CreatePrimitiveTypeSettings(kv.Value));
        }
    }
}