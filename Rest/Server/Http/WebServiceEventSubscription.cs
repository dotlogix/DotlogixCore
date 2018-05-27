using System;
using DotLogix.Core.Rest.Server.Http.Context;
using DotLogix.Core.Rest.Server.Routes;

namespace DotLogix.Core.Rest.Server.Http {
    public class WebServiceEventSubscription : IWebServiceEventSubscription {
        public IAsyncHttpContext Context { get; }
        public IWebServiceRoute Route { get; }
        public AsyncWebRequestRouter Router { get; }
        public string Channel { get; }

        public WebServiceEventSubscription(IAsyncHttpContext asyncHttpContext, IWebServiceRoute route, AsyncWebRequestRouter asyncWebRequestRouter) {
            Guid = Guid.NewGuid();
            Context = asyncHttpContext;
            Route = route;
            Router = asyncWebRequestRouter;
            if(Context.Request.HeaderParameters.TryGetParameterValue(AsyncWebRequestRouter.EventSubscriptionChannelParameterName, out var channel))
                Channel = (string)channel;
        }

        public Guid Guid { get; }
        public bool CheckPreCondition(object sender, WebServiceEventArgs eventArgs) {
            return Channel == null || string.Equals(eventArgs.Channel, Channel, StringComparison.OrdinalIgnoreCase);
        }

        protected bool Equals(WebServiceEventSubscription other) {
            return Guid.Equals(other.Guid);
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj))
                return false;
            if(ReferenceEquals(this, obj))
                return true;
            if(obj.GetType() != GetType())
                return false;
            return Equals((WebServiceEventSubscription)obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() {
            return Guid.GetHashCode();
        }

        /// <summary>Returns a value that indicates whether the values of two <see cref="T:DotLogix.Core.Rest.Server.Http.WebServiceEventSubscription" /> objects are equal.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if the <paramref name="left" /> and <paramref name="right" /> parameters have the same value; otherwise, false.</returns>
        public static bool operator ==(WebServiceEventSubscription left, WebServiceEventSubscription right) {
            return Equals(left, right);
        }

        /// <summary>Returns a value that indicates whether two <see cref="T:DotLogix.Core.Rest.Server.Http.WebServiceEventSubscription" /> objects have different values.</summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>true if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, false.</returns>
        public static bool operator !=(WebServiceEventSubscription left, WebServiceEventSubscription right) {
            return !Equals(left, right);
        }
    }
}