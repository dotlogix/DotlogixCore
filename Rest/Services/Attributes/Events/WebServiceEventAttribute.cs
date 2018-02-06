using System;
using System.Collections.Generic;
using System.Text;

namespace DotLogix.Core.Rest.Services.Attributes.Events
{
    [AttributeUsage(AttributeTargets.Event)]
    public class WebServiceEventAttribute : Attribute
    {
        public WebServiceEventAttribute(string name) {
            Name = name;
        }
        public string Name { get; }
    }
}
