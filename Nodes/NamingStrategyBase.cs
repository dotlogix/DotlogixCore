using System.Text;
using DotLogix.Core.Nodes.Processor;

namespace DotLogix.Core.Nodes {
    public abstract class NamingStrategyBase : INamingStrategy {
        private readonly StringBuilder _builder = new StringBuilder(50);

        public string TransformName(string name) {
            if(TransformIfRequired(name, _builder)) {
                name = _builder.ToString();
                _builder.Clear();
            }
            return name;
        }
        public void AppendName(string name, StringBuilder builder) {
            if(TransformIfRequired(name, builder) == false)
                builder.Append(name);
        }

        protected abstract bool TransformIfRequired(string name, StringBuilder builder);
    }
}