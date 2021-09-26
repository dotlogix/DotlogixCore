using DotLogix.Core.Diagnostics;

namespace DotLogix.WebServices.Core.Diagnostics {
    public class LoggerAdapterProvider : Microsoft.Extensions.Logging.ILoggerProvider {
        public ILogSourceProvider LogSourceProvider { get; }
        
        public LoggerAdapterProvider(ILogSourceProvider logSourceProvider) {
            LogSourceProvider = logSourceProvider;
        }
        
        
        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName) {
            var logSource = LogSourceProvider.Create(categoryName);
            return new LoggerAdapter(logSource);
        }

        public void Dispose() {
            
        }
    }
}