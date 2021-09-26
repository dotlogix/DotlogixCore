using System.Collections.Generic;
using System.Net;
using System.Text;
using DotLogix.Core.Extensions;

namespace DotLogix.WebServices.Core.Errors {
    public class ValidationApiException : TypedApiException {
        public object Value { get; set; }
        public string PropertyName { get; set; }

        public ValidationApiException(string message = null)
            : base(ApiErrorKinds.Validation, message, HttpStatusCode.BadRequest) {
        }

        protected override string GetErrorMessage() {
            if(Message != null) {
                return Message;
            }
            
            var sb = new StringBuilder();
            if(PropertyName != null) {
                sb.Append($"Can not validate property {PropertyName} of type {ClrType.GetFriendlyName()}");
            } else if(Value != null) {
                sb.Append($"Can not validate instance of type {ClrType.GetFriendlyName()}");
            }
            return sb.ToString();
        }

        protected override void AppendContext(IDictionary<string, object> dictionary) {
            base.AppendContext(dictionary);
            if(Value != null) {
                dictionary.Add("Instance", Value);
            }
            if(PropertyName != null) {
                dictionary.Add("Property", PropertyName);
            }
            if(Value != null) {
                dictionary.Add("Value", Value);
            }
        }
    }
}