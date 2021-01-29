// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CollectionNodeConverterFactory.cs
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
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    ///     An implementation of the <see cref="INodeConverterFactory" /> for collection types
    /// </summary>
    public class CollectionNodeConverterFactory : NodeConverterFactoryBase {
        private static readonly Dictionary<Type, Type> InterfaceRemapping;
        private static readonly HashSet<Type> StandardOpenGenerics;

        static CollectionNodeConverterFactory() {
            InterfaceRemapping = new Dictionary<Type, Type> {
                {typeof(IEnumerable), typeof(List<>)},
                {typeof(IEnumerable<>), typeof(List<>)},

                {typeof(ICollection), typeof(List<>)},
                {typeof(ICollection<>), typeof(List<>)},
                {typeof(IReadOnlyCollection<>), typeof(List<>)},

                {typeof(ISet<>), typeof(HashSet<>)},
                {typeof(IList<>), typeof(List<>)},
                {typeof(IReadOnlyList<>), typeof(List<>)},
                {typeof(IDictionary<,>), typeof(Dictionary<,>)},
                {typeof(IReadOnlyDictionary<,>), typeof(Dictionary<,>)}
            };

            StandardOpenGenerics = new HashSet<Type> {
                typeof(Collection<>),
                typeof(List<>),
                typeof(Dictionary<,>),
                typeof(ReadOnlyCollection<>),
                typeof(HashSet<>)
            };
        }

        /// <inheritdoc />
        public override bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out INodeConverter converter) {
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
                if(type.IsInterface) {
                    if(InterfaceRemapping.TryGetValue(genericTypeDefinition, out var mappedType) == false)
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

        private static INodeConverter CreateCollectionConverter(TypeSettings typeSettings) {
            var collectionConverterType = typeof(CollectionNodeConverter<>).MakeGenericType(typeSettings.DataType.ElementType);
            return (INodeConverter)Activator.CreateInstance(collectionConverterType, typeSettings);
        }

        private static INodeConverter CreateArrayConverter(TypeSettings typeSettings) {
            var arrayConverterType = typeof(ArrayNodeConverter<>).MakeGenericType(typeSettings.DataType.ElementType);
            return (INodeConverter)Activator.CreateInstance(arrayConverterType, typeSettings);
        }
    }
}
