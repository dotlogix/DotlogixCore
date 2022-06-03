using System;
using System.Linq;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Terms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotLogix.WebServices.Core.Serialization {
    public class SearchTermJsonConverter : JsonConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var term = (SearchTerm)value;
            var patternToken = SerializePattern(term.Pattern);
            
            if(term.Mode == SearchTermMode.Equals) {
                patternToken.WriteTo(writer);
                return;
            }

            var termToken = new JObject {
                {"mode", term.Mode.ToString()},
                {"pattern", patternToken},
                {"ignoreCase", term.IgnoreCase}
            };
            
            if(term.FuzzyThreshold.HasValue) {
                termToken.Add("fuzzyThreshold", term.FuzzyThreshold.Value);
            }
            termToken.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var token = JToken.ReadFrom(reader);
            return token switch {
                JObject jObject => DeserializeFromObject(jObject),
                JArray jArray => DeserializeFromArray(jArray),
                JValue jValue => DeserializeFromValue(jValue),
                _ => null
            };
        }
        private static SearchTerm DeserializeFromValue(JValue jValue) {
            var value = jValue.ToObject<string>();
            if(string.IsNullOrEmpty(value)) {
                return null;
            }
            return value.Contains("*")
                       ? SearchTerm.Wildcard(value)
                       : SearchTerm.EqualTo(value);
        }
        private static SearchTerm DeserializeFromArray(JArray jArray) {
            var values = jArray.ToObject<string[]>();
            if(values == null || values.Length == 0) {
                return null;
            }
            
            return values.Any(v => v.Contains("*"))
                       ? SearchTerm.Wildcard(values)
                       : SearchTerm.EqualTo(values);
        }
        private static SearchTerm DeserializeFromObject(JObject jObject) {
            var termModeToken = jObject.GetValue("mode");
            var patternToken = jObject.GetValue("pattern");

            var pattern = DeserializePattern(patternToken);
            if(pattern == null) {
                return null;
            }

            return new SearchTerm {
                Mode = termModeToken?.ToObject<string>().ConvertToEnum<SearchTermMode>() ?? SearchTermMode.Equals,
                Pattern = pattern,
                FuzzyThreshold = jObject.Value<double?>("fuzzyThreshold"),
                IgnoreCase = jObject.Value<bool>("ignoreCase")
            };
        }

        private static ManyTerm<string> DeserializePattern(JToken patternToken) {
            return patternToken switch {
                JArray array => array.ToObject<string[]>(),
                JValue value => value.ToObject<string>(),
                _ => null
            };
        }
        private static JToken SerializePattern(ManyTerm<string> pattern) {
            return pattern.Count == 1
                       ? JToken.FromObject(pattern.Values[0])
                       : JToken.FromObject(pattern.Values);
        }

        public override bool CanConvert(Type objectType) {
            return objectType.IsAssignableTo<SearchTerm>();
        }
    }
}