// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Types;
using DotLogix.Core.Utils.Naming;

#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// A base class for node converters
    /// </summary>
    public abstract class NodeConverter : INodeConverter {
        /// <summary>
        /// Creates a new instance of <see cref="NodeConverter"/>
        /// </summary>
        protected NodeConverter(TypeSettings typeSettings) {
            TypeSettings = typeSettings;
            DataType = typeSettings.DataType;
            Type = typeSettings.DataType.Type;
        }

        /// <inheritdoc />
        public Type Type { get; }
        /// <inheritdoc />
        public DataType DataType { get; }
        /// <inheritdoc />
        public TypeSettings TypeSettings { get; }

        /// <inheritdoc />
        public abstract void Write(object instance, INodeWriter writer, IReadOnlyConverterSettings settings);

        /// <inheritdoc />
        public virtual object Read(INodeReader reader, IReadOnlyConverterSettings settings)
        {
            var node = reader.ReadNode();
            return ConvertToObject(node, settings);
        }

        /// <inheritdoc />
        public abstract object ConvertToObject(Node node, IReadOnlyConverterSettings settings);

        protected static string GetMemberName(MemberSettings member, INamingStrategy strategy)
        {
            if (member.Name != null)
                return member.Name;

            if (strategy != null)
                return strategy.Rewrite(member.Accessor.Name);
            
            return member.Accessor.Name;
        }
    }
}
