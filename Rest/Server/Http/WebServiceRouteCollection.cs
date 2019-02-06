// ==================================================
// Copyright 2018(C) , DotLogix
// File:  WebServiceRouteCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  05.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Collections;
using DotLogix.Core.Rest.Server.Routes;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public struct PrefixedRoute {
        public string Prefix { get; }
        public IWebServiceRoute Route { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public PrefixedRoute(string prefix, IWebServiceRoute route) {
            Prefix = prefix;
            Route = route;
        }
    }

    public class WebServiceRouteCollection : ICollection<PrefixedRoute>, IReadOnlyCollection<PrefixedRoute> {
        private readonly IDictionary<string, SortedCollection<IWebServiceRoute>> _prefixedRoutes = new Dictionary<string, SortedCollection<IWebServiceRoute>>();


        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<PrefixedRoute> GetEnumerator() {
            return _prefixedRoutes.SelectMany(r => r.Value.Select(v => new PrefixedRoute(r.Key, v))).GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>Adds a route to the collection</summary>
        /// <param name="route">The route to add</param>
        void ICollection<PrefixedRoute>.Add(PrefixedRoute route) {
            Add(route.Prefix, route.Route);
        }
        
        /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
        /// <param name="prefix">The prefix</param>
        /// <param name="route">The route to add</param>
        public void Add(string prefix, IWebServiceRoute route) {
            Count++;
            if(_prefixedRoutes.TryGetValue(prefix, out var list) ==false)
                _prefixedRoutes.Add(prefix,(list = new SortedCollection<IWebServiceRoute>(WebServiceRouteComparer.Instance)));
            list.Add(route);
        }

        /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public void Clear() {
            Count=0;
            _prefixedRoutes.Clear();
        }

        /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.</summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>true if <paramref name="item">item</paramref> is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.</returns>
        bool ICollection<PrefixedRoute>.Contains(PrefixedRoute item) {
            return Contains(item.Prefix, item.Route);
        }


        private bool Contains(string prefix, IWebServiceRoute route) {
            return _prefixedRoutes.TryGetValue(prefix, out var list) && list.Contains(route);
        }

        /// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.</summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array">array</paramref> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex">arrayIndex</paramref> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from <paramref name="arrayIndex">arrayIndex</paramref> to the end of the destination <paramref name="array">array</paramref>.</exception>
        void ICollection<PrefixedRoute>.CopyTo(PrefixedRoute[] array, int arrayIndex) {
            if(array == null)
                throw new ArgumentNullException(nameof(array));
            if(Count + arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException();

            foreach(var prefixedRoute in _prefixedRoutes) {
                foreach(var route in prefixedRoute.Value) {
                    array[arrayIndex++] = new PrefixedRoute(prefixedRoute.Key, route);
                }
            }
        }

        /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>true if <paramref name="item">item</paramref> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if <paramref name="item">item</paramref> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public bool Remove(PrefixedRoute item) {
            if(_prefixedRoutes.TryGetValue(item.Prefix, out var list) == false || list.Remove(item.Route) == false)
                return false;

            Count--;
            return true;

        }

        public RouteMatch FindBestMatch(HttpMethods httpMethod, string path, out IWebServiceRoute route) {
            var bestPrefixLength = int.MinValue;
            RouteMatch bestMatch = null;
            IWebServiceRoute bestRoute = null;

            foreach(var prefixedRoute in _prefixedRoutes) {
                var prefixLength = prefixedRoute.Key.Length;
                if(bestPrefixLength > prefixLength)
                    continue;

                if(path.StartsWith(prefixedRoute.Key) == false)
                    continue;
                var unprefixedPath = path.Substring(prefixLength);
                var overwrite = bestPrefixLength < prefixLength;
                foreach(var webServiceRoute in prefixedRoute.Value) {
                    if((webServiceRoute.AcceptedRequests & httpMethod) == 0)
                        continue;

                    if(overwrite == false && bestRoute != null) {
                        if(bestRoute.Priority > webServiceRoute.Priority)
                            continue;
                        overwrite |= bestRoute.Priority < webServiceRoute.Priority;
                    }

                    var match = webServiceRoute.Match(httpMethod, unprefixedPath);
                    if(match.Success == false)
                        continue;
                    
                    if(overwrite == false && bestMatch != null) {
                        if(bestMatch.Length > match.Length)
                            continue;
                        overwrite |= bestMatch.Length < match.Length;
                    }

                    if(overwrite) {
                        bestPrefixLength = prefixLength;
                        bestMatch = match;
                        bestRoute = webServiceRoute;
                    }
                }
            }

            route = bestRoute;
            return bestMatch;
        }

        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
        public int Count { get; private set; }

        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
        public bool IsReadOnly => false;
    }
}
