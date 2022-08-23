using System.Threading.Tasks;

namespace DotLogix.Core.Services.Commands {
    /// <summary>
    ///     A basic console command
    /// </summary>
    public abstract class ConsoleCommand : IConsoleCommand {
        private string _description;
        private string _helpText;

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string Description => _description ??= CreateDescription();

        /// <inheritdoc />
        public string HelpText => _helpText ??= CreateHelpText();


        /// <summary>
        ///     Creates a new instance of <see cref="ConsoleCommand" />
        /// </summary>
        protected ConsoleCommand(string name) {
            Name = name;
        }

        /// <inheritdoc />
        public abstract Task ExecuteAsync(CommandContext context);

        protected abstract string CreateHelpText();
        protected abstract string CreateDescription();
    }
}
