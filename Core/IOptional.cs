namespace DotLogix.Core {
    public interface IOptional {
        bool IsDefined { get; }
        bool IsDefault { get; }
        bool IsUndefinedOrDefault { get; }
        object Value { get; }

        object GetValueOrDefault(object defaultValue = default);
        bool TryGetValue(out object defaultValue);
    }

    public interface IOptional<TValue> : IOptional{
        new TValue Value { get; }

        TValue GetValueOrDefault(TValue defaultValue = default);
        bool TryGetValue(out TValue defaultValue);
    }
}