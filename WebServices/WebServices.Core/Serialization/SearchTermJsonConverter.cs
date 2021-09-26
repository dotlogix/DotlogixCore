using System;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Terms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotLogix.WebServices.Core.Serialization {
    public class SearchTermJsonConverter : JsonConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var term = (SearchTerm)value;

            if(term.Mode == SearchTermMode.Equals) {
                writer.WriteValue(term.Pattern);
                return;
            }

            var obj = new JObject();
            obj.Add("mode", term.Mode.ToString());
            obj.Add("pattern", term.Pattern);
            obj.Add("ignoreCase", term.IgnoreCase);
            if(term.FuzzyThreshold.HasValue) {
                obj.Add("fuzzyThreshold", term.FuzzyThreshold.Value);
            }

            obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var token = JToken.ReadFrom(reader);
            switch(token) {
                case JObject jObject: {
                    return new SearchTerm {
                        Mode = jObject.Value<string>("mode").ConvertToEnum<SearchTermMode>(),
                        Pattern = jObject.Value<string>("pattern"),
                        FuzzyThreshold = jObject.Value<double?>("fuzzyThreshold"),
                        IgnoreCase = jObject.Value<bool>("ignoreCase")
                    };
                }
                case JValue jValue: {
                    var value = jValue.ToObject<string>();
                    if(value.Contains("*")) {
                        value = value.Replace("*", ".*");
                        return SearchTerm.Regex(value);
                    }
                    return SearchTerm.EqualTo(value);
                }
                default:
                    return null;
            }
        }

        public override bool CanConvert(Type objectType) {
            return objectType.IsAssignableTo<SearchTerm>();
        }
    }
}