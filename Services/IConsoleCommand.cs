namespace DotLogix.Core.Services {
    /// <summary>
    /// An interface representing a console command
    /// </summary>
    public interface IConsoleCommand {
        /// <summary>
        /// The name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The description
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The help text
        /// </summary>
        string HelpText { get; }
        /// <summary>
        /// A callback to execute an action
        /// </summary>
        ConsoleCommandResult OnExecute(CommandArgs args);
    }
}