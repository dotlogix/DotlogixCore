using System.Collections.Generic;

namespace DotLogix.Core.Services.Commands {
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
}