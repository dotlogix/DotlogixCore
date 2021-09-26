#region
using System;
using System.Reflection;
using DotLogix.Core;
using DotLogix.Core.Extensions;
using DotLogix.Core.Utils.Naming;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
#endregion

namespace DotLogix.WebServices.Core.Serialization {
    public sealed class GenericContractResolver : CamelCasePropertyNamesContractResolver {
        private JsonNamingStrategy _namingStrategy;

        public new INamingStrategy NamingStrategy {
            get => _namingStrategy?.NamingStrategy;
            set {
                if(value == null) {
                    _namingStrategy = null;
                    base.NamingStrategy = new DefaultNamingStrategy();
                } else {
                    _namingStrategy = new JsonNamingStrategy(value);
                    base.NamingStrategy = _namingStrategy;
                }
            }
        }

        public GenericContractResolver(INamingStrategy namingStrategy = null) {
            NamingStrategy = namingStrategy ?? NamingStrategies.CamelCase;
        }

        protected override JsonConverter ResolveContractConverter(Type objectType) {
            var typeInfo = objectType.GetTypeInfo();
            if(typeInfo.IsGenericType && !typeInfo.IsGenericTypeDefinition) {
                var jsonConverterAttribute = typeInfo.GetCustomAttribute<JsonConverterAttribute>();
                if((jsonConverterAttribute != null) && jsonConverterAttribute.ConverterType.GetTypeInfo().IsGenericTypeDefinition) {
                    return (JsonConverter)Activator.CreateInstance(jsonConverterAttribute.ConverterType.MakeGenericType(typeInfo.GenericTypeArguments), jsonConverterAttribute.ConverterParameters);
                }
            }

            if(objectType.IsAssignableToGeneric(typeof(Optional<>), out var genericArgs)) {
                return typeof(OptionalJsonConverter<>).MakeGenericType(genericArgs).Instantiate<JsonConverter>();
            }

            return base.ResolveContractConverter(objectType);
        }

        private class JsonNamingStrategy : NamingStrategy {
            public INamingStrategy NamingStrategy { get; }

            public JsonNamingStrategy(INamingStrategy namingStrategy) {
                NamingStrategy = namingStrategy;
                ProcessDictionaryKeys = true;
                OverrideSpecifiedNames = true;
            }

            protected override string ResolvePropertyName(string name) {
                return NamingStrategy?.Rewrite(name) ?? name;
            }
        }
    }
}
