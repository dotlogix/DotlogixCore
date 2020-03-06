namespace DotLogix.Core.Services {
    /// <summary>
    /// A helper struct to provide command results 
    /// </summary>
    public struct ConsoleCommandResult {
        /// <summary>
        /// Exit application without error
        /// </summary>
        public static ConsoleCommandResult ExitNoError { get; } = new ConsoleCommandResult(true);
        /// <summary>
        /// Continue application loop
        /// </summary>
        public static ConsoleCommandResult Continue { get; } = new ConsoleCommandResult(false);

        /// <summary>
        /// Determines if the application should exit
        /// </summary>
        public bool Exit { get; }
        /// <summary>
        /// The exit code
        /// </summary>
        public int ExitCode { get; }

        /// <summary>
        ///   Creates a new instance of <see cref="ConsoleCommandResult"/>
        /// </summary>
        public ConsoleCommandResult(bool exit, int exitCode = 0) {
            Exit = exit;
            ExitCode = exitCode;
        }

        /// <summary>
        /// Destruct this command result
        /// </summary>
        public void Deconstruct(out bool exit, out int exitCode) {
            exit = Exit;
            exitCode = ExitCode;
        }
    }
}