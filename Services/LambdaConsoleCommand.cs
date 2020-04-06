using System;

namespace DotLogix.Core.Services {
    /// <inheritdoc />
    public class LambdaConsoleCommand : ConsoleCommand {
        /// <summary>
        /// A callback to execute an action
        /// </summary>
        private readonly Func<ConsoleCommand, CommandArgs, ConsoleCommandResult> _callbackFunc;

        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, Func<ConsoleCommand, CommandArgs, ConsoleCommandResult> callbackFunc, string helpText = null) : base(name, description, helpText) {
            _callbackFunc = callbackFunc;
        }

        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, Func<bool> callbackFunc, string helpText = null) : base(name, description, helpText) {
            ConsoleCommandResult ConvertDelegate(ConsoleCommand c, CommandArgs args) {
                return new ConsoleCommandResult { Success = callbackFunc?.Invoke() ?? false };
            }
            _callbackFunc = ConvertDelegate;
        }

        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, Func<ConsoleCommandResult> callbackFunc, string helpText = null) : base(name, description, helpText) {
            ConsoleCommandResult ConvertDelegate(ConsoleCommand c, CommandArgs args) {
                return callbackFunc?.Invoke() ?? ConsoleCommandResult.CommandFailed;
            }
            _callbackFunc = ConvertDelegate;
        }

        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, Action callbackFunc, string helpText = null) : base(name, description, helpText) {
            ConsoleCommandResult ConvertDelegate(ConsoleCommand c, CommandArgs args) {
                callbackFunc?.Invoke();
                return ConsoleCommandResult.CommandCompleted;
            }

            _callbackFunc = ConvertDelegate;
        }

        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, Action<ConsoleCommand, CommandArgs> callbackFunc, string helpText = null) : base(name, description, helpText) {
            ConsoleCommandResult ConvertDelegate(ConsoleCommand command, CommandArgs args) {
                callbackFunc?.Invoke(command, args);
                return ConsoleCommandResult.CommandCompleted;
            }

            _callbackFunc = ConvertDelegate;
        }

        /// <inheritdoc />
        public override ConsoleCommandResult OnExecute(CommandArgs args) {
            return _callbackFunc?.Invoke(this, args) ?? ConsoleCommandResult.CommandCompleted;
        }
    }
}