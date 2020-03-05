using System;

namespace DotLogix.Core.Services {
    /// <inheritdoc />
    public class LambdaConsoleCommand : ConsoleCommand {
        /// <summary>
        /// A callback to execute an action
        /// </summary>
        private readonly Func<ConsoleCommand, CommandArgs, ConsoleCommandResult> _callbackFunc;
        /// <summary>
        /// A callback to validate arguments
        /// </summary>
        private readonly Func<ConsoleCommand, CommandArgs, bool> _validateArguments;

        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, Func<ConsoleCommand, CommandArgs, ConsoleCommandResult> callbackFunc, Func<ConsoleCommand, CommandArgs, bool> validationFunc=null, string helpText = null) : base(name, description, helpText) {
            _callbackFunc = callbackFunc;
            _validateArguments = validationFunc;
        }
        
        /// <summary>
        ///   Creates a new instance of <see cref="LambdaConsoleCommand"/>
        /// </summary>
        public LambdaConsoleCommand(string name, string description, Action<ConsoleCommand, CommandArgs> callbackFunc, Func<ConsoleCommand, CommandArgs, bool> validationFunc=null, string helpText = null) : base(name, description, helpText) {
            ConsoleCommandResult ActionToFunc(ConsoleCommand command, CommandArgs args) {
                callbackFunc?.Invoke(command, args);
                return ConsoleCommandResult.Continue;
            }

            _callbackFunc = ActionToFunc;
        }

        /// <inheritdoc />
        public override ConsoleCommandResult OnExecute(CommandArgs args) {
            return _callbackFunc?.Invoke(this, args) ?? ConsoleCommandResult.Continue;
        }

        /// <inheritdoc />
        public override bool OnValidate(CommandArgs args) {
            return _validateArguments == null || _validateArguments.Invoke(this, args);
        }
    }
}