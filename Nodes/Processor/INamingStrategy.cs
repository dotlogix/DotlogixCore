using System.Text;

namespace DotLogix.Core.Nodes.Processor {
    /// <summary>
    /// An interface to represent naming strategies
    /// </summary>
    public interface INamingStrategy {
        /// <summary>
        /// Transform the name according to the naming strategy
        /// </summary>
        string TransformName(string name);
        /// <summary>
        /// Append the name according to the naming strategy to a string builder
        /// </summary>
        void AppendName(string name, StringBuilder builder);
    }
}