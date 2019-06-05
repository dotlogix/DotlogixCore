namespace DotLogix.Core {
    /// <summary>
    /// An interface to define optional value types
    /// </summary>
    public interface IOptional {
        /// <summary>
        /// Checks if there is a value available
        /// </summary>
        bool IsDefined { get; }
        /// <summary>
        /// Checks if the value is equal to the types default value
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// Checks if the value undefined or is equal to the types default value
        /// </summary>
        bool IsUndefinedOrDefault { get; }
        /// <summary>
        /// Checks if the value is equal to the types default value
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Get the value or a default value if undefined
        /// </summary>
        object GetValueOrDefault(object defaultValue = default);

        /// <summary>
        /// Tries to get the value
        /// </summary>
        bool TryGetValue(out object defaultValue);
    }

    /// <inheritdoc />
    public interface IOptional<TValue> : IOptional{
        /// <summary>
        /// The value
        /// </summary>
        new TValue Value { get; }

        /// <summary>
        /// Get the value or a default value if undefined
        /// </summary>
        TValue GetValueOrDefault(TValue defaultValue = default);

        /// <summary>
        /// Tries to get the value
        /// </summary>
        bool TryGetValue(out TValue defaultValue);
    }
}