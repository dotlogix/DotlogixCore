using System;

namespace DotLogix.Core.Rest.Services.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class WebServiceAttribute : Attribute {
        public string Name { get; set; }
        public string Route { get; set; }

        /// <inheritdoc />
        public WebServiceAttribute() { }

        /// <inheritdoc />
        public WebServiceAttribute(string route) {
            Route = route;
        }

        /// <inheritdoc />
        public WebServiceAttribute(string name, string route) {
            Name = name;
            Route = route;
        }
    }
}