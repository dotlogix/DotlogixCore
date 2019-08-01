using System;
using System.Collections.Generic;

namespace DotLogix.Core.WindowsServices {
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

        /// <summary>
        /// A callback to validate arguments
        /// </summary>
        bool OnValidate(CommandArgs args);
    }


    /// <summary>
    /// A helper struct to hold command arguments
    /// </summary>
    public struct CommandArgs {
        /// <summary>
        /// No arguments
        /// </summary>
        public static CommandArgs Empty { get; } = new CommandArgs(null, null);

        /// <summary>
        /// Creates a new instance of <see cref="CommandArgs"/>
        /// </summary>
        public CommandArgs(IDictionary<string, string> named, IList<string> unnamed) {
            Named = named;
            Unnamed = unnamed;
        }

        /// <summary>
        /// Check if there are parameters
        /// </summary>
        public bool IsEmpty => (Named == null || Named.Count == 0) && (Unnamed == null || Unnamed.Count == 0);

        /// <summary>
        /// The named arguments
        /// </summary>
        public IDictionary<string, string> Named { get; }
        /// <summary>
        /// The unnamed arguments
        /// </summary>
        public IList<string> Unnamed { get; }

        
        /// <summary>
        /// Destruct struct
        /// </summary>
        public void Deconstruct(out IDictionary<string, string> named, out IList<string> unnamed) {
            named = Named;
            unnamed = Unnamed;
        }
    }

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