#region
using System;
using System.Collections.Generic;
using System.Linq;
using DotLogix.Core.Extensions;
#endregion

namespace DotLogix.Core.Utils {
    public class CommandParser {
        private IDictionary<string, Command> _commands;
        public IEnumerable<Command> Commands => _commands.Values;

        public CommandParser(IEnumerable<Command> commands) {
            _commands = commands.ToDictionary(c => c.Name);
        }

        public CommandParser(params Command[] commands) : this(commands.AsEnumerable()) { }


        public IDictionary<string, object> Parse(string line) {

        }

        public IDictionary<string, object> Parse(string name, IDictionary<string, string> args) {

        }
    }

    public class Command {
        public string Name { get; }
        public string Description { get; }
        public IEnumerable<ICommandOption> Options { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public Command(string name, string description, IEnumerable<ICommandOption> options) {
            Name = name;
            Description = description;
            Options = options;
        }

        public Command(string name, string description, params ICommandOption[] options) : this(name, description, options.AsEnumerable()) { }

        public object ParseValue(IDictionary<string, string> args) {
            return TryParseValue(args, out var parsed) ? parsed : throw new ArgumentException("The args can not be parsed", nameof(args));
        }

        public bool TryParseValue(IDictionary<string, string> args, out IDictionary<string, object> parsed) {
            parsed = new Dictionary<string, object>();

            foreach(var option in Options) {
                if(option.TryParseValue(args.GetValue(option.Name) ?? args.GetValue(option.Abbreviation), out var value))
                    parsed.Add(option.Name, value);
                else
                    return false;
            }
            return true;
        }
    }

    public interface ICommandOption {
        string Name { get; }
        string Abbreviation { get; }
        string Description { get; }
        bool Required { get; }
        Optional<object> Default { get; }

        object ParseValue(string value);
        bool TryParseValue(string value, out object parsed);
    }

    public interface ICommandOption<T> : ICommandOption {
        new Optional<T> Default { get; }
        new T ParseValue(string value);
        bool TryParseValue(string value, out T parsed);
    }


    public class CommandOption<T> : ICommandOption<T> {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public CommandOption(string name, string abbreviation, string description, Optional<T> defaultValue = default) {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            Default = defaultValue;
        }

        public CommandOption(string name, string description, Optional<T> defaultValue = default)
        : this(name, null, description, defaultValue) { }

        public string Name { get; }
        public string Abbreviation { get; }
        public string Description { get; }
        public bool Required => !Default.IsDefined;
        Optional<object> ICommandOption.Default => Default.IsDefined ? new Optional<object>(Default.Value) : Optional<object>.Undefined;

        public virtual bool TryParseValue(string value, out T parsed) {
            if(value != null)
                return value.TryConvertTo(out parsed);
            parsed = Default.Value;
            return Default.IsDefined;
        }

        public bool TryParseValue(string value, out object parsed) {
            if(TryParseValue(value, out T parsedObj)) {
                parsed = parsedObj;
                return true;
            }

            parsed = default;
            return false;
        }

        public Optional<T> Default { get; }


        public T ParseValue(string value) {
            return TryParseValue(value, out T parsed) ? parsed : throw new ArgumentException($"The value {value} can not be parsed to type {typeof(T).Name}", nameof(value));
        }

        object ICommandOption.ParseValue(string value) {
            return ParseValue(value);
        }
    }
}
