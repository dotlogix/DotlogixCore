// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ObjectNodeConverterFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using System.Reflection;
using DotLogix.Core.Nodes.Converters;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Factories {
    /// <summary>
    /// An implementation of the <see cref="INodeConverterFactory"/> for objects
    /// </summary>
    public class ObjectNodeConverterFactory : NodeConverterFactoryBase {
        /// <inheritdoc />
        public override bool TryCreateConverter(INodeConverterResolver resolver, TypeSettings typeSettings, out IAsyncNodeConverter converter) {
            converter = null;
            if(typeSettings.NodeType != NodeTypes.Map)
                return false;
            if((typeSettings.DataType.Flags & DataTypeFlags.CategoryMask) != DataTypeFlags.Complex)
                return false;

            var memberSettings = new List<MemberSettings>();
            foreach(var dynamicProperty in typeSettings.DynamicType.Properties) {
                if(dynamicProperty.PropertyInfo.IsDefined(typeof(IgnoreMemberAttribute)))
                    continue;

                if(resolver.TryResolve(typeSettings, dynamicProperty, out var memberSetting) == false)
                    return false;
                
                memberSettings.Add(memberSetting);
            }
            converter = new ObjectNodeConverter(typeSettings, memberSettings);
            return true;
        }
    }
}
