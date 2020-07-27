// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Types;
#endregion

namespace DotLogix.Core.Nodes.Converters {
    /// <summary>
    /// A base class for node converters
    /// </summary>
    public abstract class NodeConverter : IAsyncNodeConverter {
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
        public abstract ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, IReadOnlyConverterSettings settings);

        /// <inheritdoc />
        public abstract object ConvertToObject(Node node, IReadOnlyConverterSettings settings);

        protected static string GetMemberName(MemberSettings member, IReadOnlyConverterSettings settings) {
            return member.Name ?? settings.NamingStrategy?.Rewrite(member.Accessor.Name) ?? member.Accessor.Name;
        }
    }
}
