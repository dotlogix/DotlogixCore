using System;
using System.Threading.Tasks;

namespace DotLogix.Core.Services.Commands {
    /// <inheritdoc />
    public class LambdaConsoleCommand : IConsoleCommand {
        /// <summary>
        /// A callback to execute an action
        /// </summary>
        private readonly Func<IConsoleCommand, CommandArgs, Task> _callbackFunc;

        /// <inheritdoc />
        public string Name { get; }
        /// <inheritdoc />
        public string Description { get; }
        /// <inheritdoc />
        public string HelpText { get; }

        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, string helpText, Func<IConsoleCommand, CommandArgs, Task> callbackFunc) {
            Name = name;
            Description = description;
            HelpText = helpText;
            _callbackFunc = callbackFunc;
        }
        
        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, string helpText, Func<CommandArgs, Task> callbackFunc) {
            Name = name;
            Description = description;
            HelpText = helpText;
            _callbackFunc = (_, args) => callbackFunc.Invoke(args);
        }

        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, string helpText, Action<IConsoleCommand, CommandArgs> callback) {
            Name = name;
            Description = description;
            HelpText = helpText;
            _callbackFunc = (command, args) => {
                callback.Invoke(command, args);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, string helpText, Action<CommandArgs> callback) {
            Name = name;
            Description = description;
            HelpText = helpText;
            _callbackFunc = (_, args) => {
                callback.Invoke(args);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, Func<IConsoleCommand, CommandArgs, Task> callbackFunc)
            : this(name, description, null, callbackFunc) {
        }
        
        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, Func<CommandArgs, Task> callbackFunc)
            : this(name, description, null, callbackFunc) {
        }

        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, Action<IConsoleCommand, CommandArgs> callback)
            : this(name, description, null, callback) {
        }

        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, Action<CommandArgs> callback)
            : this(name, description, null, callback) {
        }

        /// <inheritdoc />
        public Task ExecuteAsync(CommandArgs args) {
            return _callbackFunc?.Invoke(this, args);
        }
    }
}