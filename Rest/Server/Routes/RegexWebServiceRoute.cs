// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RegexWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  07.02.2018
// ==================================================

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Server.Http.Parameters;
using DotLogix.Core.Rest.Services.Processors;
#endregion

namespace DotLogix.Core.Rest.Server.Routes {
    public class RegexWebServiceRoute : WebServiceRouteBase {
        public Regex Regex { get; }

        public RegexWebServiceRoute(string pattern, HttpMethods acceptedRequests, IWebRequestProcessor requestProcessor,
                                    int priority) : base(pattern, acceptedRequests, requestProcessor, priority) {
            Regex = new Regex(Pattern);
        }


        public override RouteMatch Match(HttpMethods method, string path) {
            if((AcceptedRequests & method) == 0)
                return RouteMatch.Empty;
            var names = Regex.GetGroupNames();
            var match = Regex.Match(path);
            if(!match.Success)
                return RouteMatch.Empty;
            var parameters = names.Select(n => new Parameter(n, match.Groups[n].Value)).ToList();
            return new RouteMatch(true, match.Value, match.Length, parameters);
        }
    }

    public class PatternWebServiceRoute : RegexWebServiceRoute {
        private static Dictionary<string, string> _regexPattern=new Dictionary<string, string>() {
            {"any",".+?" },
            {"number","\\d+" },
            {"n","\\d+" },
            {"letter","\\w+" },
            {"l","\\w+" },
            {"guid","[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}" },
            {"g","[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}" }
                                                                                                 };



        public PatternWebServiceRoute(string pattern, HttpMethods acceptedRequests, IWebRequestProcessor requestProcessor, int priority) : base(ConvertToRegex(pattern), acceptedRequests, requestProcessor, priority) { }

        private static string ConvertToRegex(string pattern) {
            var builder = new StringBuilder();

            var i = 0;
            var patternStart = 0;

            while((patternStart = pattern.IndexOf("<<", i)) != -1) {
                if(patternStart > i)
                    builder.Append(pattern, i, patternStart - i);

                var patternEnd = pattern.IndexOf(">>", patternStart);
                if(patternEnd == -1) {
                    break;
                }

                var parse = pattern.Substring(patternStart+2, (patternEnd - patternStart) -2);
                parse = Parse(parse);
                builder.Append(parse);
                i = patternEnd+2;
            }
            if(i == 0)
                return pattern;
            if (i < pattern.Length)
                builder.Append(pattern, i, pattern.Length - i);
            return builder.ToString();
        }

        private static string Parse(string s) {
            var parts = s.Split(':');
            var name = parts[0];
            var type = parts.Length > 1 ? parts[1] : "any";

            if (_regexPattern.TryGetValue(type, out var pattern) == false)
                throw new ArgumentException("Pattern with type " + type + " is currently not supported");

            return $"(?<{name}>{pattern})";
        }
    }
}
