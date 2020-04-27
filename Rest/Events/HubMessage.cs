//using System;
//using System.Collections.Generic;
//using DotLogix.Core.Nodes;

//namespace DotLogix.Core.Rest.Services.Events {
//    public class HubMessage : HubMessage<object> {
//        /// <inheritdoc />
//        public HubMessage(HubMessageType type, DateTime timestampUtc, object payload = default, Guid? guid = null) : base(type, timestampUtc, payload, guid) { }

//    }
//    public abstract class HubMessage<T> : IHubMessage {
//        public Guid Guid { get; }
//        public HubMessageType Type { get; }
//        public DateTime TimestampUtc { get; }
//        public T Payload { get; protected set; }
//        object IHubMessage.Payload => Payload;

//        /// <inheritdoc />
//        public HubMessage(HubMessageType type, DateTime timestampUtc, T payload = default, Guid? guid = null) {
//            Guid = guid ?? Guid.NewGuid();
//            Type = type;
//            TimestampUtc = timestampUtc;
//            Payload = payload;
//        }

//        protected bool Equals(HubMessage<T> other) {
//            return Guid.Equals(other.Guid);
//        }

//        /// <inheritdoc />
//        public override bool Equals(object obj) {
//            if(ReferenceEquals(null, obj))
//                return false;
//            if(ReferenceEquals(this, obj))
//                return true;

//            return obj is HubMessage<T> m && Equals(m);
//        }

//        /// <inheritdoc />
//        public override int GetHashCode() {
//            unchecked {
//                return (Guid.GetHashCode() * 397) ^ Type.GetHashCode();
//            }
//        }

//        public static bool operator ==(HubMessage<T> left, HubMessage<T> right) {
//            return Equals(left, right);
//        }

//        public static bool operator !=(HubMessage<T> left, HubMessage<T> right) {
//            return !Equals(left, right);
//        }
//    }
//    public class HubEventMessage : HubMessage<Node> {
//        public Guid Issuer { get; set; }
//        public IEnumerable<Guid> Targets { get; set; }

//        public HubEventMessage(DateTime timestampUtc, Node payload = default, Guid? guid = null) : base(HubMessageType.Custom, timestampUtc, payload, guid) {
//        }
//    }
//    public class HubBinaryMessage : HubMessage<ArraySegment<byte>> {
//        public HubBinaryMessage(DateTime timestampUtc, ArraySegment<byte> payload = default, Guid? guid = null) : base(HubMessageType.Custom, timestampUtc, payload, guid) {
//        }
//    }
//    public class HubTextMessage : HubMessage<string> {
//        public HubTextMessage(DateTime timestampUtc, string payload = default, Guid? guid = null) : base(HubMessageType.Custom, timestampUtc, payload, guid) {
//        }
//    }
//    public class HubCustomMessage : HubMessage<Node> {
//        public string MessageType { get; set; }
//        public HubCustomMessage(DateTime timestampUtc, Node payload = default, Guid? guid = null) : base(HubMessageType.Custom, timestampUtc, payload, guid) {
//        }
//    }

//    public class HubSubscribeMessage : HubMessage<Node> {
//        public string EventName { get; set; }
//        public HubSubscribeMessage(DateTime timestampUtc, Node payload = default, Guid? guid = null) : base(HubMessageType.Event, timestampUtc, payload, guid) {
//        }
//    }
//    public class HubUnsubscribeMessage : HubMessage<Node> {
//        public string EventName { get; set; }

//        public HubUnsubscribeMessage(DateTime timestampUtc, Node payload = default, Guid? guid = null) : base(HubMessageType.Event, timestampUtc, payload, guid) {
//        }
//    }
//}