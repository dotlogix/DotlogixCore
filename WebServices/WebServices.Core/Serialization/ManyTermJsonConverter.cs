using System;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Terms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotLogix.WebServices.Core.Serialization; 

public class ManyTermJsonConverter<T> : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        var filter = (ManyTerm<T>)value;

        switch(filter.Count)
        {
            case 0:
                return;
            case 1:
                serializer.Serialize(writer, filter.Values[0]);
                return;
            default:
                serializer.Serialize(writer, filter.Values);
                break;
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        var token = JToken.ReadFrom(reader);;
        switch(token) {
            case JArray jArray:
                var array = jArray.ToObject<T[]>();
                return array.Length > 0 ? new ManyTerm<T>(array) : null;
            default:
                return new ManyTerm<T>(token.ToObject<T>().CreateArray());
        }
    }

    public override bool CanConvert(Type objectType) {
        return objectType.IsAssignableTo(typeof(ManyTerm<T>));
    }
}