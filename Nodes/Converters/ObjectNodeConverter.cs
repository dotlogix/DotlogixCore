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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotLogix.Core.Utils.Naming;

#endregion

namespace DotLogix.Core.Nodes.Converters
{
    public class MemberNameCacheItem
    {
        public MemberSettings Settings { get; }
        public bool Serialize { get; }
        public bool Use { get; }
        public bool UseInCtor { get; }


        public string RewrittenName { get; }
    }



    /// <summary>
    /// An implementation of the <see cref="IAsyncNodeConverter"/> interface to convert objects
    /// </summary>
    public class ObjectNodeConverter : NodeConverter
    {
        private static readonly SelectorEqualityComparer<MemberSettings, DynamicAccessor> SettingsEqualityComparer = new SelectorEqualityComparer<MemberSettings, DynamicAccessor>(s => s.Accessor);
        private static readonly SelectorComparer<MemberSettings, int> SettingsOrderComparer = new SelectorComparer<MemberSettings, int>(s => s.Order ?? int.MaxValue);
        private Dictionary<INamingStrategy, string[]> MemberNameCache { get; } = new Dictionary<INamingStrategy, string[]>();

        public DynamicCtor Ctor { get; }
        public MemberSettings[] MemberSettings { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ObjectNodeConverter"/>
        /// </summary>
        public ObjectNodeConverter(TypeSettings typeSettings, IEnumerable<MemberSettings> memberSettings) : base(typeSettings)
        {
            MemberSettings = memberSettings.AsArray();

            Array.Sort(MemberSettings, SettingsOrderComparer);

            var dynamicType = typeSettings.DynamicType;
            if (!dynamicType.HasDefaultConstructor)
            {
                throw new ArgumentException($"Type {dynamicType.Name} does not have a default constructor");
            }

            Ctor = dynamicType.DefaultConstructor;


            //foreach (var ctor in dynamicType.Constructors)
            //{
            //    if (CanConstructWith(ctor, MemberSettings, out var neededMembers) == false)
            //        continue;
            //    Ctor = ctor;
            //    MembersToDeserialize = MemberSettings.Where(a => a.Accessor.CanWrite).Except(neededMembers, SettingsEqualityComparer).ToArray();
            //    MembersForCtor = neededMembers;
            //}
        }

        /// <inheritdoc/>
        public override async Task WriteAsync(object instance, IAsyncNodeWriter writer, IReadOnlyConverterSettings settings)
        {
            var scopedSettings = settings.GetScoped(TypeSettings);
            if (scopedSettings.ShouldEmitValue(instance) == false)
                return;

            if(instance == null) {
                await writer.WriteValueAsync(null).ConfigureAwait(false);
                return;
            }

            await writer.WriteBeginMapAsync().ConfigureAwait(false);

            var namingStrategy = scopedSettings.NamingStrategy;
            var memberNames = EnsureMemberNames(namingStrategy);
            for (var i = 0; i < MemberSettings.Length; i++)
            {
                var member = MemberSettings[i];
                if (member.Accessor.CanRead == false)
                    continue;

                var scopedMemberSettings = scopedSettings.GetScoped(memberSettings: member);

                var memberValue = member.Accessor.GetValue(instance);
                await writer.WriteNameAsync(memberNames[i]).ConfigureAwait(false);
                await member.Converter.WriteAsync(memberValue, writer, scopedMemberSettings).ConfigureAwait(false);
            }

            await writer.WriteEndMapAsync().ConfigureAwait(false);
        }

        private string[] EnsureMemberNames(INamingStrategy namingStrategy)
        {
            string[] CreateMemberNames(INamingStrategy s)
            {
                return MemberSettings.Select(m => GetMemberName(m, s)).ToArray();
            }

            return MemberNameCache.GetOrAdd(namingStrategy, CreateMemberNames);
        }

        /// <inheritdoc />
        public override async Task<object> ReadAsync(IAsyncNodeReader reader, IReadOnlyConverterSettings settings) {
            var next = await reader.PeekOperationAsync().ConfigureAwait(false);
            if (next.HasValue == false || (next.Value.Type == NodeOperationTypes.Value && next.Value.Value == null))
                return default;
            await reader.ReadBeginMapAsync().ConfigureAwait(false);

            var instance = Ctor.Invoke();
            var scopedSettings = settings.GetScoped(TypeSettings);
            var memberNames = EnsureMemberNames(scopedSettings.NamingStrategy);
            while (true) {
                next = await reader.PeekOperationAsync().ConfigureAwait(false);
                if (next.HasValue == false || next.Value.Type == NodeOperationTypes.EndMap)
                    break;

                var memberIdx = Array.IndexOf(memberNames, next.Value.Name);
                var memberSettings = MemberSettings[memberIdx];
                if (memberSettings.Accessor.CanWrite == false)
                    continue;

                var scopedMemberSettings = scopedSettings.GetScoped(memberSettings: memberSettings);

                var memberValue = await memberSettings.Converter.ReadAsync(reader, scopedMemberSettings).ConfigureAwait(false);
                memberSettings.Accessor.SetValue(instance, memberValue);
            }

            await reader.ReadEndMapAsync().ConfigureAwait(false);

            return instance;
        }

        /// <inheritdoc/>
        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings)
        {
            if (node.Type == NodeTypes.Empty)
                return default;

            if (!(node is NodeMap nodeMap))
                throw new ArgumentException($"Expected node of type \"NodeMap\" got \"{node.Type}\"");

            var scopedSettings = settings.GetScoped(TypeSettings);
            object instance = Ctor.Invoke();
            //if (Ctor.IsDefault)
            //    instance = Ctor.Invoke();
            //else if (TryConstructWith(Ctor, nodeMap, scopedSettings, out instance) == false)
            //    throw new InvalidOperationException("Object can not be constructed with the given nodes");

            var memberNames = EnsureMemberNames(scopedSettings.NamingStrategy);
            for (var i = 0; i < MemberSettings.Length; i++)
            {
                var memberSettings = MemberSettings[i];
                if (memberSettings.Accessor.CanWrite == false)
                    continue;

                var scopedMemberSettings = scopedSettings.GetScoped(memberSettings: memberSettings);

                var memberNode = nodeMap.GetChild(memberNames[i]);
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

        //private bool TryConstructWith(DynamicCtor ctor, NodeMap nodeMap, IReadOnlyConverterSettings settings, out object instance)
        //{
        //    instance = null;

        //    var parameterCount = MembersForCtor.Length;
        //    var parametersForCtor = new object[parameterCount];
        //    for (var i = 0; i < MembersForCtor.Length; i++)
        //    {
        //        var memberSettings = MembersForCtor[i];
        //        var scopedMemberSettings = settings.GetScoped(memberSettings);
        //        var memberNode = nodeMap.GetChild(GetMemberName(memberSettings, scopedMemberSettings));
        //        if (memberNode == null)
        //            continue;
        //        parametersForCtor[i] = memberSettings.Converter.ConvertToObject(memberNode, scopedMemberSettings);
        //    }

        //    instance = ctor.Invoke(parametersForCtor);
        //    return true;
        //}
    }
}
