using System.Collections.Generic;
using DotLogix.Core.Rest.Services.Parameters;

namespace DotLogix.Core.Rest.Events {
    public class ParameterProviderCollection : List<IParameterProvider> {
        /// <inheritdoc />
        public ParameterProviderCollection() { }

        /// <inheritdoc />
        public ParameterProviderCollection(IEnumerable<IParameterProvider> collection) : base(collection) { }

        /// <inheritdoc />
        public ParameterProviderCollection(int capacity) : base(capacity) { }

        public void InsertBefore(string name, IParameterProvider provider) {
            var index = FindIndex(p => p.Name == name);
            Insert(index, provider);
        }

        public void InsertAfter(string name, IParameterProvider provider) {
            var index = FindIndex(p => p.Name == name);
            Insert(index + 1, provider);
        }

        public void InsertBefore(ParameterSources source, IParameterProvider provider) {
            var index = FindIndex(p => p.Source == source);
            Insert(index, provider);
        }

        public void InsertAfter(ParameterSources source, IParameterProvider provider) {
            var index = FindLastIndex(p => p.Source == source);
            Insert(index + 1, provider);
        }
    }
}