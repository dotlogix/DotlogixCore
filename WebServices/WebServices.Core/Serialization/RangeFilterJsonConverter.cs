using System;
using DotLogix.Core;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Terms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotLogix.WebServices.Core.Serialization; 

public class RangeFilterJsonConverter<T> : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        var instance = (RangeTerm<T>)value;

        if(instance.Min.IsUndefined && instance.Max.IsUndefined)
            return;
            
            
        if(instance.Min.IsDefined && Equals(value, instance.Max.Value)) {
            serializer.Serialize(writer, instance.Min.Value);
            return;
        }
            
        writer.WriteStartObject();
        if(instance.Min.IsDefined) {
            writer.WritePropertyName("min");
            serializer.Serialize(writer, instance.Min.Value);
        }
            
        if(instance.Max.IsDefined) {
            writer.WritePropertyName("max");
            serializer.Serialize(writer, instance.Max.Value);
        }
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        var token = JToken.ReadFrom(reader);
        var filter = new RangeTerm<T>();
        switch(token) {
            case JObject jObject: {
                filter.Min = jObject.TryGetValue("min", out var vToken) ? vToken.ToObject<Optional<T>>(serializer) : default;
                filter.Max = jObject.TryGetValue("max", out vToken) ? vToken.ToObject<Optional<T>>(serializer) : default;
                break;
            }
            default:
                filter.Min = token.ToObject<Optional<T>>(serializer);
                filter.Max = filter.Min;
                break;
        }
        return filter;
    }

    public override bool CanConvert(Type objectType) {
        return objectType.IsAssignableToGeneric(typeof(RangeTerm<>));
    }
}