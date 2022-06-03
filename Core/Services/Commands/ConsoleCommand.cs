using System;
using System.Threading.Tasks;

namespace DotLogix.Core.Services.Commands {
    /// <summary>
    /// A basic console command
    /// </summary>
    public abstract class ConsoleCommand : IConsoleCommand {
        private string _helpText;
        private string _description;

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string Description => _description ??= CreateDescription();

        /// <inheritdoc />
        public string HelpText => _helpText ??= CreateHelpText();


        /// <summary>
        ///   Creates a new instance of <see cref="ConsoleCommand"/>
        /// </summary>
        protected ConsoleCommand(string name) {
            Name = name;
        }

        /// <inheritdoc />
        public abstract Task ExecuteAsync(CommandArgs args);
        protected abstract string CreateHelpText();
        protected abstract string CreateDescription();
    }
}