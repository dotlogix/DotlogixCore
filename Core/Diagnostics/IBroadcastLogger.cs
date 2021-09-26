using System.Collections.Generic;

namespace DotLogix.Core.Diagnostics {
    /// <summary>
    /// An interface to broadcast log messages to multiple targets
    /// </summary>
    public interface IBroadcastLogger : ILogger {
        /// <summary>
        /// Attach loggers to the hub
        /// </summary>
        bool AttachLogger(IEnumerable<ILogger> loggers);

        /// <summary>
        /// Detach loggers to the hub
        /// </summary>
        bool DetachLogger(IEnumerable<ILogger> loggers);
    }
}