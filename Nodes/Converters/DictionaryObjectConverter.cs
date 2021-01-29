using System;
using System.Collections.Generic;
using DotLogix.Core.Extensions;
using DotLogix.Core.Nodes.Formats;
using DotLogix.Core.Nodes.Formats.Nodes;
using DotLogix.Core.Nodes.Schema;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Nodes.Converters {
    public class DictionaryObjectConverter<TKey, TValue> : NodeConverter {
        protected TypeSettings ValueSettings { get; }
        protected DynamicCtor Ctor { get; set; }


        public DictionaryObjectConverter(TypeSettings typeSettings, TypeSettings valueSettings) : base(typeSettings) {
            ValueSettings = valueSettings;
            Ctor = typeSettings.DynamicType.DefaultConstructor;
            if(Ctor == null)
                throw new ArgumentException($"Type {typeSettings.DynamicType.Type.GetFriendlyGenericName()} does not define a default constructor");
        }

        public override void Write(object instance, INodeWriter writer, IReadOnlyConverterSettings settings) {
            var scopedSettings = settings.GetScoped(TypeSettings);
            if (scopedSettings.ShouldEmitValue(instance) == false)
                return;

            if (instance == null)
            {
                writer.WriteValue(null);
                return;
            }

            var dict = (IDictionary<TKey, TValue>)instance;
            writer.WriteBeginMap();
            foreach(var kv in dict) {
                writer.WriteName(kv.Key.ToString());
                ValueSettings.Converter.Write(kv.Value, writer, scopedSettings);
            }
            writer.WriteEndMap();


        }

        public override object Read(INodeReader reader, IReadOnlyConverterSettings settings) {
            var next = reader.PeekOperation();
            if (next.HasValue == false || (next.Value.Type == NodeOperationTypes.Value && next.Value.Value == null)) {
                reader.ReadOperation();
                return default;
            }

            var scopedSettings = settings.GetScoped(TypeSettings);
            var instance = (IDictionary<TKey, TValue>)Ctor.Invoke();
            reader.ReadBeginMap();
            while (true)
            {
                next = reader.PeekOperation();
                if (next.HasValue == false || next.Value.Type == NodeOperationTypes.EndMap)
                    break;

                var key = next.Value.Name.ConvertTo<TKey>();
                var value = (TValue)ValueSettings.Converter.Read(reader, scopedSettings);
                instance.Add(key, value);
            }
            reader.ReadEndMap();
            return instance;
        }

        public override object ConvertToObject(Node node, IReadOnlyConverterSettings settings) {
            if (node.Type == NodeTypes.Empty)
                return default;

            if (!(node is NodeMap nodeMap))
                throw new ArgumentException($"Expected node of type \"NodeMap\" got \"{node.Type}\"");

            var scopedSettings = settings.GetScoped(TypeSettings);
            var instance = (IDictionary<TKey, TValue>)Ctor.Invoke();
            foreach(var nodeChild in nodeMap.Children()) {
                var key = nodeChild.Name.ConvertTo<TKey>();
                var value = (TValue)ValueSettings.Converter.ConvertToObject(nodeChild, scopedSettings);
                instance.Add(key, value);
            }

            return instance;
        }
    }
}