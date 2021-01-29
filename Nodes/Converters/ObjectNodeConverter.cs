// ================================================== Copyright 2018(C) , DotLogix
// File:  ObjectNodeConverter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018 ==================================================

#region

using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
using DotLogix.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Nodes.Formats;
using DotLogix.Core.Nodes.Formats.Nodes;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Utils.Naming;

#endregion

namespace DotLogix.Core.Nodes.Converters
{
    /// <summary>
    /// An implementation of the <see cref="INodeConverter"/> interface to convert objects
    /// </summary>
    public class ObjectNodeConverter : NodeConverter
    {
        private static readonly SelectorComparer<MemberSettings, int> SettingsOrderComparer = new SelectorComparer<MemberSettings, int>(s => s.Order ?? int.MaxValue);
        private Dictionary<INamingStrategy, MemberCache> MemberNameCache { get; } = new Dictionary<INamingStrategy, MemberCache>();

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
            Ctor = dynamicType.HasDefaultConstructor
                       ? dynamicType.DefaultConstructor
                       : null;
        }

        /// <inheritdoc/>
        public override void Write(object instance, INodeWriter writer, IReadOnlyConverterSettings settings)
        {
            var scopedSettings = settings.GetScoped(TypeSettings);
            if (scopedSettings.ShouldEmitValue(instance) == false)
                return;

            if(instance == null) {
                writer.WriteValue(null);
                return;
            }

            writer.WriteBeginMap();

            var namingStrategy = scopedSettings.NamingStrategy;
            var memberNames = EnsureNameCache(namingStrategy);
            for (var i = 0; i < MemberSettings.Length; i++)
            {
                var member = MemberSettings[i];
                if (member.Accessor.CanRead == false)
                    continue;

                var scopedMemberSettings = scopedSettings.GetScoped(memberSettings: member);

                var memberValue = member.Accessor.GetValue(instance);
                writer.WriteName(memberNames.GetRewrittenName(i));
                member.Converter.Write(memberValue, writer, scopedMemberSettings);
            }

            writer.WriteEndMap();
        }

        private MemberCache EnsureNameCache(INamingStrategy namingStrategy)
        {
            MemberCache CreateMemberNames(INamingStrategy s)
            {
                return new MemberCache(MemberSettings.Select(m => GetMemberName(m, s)).ToArray());
            }

            return MemberNameCache.GetOrAdd(namingStrategy, CreateMemberNames);
        }

        /// <inheritdoc />
        public override object Read(INodeReader reader, IReadOnlyConverterSettings settings) {
            var next = reader.PeekOperation();
            if (next.HasValue == false || (next.Value.Type == NodeOperationTypes.Value && next.Value.Value == null)) {
                reader.ReadOperation();
                return default;
            }

            return Ctor != null
                       ? ReadWithDefaultCtor(reader, settings)
                       : ReadAutoSelectCtor(reader, settings);
        }


        /// <inheritdoc />
        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings)
        {
            if (node.Type == NodeTypes.Empty)
                return default;

            if (!(node is NodeMap nodeMap))
                throw new ArgumentException($"Expected node of type \"NodeMap\" got \"{node.Type}\"");

            return Ctor != null
                       ? ConvertToObjectWithDefaultCtor(nodeMap, settings)
                       : ConvertToObjectAutoSelectCtor(nodeMap, settings);
        }

        private object ReadWithDefaultCtor(INodeReader reader, IReadOnlyConverterSettings settings) {
            reader.ReadBeginMap();

            var instance = Ctor.Invoke();
            var scopedSettings = settings.GetScoped(TypeSettings);
            var memberNames = EnsureNameCache(scopedSettings.NamingStrategy);

            while (true)
            {
                var next = reader.PeekOperation();
                if (next.HasValue == false || next.Value.Type == NodeOperationTypes.EndMap)
                    break;

                var memberIdx = memberNames.GetMemberIndex(next.Value.Name);
                if (memberIdx < 0) {
                    reader.SkipNode();
                    continue;
                }

                var memberSettings = MemberSettings[memberIdx];
                if (memberSettings.Accessor.CanWrite == false)
                    continue;

                var scopedMemberSettings = scopedSettings.GetScoped(memberSettings: memberSettings);
                var memberValue = memberSettings.Converter.Read(reader, scopedMemberSettings);
                memberSettings.Accessor.SetValue(instance, memberValue);
            }

            reader.ReadEndMap();

            return instance;
        }

        private object ReadAutoSelectCtor(INodeReader reader, IReadOnlyConverterSettings settings) {
            
            reader.ReadBeginMap();

            var scopedSettings = settings.GetScoped(TypeSettings);
            var memberNames = EnsureNameCache(scopedSettings.NamingStrategy);

            var memberValues = new object[MemberSettings.Length];

            while (true)
            {
                var next = reader.PeekOperation();
                if (next.HasValue == false || next.Value.Type == NodeOperationTypes.EndMap)
                    break;

                var memberIdx = memberNames.GetMemberIndex(next.Value.Name);
                if (memberIdx < 0) {
                    reader.SkipNode();
                    continue;
                }

                var memberSettings = MemberSettings[memberIdx];
                var scopedMemberSettings = scopedSettings.GetScoped(memberSettings: memberSettings);
                var memberValue = memberSettings.Converter.Read(reader, scopedMemberSettings);
                memberValues[memberIdx] = memberValue;
            }

            reader.ReadEndMap();

            foreach (var ctor in TypeSettings.DynamicType.Constructors)
            {
                if (TryConstructWith(ctor, memberValues, out var instance))
                    return instance;
            }

            return default;
        }

        /// <inheritdoc/>
        private object ConvertToObjectWithDefaultCtor(NodeMap node, IReadOnlyConverterSettings settings)
        {
            var scopedSettings = settings.GetScoped(TypeSettings);
            var instance = Ctor.Invoke();

            var memberNames = EnsureNameCache(scopedSettings.NamingStrategy);
            for (var i = 0; i < MemberSettings.Length; i++)
            {
                var memberSettings = MemberSettings[i];
                if (memberSettings.Accessor.CanWrite == false)
                    continue;

                var scopedMemberSettings = scopedSettings.GetScoped(memberSettings: memberSettings);

                var memberNode = node.GetChild(memberNames.GetRewrittenName(i));
                if (memberNode == null)
                    continue;


                var memberValue = memberSettings.Converter.ConvertToObject(memberNode, scopedMemberSettings);
                memberSettings.Accessor.SetValue(instance, memberValue);
            }

            return instance;
        }

        /// <inheritdoc/>
        private object ConvertToObjectAutoSelectCtor(NodeMap node, IReadOnlyConverterSettings settings)
        {
            var scopedSettings = settings.GetScoped(TypeSettings);

            var memberNames = EnsureNameCache(scopedSettings.NamingStrategy);
            var memberValues = new object[MemberSettings.Length];
            for (var i = 0; i < MemberSettings.Length; i++)
            {
                var memberSettings = MemberSettings[i];
                var scopedMemberSettings = scopedSettings.GetScoped(memberSettings: memberSettings);

                var memberNode = node.GetChild(memberNames.GetRewrittenName(i));
                if (memberNode == null)
                    continue;


                var memberValue = memberSettings.Converter.ConvertToObject(memberNode, scopedMemberSettings);
                memberValues[i] = memberValue;
            }

            foreach (var ctor in TypeSettings.DynamicType.Constructors)
            {
                if (TryConstructWith(ctor, memberValues, out var instance))
                    return instance;
            }

            return default;
        }

        private bool TryConstructWith(DynamicCtor ctor, object[] values, out object instance) {
            instance = null;
            var parameters = ctor.Parameters;
            var parameterCount = parameters.Length;

            if(parameterCount > values.Length) {
                return false;
            }

            var members = new object[parameterCount];
            for (var i = 0; i < parameterCount; i++)
            {
                var parameter = parameters[i];
                var memberIdx = Array.FindIndex(MemberSettings, a => a.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
                if(memberIdx <= 0)
                    return false;

                var value = values[memberIdx];
                if(parameter.ParameterType.IsInstanceOfType(value) == false)
                    return false;

                members[i] = value;
            }

            instance = ctor.Invoke(members);
            for(var i = 0; i < values.Length; i++) {
                var memberSettings = MemberSettings[i];
                if(memberSettings.Accessor.CanWrite == false)
                    continue;

                memberSettings.Accessor.SetValue(instance, values[i]);
            }
            return true;
        }
        
        private class MemberCache
        {
            private readonly string[] _memberNames;
            private readonly Dictionary<string, int> _memberMap;

            public MemberCache(string[] memberNames) {
                _memberNames = memberNames;
                _memberMap = new Dictionary<string, int>(memberNames.Length, StringComparer.Ordinal);
                for(var i = 0; i < memberNames.Length; i++) {
                    _memberMap.Add(memberNames[i], i);
                }
            }

            public int GetMemberIndex(string rewrittenName) {
                return _memberMap.TryGetValue(rewrittenName, out var index) ? index : -1;
            }

            public string GetRewrittenName(int memberIndex) {
                return _memberNames[memberIndex];
            }
        }
    }
}
