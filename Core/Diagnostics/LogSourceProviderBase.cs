namespace DotLogix.Core.Diagnostics {
    public abstract class LogSourceProviderBase : ILogSourceProvider {
        public ILogger Logger { get; }

        protected LogSourceProviderBase(ILogger logger) {
            Logger = logger;
        }
        
        public virtual ILogSource Create(string name, int skipFrames = 2) {
            return new LogSource(name, Logger, GetLogLevel);
        }

        protected virtual LogLevels GetLogLevel(string name) {
            return Log.LogLevel;
        }
    }
}
