using System;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Diagnostics {
    public abstract class LogSourceProviderBase : ILogSourceProvider {
        public ILogger Logger { get; }

        protected LogSourceProviderBase(ILogger logger) {
            Logger = logger;
        }
        
        public virtual ILogSource Create(string name, int skipFrames = 2) {
            return new LogSource(name, Logger, skipFrames, GetLogLevel);
        }
        
        public virtual ILogSource Create(Type type, int skipFrames = 2) {
            var name = string.Join(".", type.Namespace, type.GetFriendlyGenericName());
            return new LogSource(name, Logger, skipFrames, GetLogLevel);
        }

        protected abstract LogLevels GetLogLevel(ILogSource logSource);
    }
}