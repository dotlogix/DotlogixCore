using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using DotLogix.Core.Collections;
using DotLogix.Core.Extensions;
using DotLogix.Core.Interfaces;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Nodes.Factories;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;

namespace DotLogix.Core.Nodes {
    public class ConverterSettings : IConverterSettings {
        public static ConverterSettings Default => new ConverterSettings();

        /// <summary>
        ///     The naming strategy (camelCase by default)
        /// </summary>
        public INamingStrategy NamingStrategy { get; set; } = new CamelCaseNamingStrategy();

        /// <summary>
        ///     The time format (u by default)
        /// </summary>
        public string TimeFormat { get; set; } = "u";

        /// <summary>
        ///     The number format (G by default)
        /// </summary>
        public string NumberFormat { get; set; } = "G";

        /// <summary>
        ///     The guid format (D by default)
        /// </summary>
        public string GuidFormat { get; set; } = "D";

        /// <summary>
        ///     The enum format (D by default)
        /// </summary>
        public string EnumFormat { get; set; } = "D";

        /// <summary>
        ///     Determines if default or null values should be ignored
        /// </summary>
        public EmitMode EmitMode { get; set; } = EmitMode.Emit;

        /// <summary>
        ///     The format provider (invariant by default)
        /// </summary>
        public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;

        /// <summary>
        ///     The format provider (invariant by default)
        /// </summary>
        public INodeConverterResolver Resolver { get; set; } = Nodes.DefaultResolver;

        public virtual void Apply(IConverterSettings settings) {
            Resolver = settings.Resolver;
            NamingStrategy = settings.NamingStrategy;
            EmitMode = settings.EmitMode;
            EnumFormat = settings.EnumFormat;
            FormatProvider = settings.FormatProvider;
            GuidFormat = settings.GuidFormat;
            EnumFormat = settings.EnumFormat;
            NumberFormat = settings.NumberFormat;
            TimeFormat = settings.TimeFormat;
        }

        /// <summary>
        ///     Clone object
        /// </summary>
        /// <returns></returns>
        public IConverterSettings Clone() {
            return (IConverterSettings)MemberwiseClone();
        }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object ICloneable.Clone() {
            return Clone();
        }
    }


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
        bool Unregister(INodeConverterFactory factory);
        bool Unregister(IAsyncNodeConverter converter);
        bool TryResolve(Type type, out TypeSettings settings);
        bool TryResolve(TypeSettings typeSettings, MemberInfo memberInfo, out MemberSettings settings);
        bool TryResolve(TypeSettings typeSettings, DynamicAccessor dynamicAccessor, out MemberSettings settings);
    }


    public enum EmitMode {
        Inherit = 0,
        Emit = 1,
        IgnoreNull = 2,
        IgnoreDefault = 3
    }

    public class TypeSettings {
        public DynamicType DynamicType { get; set; }
        public DataType DataType { get; set; }
        public NodeTypes NodeType { get; set; }

        public IAsyncNodeConverter Converter { get; set; }
        public INamingStrategy NamingStrategy { get; set; }
        public EmitMode EmitMode { get; set; }

        public TypeSettings ChildSettings { get; set; }


        public virtual void Apply(TypeSettings settings) {
            DataType = settings.DataType;
            NodeType = settings.NodeType;
            DynamicType = settings.DynamicType;

            NamingStrategy = settings.NamingStrategy;
            Converter = settings.Converter;
            ChildSettings = settings.ChildSettings;

            EmitMode = settings.EmitMode;
        }

        public bool ShouldEmitValue(object value, ConverterSettings settings) {
            var emitMode = EmitMode == EmitMode.Inherit
                           ? settings.EmitMode
                           : EmitMode;

            if(value == null)
                return emitMode == EmitMode.Emit;

            var type = value.GetType();
            return (type.IsValueType == false) || (emitMode != EmitMode.IgnoreDefault) || (Equals(type.GetDefaultValue(), value) == false);
        }
    }

    public class NodeConverterResolver : INodeConverterResolver {
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

        public virtual bool TryResolve(Type type, out TypeSettings settings) {
            if(TypeSettings.TryGetValue(type, out settings))
                return true;

            if(TryCreateSettings(type, out settings) == false)
                return true;

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

            settings = new MemberSettings {Accessor = accessor};
            settings.Apply(memberTypeSettings);

            var propertyAttribute = accessor.MemberInfo.GetCustomAttribute<NodePropertyAttribute>();
            if((propertyAttribute != null) && (ApplyPropertyOverrides(settings, propertyAttribute) == false)) {
                settings = null;
                return false;
            }

            var childType = settings.DataType.ElementType ?? settings.DataType.UnderlayingType;
            if(childType == null)
                return true;

            var childAttribute = accessor.MemberInfo.GetCustomAttribute<NodeChildAttribute>();

            if(TryResolve(childType, out var elementTypeSettings) == false) {
                settings = null;
                return false;
            }

            var elementSettings = new TypeSettings();
            elementSettings.Apply(elementTypeSettings);

            if((childAttribute != null) && (ApplyElementOverrides(elementSettings, childAttribute) == false)) {
                settings = null;
                return false;
            }

            settings.ChildSettings = elementTypeSettings;
            return true;
        }

        private bool ApplyElementOverrides(TypeSettings elementSettings, NodeChildAttribute childAttribute) {
            if(childAttribute.EmitMode != EmitMode.Inherit)
                elementSettings.EmitMode = childAttribute.EmitMode;

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

            if(propertyAttribute.EmitMode != EmitMode.Inherit)
                settings.EmitMode = propertyAttribute.EmitMode;

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
                settings.EmitMode = typeAttribute.EmitMode;

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
    }


    public class MemberSettings : TypeSettings {
        public DynamicAccessor Accessor { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreMemberAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class NodeTypeAttribute : Attribute {
        public Type ConverterFactory { get; set; }
        public Type NamingStrategy { get; set; }
        public EmitMode EmitMode { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
    public class NodeChildAttribute : Attribute {
        public Type ConverterFactory { get; set; }
        public EmitMode EmitMode { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NodePropertyAttribute : NodeTypeAttribute {
        public string Name { get; set; }
        public int? Order { get; set; }
    }

    public interface ISettings {
        object this[string name] { get; set; }
        void Set(string name, object value = default);
        object Get(string name, object defaultValue = default);
        T Get<T>(string name, T defaultValue = default);
        bool TryGet<T>(string name, out T value);
    }

    public abstract class Settings : ISettings {
        protected IDictionary<string, object> Values { get; }

        public object this[string name] {
            get => Get(name);
            set => Set(name);
        }

        public Settings() : this(new Dictionary<string, object>(StringComparer.Ordinal)) { }

        public Settings(IDictionary<string, object> values) {
            Values = values;
        }

        public void Set(string name, object value = default) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));
            Values[name] = value;
        }

        public object Get(string name, object defaultValue = default) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));
            return Values.GetValue(name, defaultValue);
        }

        public T Get<T>(string name, T defaultValue = default) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));
            return Values.GetValueAs(name, defaultValue);
        }

        public bool TryGet<T>(string name, out T value) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));
            return Values.TryGetValueAs(name, out value);
        }

        protected void SetWithMemberName(object value, [CallerMemberName] string memberName = null) {
            if(memberName == null)
                throw new ArgumentNullException(nameof(memberName));
            Values[memberName] = value;
        }

        protected T GetWithMemberName<T>([CallerMemberName] string memberName = null, T defaultValue = default) {
            if(memberName == null)
                throw new ArgumentNullException(nameof(memberName));
            return Values.GetValueAs(memberName, defaultValue);
        }
    }


    public interface ILayeredSettings : ISettings {
        /// <summary>
        ///     Add a new layer to the collection
        /// </summary>
        void PushLayer();

        /// <summary>
        ///     Removes the topmost layer from the stack
        /// </summary>
        /// <returns></returns>
        IDictionary<string, object> PopLayer();

        /// <summary>
        ///     Get the topmost layer but don't remove it
        /// </summary>
        /// <returns></returns>
        IDictionary<string, object> PeekLayer();
    }

    public class LayeredSettings : ILayeredSettings {
        protected LayeredDictionary<string, object> Values { get; }

        public object this[string name] {
            get => Values.GetValue(name);
            set => Values[name] = value;
        }

        public LayeredSettings() {
            Values = new LayeredDictionary<string, object>(StringComparer.Ordinal);
            Values.
        }

        public void Set(string name, object value) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));
            Values[name] = value;
        }

        public object Get(string name, object defaultValue = default) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));
            return Values.GetValue(name, defaultValue);
        }

        public T Get<T>(string name, T defaultValue = default) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));
            return Values.GetValueAs(name, defaultValue);
        }

        public bool TryGet<T>(string name, out T value) {
            if(name == null)
                throw new ArgumentNullException(nameof(name));
            return Values.TryGetValueAs(name, out value);
        }

        /// <summary>
        ///     Add a new layer to the collection
        /// </summary>
        public void PushLayer() {
            Values.PushLayer(new Dictionary<string, object>(StringComparer.Ordinal));
        }

        /// <summary>
        ///     Removes the topmost layer from the stack
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> PopLayer() {
            return Values.PopLayer();
        }

        /// <summary>
        ///     Get the topmost layer but don't remove it
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> PeekLayer() {
            return Values.PeekLayer();
        }

        protected void SetWithMemberName(object value, [CallerMemberName] string memberName = null) {
            Set(memberName, value);
        }

        protected T GetWithMemberName<T>([CallerMemberName] string memberName = null, T defaultValue = default) {
            return Get(memberName, defaultValue);
        }
    }

    public interface IConverterSettings : ICloneable<IConverterSettings> {
        /// <summary>
        ///     The naming strategy (camelCase by default)
        /// </summary>
        INamingStrategy NamingStrategy { get; set; }

        /// <summary>
        ///     The time format (u by default)
        /// </summary>
        string TimeFormat { get; set; }

        /// <summary>
        ///     The number format (G by default)
        /// </summary>
        string NumberFormat { get; set; }

        /// <summary>
        ///     The guid format (D by default)
        /// </summary>
        string GuidFormat { get; set; }

        /// <summary>
        ///     The enum format (D by default)
        /// </summary>
        string EnumFormat { get; set; }

        /// <summary>
        ///     Determines if default or null values should be ignored
        /// </summary>
        EmitMode EmitMode { get; set; }

        /// <summary>
        ///     The format provider (invariant by default)
        /// </summary>
        IFormatProvider FormatProvider { get; set; }

        /// <summary>
        ///     The resolver instance to resolve type converters
        /// </summary>
        INodeConverterResolver Resolver { get; set; }

        void Apply(IConverterSettings settings);
    }

    ///// <summary>
    /////     Basic node converter settings
    ///// </summary>
    //public class ConverterSettings : IConverterSettings {
    //    /// <summary>
    //    ///     The default settings
    //    /// </summary>
    //    public static ConverterSettings Default => new ConverterSettings();

    //    /// <summary>
    //    ///     The naming strategy (camelCase by default)
    //    /// </summary>
    //    public INamingStrategy NamingStrategy { get; set; } = new CamelCaseNamingStrategy();

    //    /// <summary>
    //    ///     The time format (u by default)
    //    /// </summary>
    //    public string TimeFormat { get; set; } = "u";

    //    /// <summary>
    //    ///     The number format (G by default)
    //    /// </summary>
    //    public string NumberFormat { get; set; } = "G";

    //    /// <summary>
    //    ///     The guid format (D by default)
    //    /// </summary>
    //    public string GuidFormat { get; set; } = "D";

    //    /// <summary>
    //    ///     The enum format (D by default)
    //    /// </summary>
    //    public string EnumFormat { get; set; } = "D";

    //    /// <summary>
    //    ///     Determines if default values should be ignored
    //    /// </summary>
    //    public bool? EmitDefault { get; set; } = true;

    //    /// <summary>
    //    ///     Determines if null values should be ignored
    //    /// </summary>
    //    public bool? EmitNull { get; set; } = true;

    //    /// <summary>
    //    ///     Determines if only the defined type should be instead of the actual instance type
    //    /// </summary>
    //    public bool? EmitDefinedTypeOnly { get; set; }

    //    /// <summary>
    //    ///     The format provider (invariant by default)
    //    /// </summary>
    //    public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;

    //    /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
    //    public ConverterSettings() { }

    //    /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
    //    public ConverterSettings(ConverterSettings template) {
    //        TimeFormat = template.TimeFormat;
    //        NumberFormat = template.NumberFormat;
    //        GuidFormat = template.GuidFormat;
    //        EnumFormat = template.EnumFormat;
    //        EmitDefault = template.EmitDefault;
    //        EmitNull = template.EmitNull;
    //        FormatProvider = template.FormatProvider;
    //        EmitDefinedTypeOnly = template.EmitDefinedTypeOnly;
    //    }

    //    public bool ShouldEmitValue(object value) {
    //        if(EmitDefault == false) {
    //            return Equals(value?.GetType()
    //                               .GetDefaultValue(),
    //                          value) ==
    //                   false;
    //        }

    //        if(EmitNull == false)
    //            return Equals(value, null) == false;
    //        return true;
    //    }

    //    public IConverterSettings CreateInherited() {
    //        return new CascadingConverterSetting { Inherits = this };
    //    }
    //}
}
