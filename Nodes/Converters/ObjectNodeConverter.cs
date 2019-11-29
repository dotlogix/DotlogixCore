// ================================================== Copyright 2018(C) , DotLogix
// File:  ObjectNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018 ==================================================

#region

using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Processor;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace DotLogix.Core.Nodes.Converters
{
    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to convert objects
    /// </summary>
    public class ObjectNodeConverter : NodeConverter
    {
        private static readonly SelectorEqualityComparer<MemberSettings, DynamicAccessor> SettingsEqualityComparer = new SelectorEqualityComparer<MemberSettings, DynamicAccessor>(s => s.Accessor);
        private static readonly SelectorComparer<MemberSettings, int> SettingsOrderComparer = new SelectorComparer<MemberSettings, int>(s => s.Order ?? int.MaxValue);

        public DynamicCtor Ctor { get; }
        public MemberSettings[] MemberSettings { get; }

        public MemberSettings[] MembersToSerialize { get; }
        public MemberSettings[] MembersToDeserialize { get; }
        public MemberSettings[] MembersForCtor { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ObjectNodeConverter"/>
        /// </summary>
        public ObjectNodeConverter(TypeSettings typeSettings, IEnumerable<MemberSettings> memberSettings) : base(typeSettings)
        {
            MemberSettings = memberSettings.AsArray();

            Array.Sort(MemberSettings, SettingsOrderComparer);

            MembersToSerialize = MemberSettings.Where(a => a.Accessor.CanRead).ToArray();

            var dynamicType = typeSettings.DynamicType;
            if (dynamicType.HasDefaultConstructor)
            {
                Ctor = dynamicType.DefaultConstructor;
                MembersToDeserialize = MemberSettings.Where(a => a.Accessor.CanWrite).ToArray();
                return;
            }

            foreach (var ctor in dynamicType.Constructors)
            {
                if (CanConstructWith(ctor, MemberSettings, out var neededMembers) == false)
                    continue;
                Ctor = ctor;
                MembersToDeserialize = MemberSettings.Where(a => a.Accessor.CanWrite).Except(neededMembers, SettingsEqualityComparer).ToArray();
                MembersForCtor = neededMembers;
            }
        }

        /// <inheritdoc/>
        public override async ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, ConverterSettings settings)
        {
            var task = writer.BeginMapAsync(name);
            if (task.IsCompletedSuccessfully == false)
                await task;
            foreach (var member in MembersToSerialize)
            {
                var memberValue = member.Accessor.GetValue(instance);
                if (member.ShouldEmitValue(memberValue, settings) == false)
                    continue;

                var converter = member.Converter;
                if (converter == null)
                {
                    if (settings.Resolver.TryResolve(member.DataType.Type, out var typeSettings) == false)
                        throw new NotSupportedException($"Can not resolve a converter for {member.Accessor.DeclaringType.Name}.{member.Accessor.Name} of type {member.DataType.Type}");
                    converter = typeSettings.Converter;
                }

                task = converter.WriteAsync(memberValue, GetMemberName(member, settings), writer, settings);

                if (task.IsCompletedSuccessfully == false)
                    await task;
            }
            task = writer.EndMapAsync();
            if (task.IsCompletedSuccessfully == false)
                await task;
        }

        /// <inheritdoc/>
        public override object ConvertToObject(Node node, ConverterSettings settings)
        {
            if (node.Type == NodeTypes.Empty)
                return default;

            if (!(node is NodeMap nodeMap))
                throw new ArgumentException("Node is not a NodeMap");

            object instance;
            if (Ctor.IsDefault)
                instance = Ctor.Invoke();
            else if (TryConstructWith(Ctor, nodeMap, settings, out instance) == false)
                throw new InvalidOperationException("Object can not be constructed with the given nodes");

            foreach (var member in MembersToDeserialize)
            {
                var memberNode = nodeMap.GetChild(GetMemberName(member, settings));
                if (memberNode == null)
                    continue;
                var memberValue = member.Converter.ConvertToObject(memberNode, settings);
                member.Accessor.SetValue(instance, memberValue);
            }
            return instance;
        }

        private bool CanConstructWith(DynamicCtor ctor, MemberSettings[] readableMembers, out MemberSettings[] neededAccessors)
        {
            neededAccessors = null;
            var parameters = ctor.Parameters;
            var parameterCount = parameters.Length;
            var members = new MemberSettings[parameterCount];
            for (var i = 0; i < parameterCount; i++)
            {
                var parameter = parameters[i];
                var member = readableMembers.FirstOrDefault(a => a.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
                if (member == null)
                    return false;
                members[i] = member;
            }
            neededAccessors = members;
            return true;
        }

        private bool TryConstructWith(DynamicCtor ctor, NodeMap nodeMap, ConverterSettings settings, out object instance)
        {
            instance = null;

            var parameterCount = MembersForCtor.Length;
            var parametersForCtor = new object[parameterCount];
            for (var i = 0; i < MembersForCtor.Length; i++)
            {
                var member = MembersForCtor[i];
                var memberNode = nodeMap.GetChild(GetMemberName(member, settings));
                if (memberNode == null)
                    continue;
                parametersForCtor[i] = member.Converter.ConvertToObject(memberNode, settings);
            }

            instance = ctor.Invoke(parametersForCtor);
            return true;
        }
    }
}
