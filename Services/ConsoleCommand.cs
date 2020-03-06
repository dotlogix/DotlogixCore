namespace DotLogix.Core.Services {
    /// <summary>
    /// A basic console command
    /// </summary>
    public abstract class ConsoleCommand : IConsoleCommand {
        /// <inheritdoc />
        public string Name { get; }
        /// <inheritdoc />
        public string Description { get; }
        /// <inheritdoc />
        public string HelpText { get; }

        /// <summary>
        ///   Creates a new instance of <see cref="ConsoleCommand"/>
        /// </summary>
        public ConsoleCommand(string name, string description, string helpText = null) {
            Description = description;
            HelpText = helpText;
            Name = name;
        }

        /// <inheritdoc />
        public abstract ConsoleCommandResult OnExecute(CommandArgs args);
        /// <inheritdoc />
        public abstract bool OnValidate(CommandArgs args);
    }
}