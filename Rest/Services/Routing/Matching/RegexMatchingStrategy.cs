using System.Collections.Generic;
using System.Text.RegularExpressions;
using DotLogix.Core.Rest.Http;

namespace DotLogix.Core.Rest.Services.Routing.Matching {
    public class RegexMatchingStrategy : IRouteMatchingStrategy {
        public RouteMatch Match(HttpMethods method, string path) {
            if ((AcceptedMethods & method) == 0)
                return RouteMatch.Empty;
            var names = Regex.GetGroupNames();
            var match = Regex.Match(path);
            if (!match.Success)
                return RouteMatch.Empty;

            var parameters = new Dictionary<string, object>();
            foreach (var name in names) {
                var group = match.Groups[name];
                if (group.Captures.Count > 1) {
                    var values = new string[group.Captures.Count];
                    for (int i = 0; i < group.Captures.Count; i++) {
                        values[i] = group.Captures[i].Value;
                    }
                    parameters.Add(name, values);
                } else
                    parameters.Add(name, group.Value);
            }

            return new RouteMatch(true, match.Value, match.Length, parameters);
        }

        public RegexMatchingStrategy(string pattern, HttpMethods acceptedRequests, bool isRooted = false) {
            Pattern = pattern;
            Regex = new Regex(pattern, RegexOptions.Compiled);
            AcceptedMethods = acceptedRequests;
            IsRooted = isRooted;
        }

        public Regex Regex { get; }
        public bool IsRooted { get; }
        public string Pattern { get; protected set; }
        public HttpMethods AcceptedMethods { get; }
    }
}