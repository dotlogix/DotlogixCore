using System;

namespace DotLogix.Core.Rest.Server.Http {
    public class WebServiceEventArgs : EventArgs {
        public string Channel { get; }

        public WebServiceEventArgs(string channel = null) {
            Channel = channel;
        }
    }
}