using System;
using System.Collections.Concurrent;
using System.Text;
using DotLogix.Core.Nodes.Processor;

namespace DotLogix.Core.Nodes {
    /// <summary>
    /// A base class for naming strategies
    /// </summary>
    public abstract class NamingStrategyBase : INamingStrategy {
        private const int MaxBuilderPoolSize = 20;
        private static readonly ConcurrentQueue<StringBuilder> StringBuilderPool = new ConcurrentQueue<StringBuilder>();

        /// <inheritdoc />
        public string TransformName(string name) {
            if (StringBuilderPool.TryDequeue(out var builder) == false)
                builder = new StringBuilder(Math.Max(name.Length, 50));
            
            if (TransformIfRequired(name, builder) == false)
                return name;
            name = StringBuilderPool.ToString();
            builder.Clear();
            if(StringBuilderPool.Count < MaxBuilderPoolSize)
                StringBuilderPool.Enqueue(builder);
            return name;
        }

        /// <inheritdoc />
        public void AppendName(string name, StringBuilder builder) {
            if(TransformIfRequired(name, builder) == false)
                builder.Append(name);
        }

        /// <summary>
        /// Transform the name according to the naming strategy and append it to the string builder
        /// </summary>
        /// <returns>true if the name was transformed, otherwise false</returns>
        protected abstract bool TransformIfRequired(string name, StringBuilder builder);
    }
}