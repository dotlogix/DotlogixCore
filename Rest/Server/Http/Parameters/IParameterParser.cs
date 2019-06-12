using System.Collections.Generic;
using System.Collections.Specialized;

namespace DotLogix.Core.Rest.Server.Http.Parameters {
    public interface IParameterParser {
        IDictionary<string, object> Deserialize(NameValueCollection collection);
        TCollection Serialize<TCollection>(IDictionary<string, object> parameters) where TCollection : NameValueCollection, new();
        void DeserializeValue(string name, string[] values, IDictionary<string, object> parameters);
        void SerializeValue(string name, object value, NameValueCollection collection);
    }
}