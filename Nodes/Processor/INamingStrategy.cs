using System.Text;

namespace DotLogix.Core.Nodes.Processor {
    public interface INamingStrategy {
        string TransformName(string name);
        void AppendName(string name, StringBuilder builder);
    }
}