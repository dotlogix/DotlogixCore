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
        public override async ValueTask WriteAsync(object instance, string name, IAsyncNodeWriter writer, IReadOnlyConverterSettings settings)
        {
            var scopedSettings = settings.GetScoped(TypeSettings);
            if (scopedSettings.ShouldEmitValue(instance) == false)
                return;

            if(instance == null) {
                await writer.WriteValueAsync(name, null).ConfigureAwait(false);
                return;
            }

            await writer.BeginMapAsync(name).ConfigureAwait(false);
            foreach (var member in MembersToSerialize) {
                var scopedMemberSettings = scopedSettings.GetScoped(memberSettings: member);

                var memberValue = member.Accessor.GetValue(instance);
                await member.Converter.WriteAsync(memberValue, GetMemberName(member, scopedMemberSettings), writer, scopedMemberSettings).ConfigureAwait(false);
            }
            await writer.EndMapAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings)
        {
            if (node.Type == NodeTypes.Empty)
                return default;

            if (!(node is NodeMap nodeMap))
                throw new ArgumentException($"Expected node of type \"NodeMap\" got \"{node.Type}\"");

            var scopedSettings = settings.GetScoped(TypeSettings);
            object instance;
            if (Ctor.IsDefault)
                instance = Ctor.Invoke();
            else if (TryConstructWith(Ctor, nodeMap, scopedSettings, out instance) == false)
                throw new InvalidOperationException("Object can not be constructed with the given nodes");

            foreach (var memberSettings in MembersToDeserialize)
            {
                var scopedMemberSettings = scopedSettings.GetScoped(memberSettings);

                var memberNode = nodeMap.GetChild(GetMemberName(memberSettings, scopedMemberSettings));
                if (memberNode == null)
                    continue;

                
                var memberValue = memberSettings.Converter.ConvertToObject(memberNode, scopedMemberSettings);
                memberSettings.Accessor.SetValue(instance, memberValue);
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

        private bool TryConstructWith(DynamicCtor ctor, NodeMap nodeMap, IReadOnlyConverterSettings settings, out object instance)
        {
            instance = null;

            var parameterCount = MembersForCtor.Length;
            var parametersForCtor = new object[parameterCount];
            for (var i = 0; i < MembersForCtor.Length; i++)
            {
                var memberSettings = MembersForCtor[i];
                var scopedMemberSettings = settings.GetScoped(memberSettings);
                var memberNode = nodeMap.GetChild(GetMemberName(memberSettings, scopedMemberSettings));
                if (memberNode == null)
                    continue;
                parametersForCtor[i] = memberSettings.Converter.ConvertToObject(memberNode, scopedMemberSettings);
            }

            instance = ctor.Invoke(parametersForCtor);
            return true;
        }
    }
}
