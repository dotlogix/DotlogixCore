// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AsyncHttpRequest.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  30.06.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotLogix.Core.Nodes;
using DotLogix.Core.Rest.Server.Http.Mime;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Context {
    public interface IParameterParser {
        NodeMap Deserialize(NameValueCollection collection);
        TCollection Serialize<TCollection>(NodeMap nodeMap) where TCollection : NameValueCollection, new();
        void DeserializeValue(string name, string[] values, NodeMap nodeMap);
        void SerializeValue(string name, Node valueNode, NameValueCollection collection);
    }

    public class ExtendedParameterParser : ParameterParserBase {
        public ExtendedParameterParser(IParameterParser fallBackParser = null) {
            FallBackParser = fallBackParser ?? PrimitiveParameterParser.Instance;
        }
        public IParameterParser FallBackParser { get; }
        public Dictionary<string, IParameterParser> SpecializedParsers { get; } = new Dictionary<string, IParameterParser>();
        public static ExtendedParameterParser Default => CreateDefaultParser();

        private static ExtendedParameterParser CreateDefaultParser() {
            var parser = new ExtendedParameterParser();
            return parser;
        }

        public override void DeserializeValue(string name, string[] values, NodeMap nodeMap) {
            if (SpecializedParsers.TryGetValue(name, out var parameterParser) == false) {
                parameterParser = FallBackParser;
            }
            parameterParser.DeserializeValue(name, values, nodeMap);
        }

        public override void SerializeValue(string name, Node valueNode, NameValueCollection collection) {
            if (SpecializedParsers.TryGetValue(name, out var parameterParser) == false)
            {
                parameterParser = FallBackParser;
            }
            parameterParser.SerializeValue(name, valueNode, collection);
        }
    }

    public class AsyncHttpRequest : IAsyncHttpRequest {
        private AsyncHttpRequest(HttpListenerRequest originalRequest, NodeMap headerMap, NodeMap queryMap) {
            OriginalRequest = originalRequest;
            HeaderParameters = headerMap;
            QueryParameters = queryMap;
            UrlParameters = new NodeMap();
            UserDefinedParameters = new NodeMap();
            ContentType = MimeType.Parse(originalRequest.ContentType);
            HttpMethod = AsyncHttpServer.HttpMethodFromString(originalRequest.HttpMethod);
            ContentLength64 = originalRequest.ContentLength64;
            ContentEncoding = originalRequest.ContentEncoding;
            InputStream = originalRequest.InputStream;
        }

        public NodeMap HeaderParameters { get; }
        public NodeMap QueryParameters { get; }
        public NodeMap UrlParameters { get; }
        public NodeMap UserDefinedParameters { get; }

        public Uri Url => OriginalRequest.Url;
        public HttpMethods HttpMethod { get; }
        public MimeType ContentType { get; }
        public long ContentLength64 { get; }
        public Encoding ContentEncoding { get; }
        public Stream InputStream { get; }

        public HttpListenerRequest OriginalRequest { get; }

        public virtual async Task<int> ReadDataFromRequestStreamAsync(byte[] data, int offset, int count) {
            return await InputStream.ReadAsync(data, offset, count);
        }

        public virtual async Task<byte[]> ReadDataFromRequestStreamAsync() {
            using(var memoryStream = new MemoryStream()) {
                await InputStream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public virtual async Task<string> ReadStringFromRequestStreamAsync() {
            var data = await ReadDataFromRequestStreamAsync();
            return ContentEncoding.GetString(data);
        }

        public static AsyncHttpRequest Create(HttpListenerRequest originalRequest, IParameterParser parameterParser) {
            var headerMap = parameterParser.Deserialize(originalRequest.Headers);
            var queryMap = parameterParser.Deserialize(originalRequest.QueryString);
            return new AsyncHttpRequest(originalRequest, headerMap, queryMap);
        }
    }


    public abstract class ParameterParserBase : IParameterParser {
        public NodeMap Deserialize(NameValueCollection collection) {
            var nodeMap = new NodeMap();
            for(var i = 0; i < collection.Count; i++) {
                var name = collection.GetKey(i);
                var values = collection.GetValues(i);

                DeserializeValue(name, values, nodeMap);
            }
            return nodeMap;
        }

        public TCollection Serialize<TCollection>(NodeMap nodeMap) where TCollection : NameValueCollection, new() {
            var collection = new TCollection();
            foreach(var childNode in nodeMap.Children()) {
                var name = childNode.Name;
                SerializeValue(name, childNode, collection);
            }
            return collection;
        }

        public abstract void DeserializeValue(string name, string[] values, NodeMap nodeMap);
        public abstract void SerializeValue(string name, Node valueNode, NameValueCollection collection);
    }

    public class PrimitiveParameterParser : ParameterParserBase {
        public static IParameterParser Instance { get; } = new PrimitiveParameterParser();
        private PrimitiveParameterParser() { }

        public override void DeserializeValue(string name, string[] values, NodeMap nodeMap) {
            if(values == null)
                return;

            switch(values.Length) {
                case 0:
                    nodeMap.CreateValue(name);
                    break;
                case 1:
                    nodeMap.CreateValue(name, values[0]);
                    break;
                default:
                    var list = nodeMap.CreateList(name);
                    foreach(var value in values)
                        list.AddChild(new NodeValue(value));
                    break;
            }
        }

        public override void SerializeValue(string name, Node valueNode, NameValueCollection collection) {
            switch (valueNode.Type) {
                case NodeTypes.Empty:
                case NodeTypes.Value:
                    collection.Add(name, ((NodeValue)valueNode).GetValue<string>());
                    break;
                case NodeTypes.Map:
                    foreach(var childNode in ((NodeList)valueNode).Children())
                        collection.Add(name, ((NodeValue)childNode).GetValue<string>());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
