namespace DotLogix.Core.Services {
    /// <summary>
    /// A helper struct to provide command results 
    /// </summary>
    public struct ConsoleCommandResult {
        /// <summary>
        /// Exit application without error
        /// </summary>
        public static ConsoleCommandResult ExitNoError { get; } = new ConsoleCommandResult { Exit = true, Success = true};

        /// <summary>
        /// Continue execution completed
        /// </summary>
        public static ConsoleCommandResult CommandCompleted { get; } = new ConsoleCommandResult { Success = true };

        /// <summary>
        /// Command execution failed
        /// </summary>
        public static ConsoleCommandResult CommandFailed { get; } = new ConsoleCommandResult { Success = false };

        /// <summary>
        /// Determines if the command executed successfully
        /// </summary>
        public bool Success;
        /// <summary>
        /// Determines if the application should exit
        /// </summary>
        public bool Exit;
        /// <summary>
        /// The exit code
        /// </summary>
        public int ExitCode;

        public void Deconstruct(out bool success, out bool exit, out int exitCode) {
            success = Success;
            exit = Exit;
            exitCode = ExitCode;
        }
    }
}