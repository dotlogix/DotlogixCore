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
        /// Checks if there is no value available
        /// </summary>
        bool IsUndefined { get; }

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
        /// Get the value or throw if undefined
        /// </summary>
        object GetValue();
        
        /// <summary>
        /// Get the value or a default value if undefined
        /// </summary>
        object GetValueOrDefault(object defaultValue = default);

        /// <summary>
        /// Tries to get the value
        /// </summary>
        bool TryGetValue(out object defaultValue);
        
        /// <summary>
        /// Throws if the value is undefined
        /// </summary>
        void ThrowIfUndefined();
        
        /// <summary>
        /// Throws if the value is undefined or default
        /// </summary>
        void ThrowIfUndefinedOrDefault();
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
        /// Get the value or throw if undefined
        /// </summary>
        new TValue GetValue();
        
        /// <summary>
        /// Tries to get the value
        /// </summary>
        bool TryGetValue(out TValue defaultValue);
    }
}