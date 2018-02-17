// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PatternWebServiceRoute.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Text;
using DotLogix.Core.Rest.Server.Http;
using DotLogix.Core.Rest.Services.Processors;
using DotLogix.Core.Utils.Patterns;
#endregion

namespace DotLogix.Core.Rest.Server.Routes {
    public class PatternWebServiceRoute : RegexWebServiceRoute {
        public PatternWebServiceRoute(int routeIndex, string pattern, HttpMethods acceptedRequests, IWebRequestProcessor requestProcessor, int priority) : base(routeIndex, ConvertToRegex(pattern), acceptedRequests, requestProcessor, priority) { }

        private static string ConvertToRegex(string pattern) {
            var builder = new StringBuilder();

            var i = 0;
            int patternStart;

            while((patternStart = pattern.IndexOf("<<", i, StringComparison.Ordinal)) != -1) {
                if(patternStart > i)
                    builder.Append(pattern, i, patternStart - i);

                var patternEnd = pattern.IndexOf(">>", patternStart, StringComparison.Ordinal);
                if(patternEnd == -1)
                    break;

                var parse = pattern.Substring(patternStart + 2, patternEnd - patternStart - 2);
                parse = Parse(parse);
                builder.Append(parse);
                i = patternEnd + 2;
            }
            if(i == 0)
                return pattern;
            if(i < pattern.Length)
                builder.Append(pattern, i, pattern.Length - i);
            return builder.ToString();
        }

        private static string Parse(string s) {
            var parts = s.Split(new[] {'|'}, 3);
            var name = parts[0];
            string typeStr;
            string variantStr;
            if((parts.Length > 1) && (string.IsNullOrEmpty(parts[1]) == false)) {
                var typeSplit = parts[1].Split(new[] {':'}, 2);
                typeStr = typeSplit[0];
                variantStr = typeSplit.Length > 1 ? typeSplit[1] : null;
            } else {
                typeStr = "any";
                variantStr = null;
            }

            var args = parts.Length > 2 ? parts[2].Split('|') : new string[0];

            if(PatternParser.Default.TryGetPattern(typeStr, out var patternType) == false)
                throw new ArgumentException("Pattern with type " + typeStr + " is not registered in default pattern parser");

            var pattern = patternType.GetRegexPattern(variantStr, args);
            return pattern == null ? s : $"(?<{name}>{pattern})";
        }
    }
}
