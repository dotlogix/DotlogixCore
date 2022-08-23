using System;
using System.Threading.Tasks;

namespace DotLogix.Core.Services.Commands {
    public class CommandBuilder : ICommandBuilder {
        private Func<CommandContext, Task> _callback;
        private string _description;
        private string _helpText;
        private string _name;

        public CommandBuilder UseName(string name) {
            _name = name;
            return this;
        }

        public CommandBuilder UseDescription(string description) {
            _description = description;
            return this;
        }

        public CommandBuilder UseHelpText(string helpText) {
            _helpText = helpText;
            return this;
        }

        public CommandBuilder UseCallback(Func<CommandContext, Task> callback) {
            _callback = callback;
            return this;
        }

        public CommandBuilder UseCallback(Action<CommandContext> callback) {
            return UseCallback(ctx => {
                callback(ctx);
                return Task.CompletedTask;
            });
        }

        public IConsoleCommand Build() {
            if(_name == null) throw new InvalidOperationException("A name is required to build this command");
            return new LambdaConsoleCommand(_name, _description, _helpText, _callback);
        }
    }
}
