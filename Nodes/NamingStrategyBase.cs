using System.Text;
using DotLogix.Core.Nodes.Processor;

namespace DotLogix.Core.Nodes {
    /// <summary>
    /// A base class for naming strategies
    /// </summary>
    public abstract class NamingStrategyBase : INamingStrategy {
        private readonly object _lock = new object();
        private readonly StringBuilder _builder = new StringBuilder(50);

        /// <inheritdoc />
        public string TransformName(string name) {
            lock (_lock)
            {
                if(TransformIfRequired(name, _builder) == false)
                    return name;
                name = _builder.ToString();
                _builder.Clear();
            }
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