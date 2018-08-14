// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ListNodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    public class ListNodeConverterFactory : NodeConverterFactoryBase {
        private static readonly HashSet<Type> StandardOpenGenerics = new HashSet<Type> {
                                                                                           typeof(IEnumerable<>),
                                                                                           typeof(Collection<>),
                                                                                           typeof(List<>),
                                                                                           typeof(Dictionary<,>),
                                                                                           typeof(ReadOnlyCollection<>)
                                                                                       };

        public override bool TryCreateConverter(NodeTypes nodeType, DataType dataType, out INodeConverter converter) {
            converter = null;
            if(nodeType != NodeTypes.List)
                return false;

            if((dataType.Flags & DataTypeFlags.CategoryMask) != DataTypeFlags.Collection)
                return false;

            var type = dataType.Type;
            if(type.IsArray)
                converter = CreateArrayConverter(dataType);
            else if(type.IsGenericType) {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if(genericTypeDefinition == typeof(IEnumerable<>))
                    converter = CreateArrayConverter(dataType);
                else if(StandardOpenGenerics.Contains(genericTypeDefinition) || type.IsAssignableToOpenGeneric(typeof(ICollection<>)))
                    converter = CreateCollectionConverter(dataType);
                else
                    return false;
            } else
                return false;

            return true;
        }

        private static INodeConverter CreateCollectionConverter(DataType dataType) {
            var collectionConverterType = typeof(CollectionNodeConverter<>).MakeGenericType(dataType.ElementType);
            return (INodeConverter)Activator.CreateInstance(collectionConverterType, dataType);
        }

        private static INodeConverter CreateArrayConverter(DataType dataType) {
            var arrayConverterType = typeof(ArrayNodeConverter<>).MakeGenericType(dataType.ElementType);
            return (INodeConverter)Activator.CreateInstance(arrayConverterType, dataType);
        }
    }
}
