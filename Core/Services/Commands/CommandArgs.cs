using System.Collections.Generic;

namespace DotLogix.Core.Services.Commands; 

/// <summary>
///     A helper struct to hold command arguments
/// </summary>
public struct CommandArgs {
    /// <summary>
    ///     No arguments
    /// </summary>
    public static CommandArgs Empty { get; } = new(null, null);

    /// <summary>
    ///     The amount of provided arguments
    /// </summary>
    public int Count => (Named?.Count ?? 0) + (Unnamed?.Count ?? 0);

    /// <summary>
    ///     The named arguments
    /// </summary>
    public IReadOnlyDictionary<string, string> Named { get; }

    /// <summary>
    ///     The unnamed arguments
    /// </summary>
    public IList<string> Unnamed { get; }

    /// <summary>
    ///     Creates a new instance of <see cref="CommandArgs" />
    /// </summary>
    public CommandArgs(IReadOnlyDictionary<string, string> named, IList<string> unnamed) {
        Named = named;
        Unnamed = unnamed;
    }

    /// <summary>
    ///     Destruct struct
    /// </summary>
    public void Deconstruct(out IReadOnlyDictionary<string, string> named, out IList<string> unnamed) {
        named = Named;
        unnamed = Unnamed;
    }
}