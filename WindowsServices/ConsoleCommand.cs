using System;

namespace DotLogix.Core.WindowsServices {
    public class ConsoleCommand {
        public Func<ConsoleCommand, string[], int?> Callback { get; }
        public Func<ConsoleCommand, string[], bool> ValidateArguments { get; }

        public string Name { get; }
        public string Description { get; }
        public string HelpText { get; }

        /// <summary>
        ///   Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public ConsoleCommand(string name, string description, Func<ConsoleCommand, string[], int?> callbackFunc, Func<ConsoleCommand, string[], bool> validateArguments = null, string helpText = null) {
            Callback = callbackFunc;
            ValidateArguments = validateArguments;
            Description = description;
            HelpText = helpText;
            Name = name;
        }

        public int? Execute(string[] args) {
            return Callback.Invoke(this, args);
        } 
    }
}