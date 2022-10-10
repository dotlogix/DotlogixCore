// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ListNodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    /// <summary>
    /// An implementation of the <see cref="INodeConverterFactory"/> for collection types
    /// </summary>
    public class ListNodeConverterFactory : NodeConverterFactoryBase {
        private static readonly Dictionary<Type, Type> _interfaceRemapping = new Dictionary<Type, Type> {
                                                                                                  {typeof(IEnumerable), typeof(List<>) },
                                                                                                  {typeof(IEnumerable<>), typeof(List<>) },
                                                                                                  {typeof(ICollection), typeof(List<>) },
                                                                                                  {typeof(ICollection<>), typeof(List<>) },
                                                                                                  {typeof(IList), typeof(List<>) },
                                                                                                  {typeof(IList<>), typeof(List<>) },
                                                                                                  {typeof(IReadOnlyList<>), typeof(List<>) },
                                                                                                  {typeof(IDictionary<,>), typeof(Dictionary<,>) },
                                                                                                  {typeof(IReadOnlyDictionary<,>), typeof(Dictionary<,>) },
                                                                                                  };

        private static readonly HashSet<Type> StandardOpenGenerics = new HashSet<Type> {
                                                                                           typeof(Collection<>),
                                                                                           typeof(List<>),
                                                                                           typeof(Dictionary<,>),
                                                                                           typeof(ReadOnlyCollection<>)
                                                                                       };

        /// <inheritdoc />
        public override bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out IAsyncNodeConverter converter) {
            converter = null;
            if(typeSettings.NodeType != NodeTypes.List)
                return false;

            if((typeSettings.DataType.Flags & DataTypeFlags.CategoryMask) != DataTypeFlags.Collection)
                return false;

            var type = typeSettings.DataType.Type;
            if(type.IsArray)
                converter = CreateArrayConverter(typeSettings);
            else if(type.IsGenericType) {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (type.IsInterface) {
                    if(_interfaceRemapping.TryGetValue(genericTypeDefinition, out var mappedType) == false)
                        return false;
                    var genericArguments = type.GetGenericArguments();
                    type = mappedType.MakeGenericType(genericArguments);

                    typeSettings.DynamicType = type.CreateDynamicType();
                    typeSettings.DataType = type.ToDataType();
                }

                if(StandardOpenGenerics.Contains(genericTypeDefinition) || type.IsAssignableToOpenGeneric(typeof(ICollection<>)))
                    converter = CreateCollectionConverter(typeSettings);
                else
                    return false;
            } else
                return false;

            return true;
        }

        private static IAsyncNodeConverter CreateCollectionConverter(TypeSettings typeSettings) {
            var collectionConverterType = typeof(CollectionNodeConverter<>).MakeGenericType(typeSettings.DataType.ElementType);
            return (IAsyncNodeConverter)Activator.CreateInstance(collectionConverterType, typeSettings);
        }

        private static IAsyncNodeConverter CreateArrayConverter(TypeSettings typeSettings) {
            var arrayConverterType = typeof(ArrayNodeConverter<>).MakeGenericType(typeSettings.DataType.ElementType);
            return (IAsyncNodeConverter)Activator.CreateInstance(arrayConverterType, typeSettings);
        }
    }
}
