using System;
using DotLogix.Core;
using DotLogix.Core.Extensions;
using Newtonsoft.Json;

namespace DotLogix.WebServices.Core.Serialization {
    public class OptionalJsonConverter<T> : JsonConverter {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var optional = (Optional<T>)value;
            
            if(optional.IsUndefined)
                return;
            
            serializer.Serialize(writer, optional.Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var innerValue = serializer.Deserialize<T>(reader);
            return new Optional<T>(innerValue);
        }

        public override bool CanConvert(Type objectType) {
            return objectType.IsAssignableTo(typeof(Optional<T>));
        }
    }
}