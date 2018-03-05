using System;
using DotLogix.Core.Rest.Server.Http;

namespace DotLogix.Core.Rest.Services.Attributes.Events {
    [AttributeUsage(AttributeTargets.Event)]
    public abstract class WebServiceEventAttributeBase : Attribute {
        public string Name { get; }

        protected WebServiceEventAttributeBase(string name) {
            Name = name;
        }

        public abstract IWebServiceEvent CreateEvent();
    }
}