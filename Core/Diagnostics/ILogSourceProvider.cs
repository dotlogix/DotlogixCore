namespace DotLogix.Core.Diagnostics {
    public interface ILogSourceProvider {
        /// <summary>
        /// The target logger
        /// </summary>
        ILogger Logger { get; }
        
        /// <summary>
        /// Creates a log source 
        /// </summary>
        ILogSource Create(string name, int skipFrames = 2);
    }
}