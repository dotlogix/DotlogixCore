using System;
using System.Threading.Tasks;

namespace DotLogix.Core.Services.Commands {
    /// <inheritdoc />
    public class LambdaConsoleCommand : IConsoleCommand {
        private readonly Func<CommandContext, Task> _callbackFunc;

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string Description { get; }

        /// <inheritdoc />
        public string HelpText { get; }

        /// <summary>
        ///     Creates a new instance of <see cref="LambdaConsoleCommand" />
        /// </summary>
        public LambdaConsoleCommand(string name, string description, string helpText, Func<CommandContext, Task> callbackFunc) {
            Name = name;
            Description = description;
            HelpText = helpText;
            _callbackFunc = callbackFunc;
        }

        /// <inheritdoc />
        public Task ExecuteAsync(CommandContext context) {
            return _callbackFunc?.Invoke(context) ?? Task.CompletedTask;
        }
    }
}
