using System;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Factories; 

/// <inheritdoc />
public class SingletonFactory : IFactory {
    private readonly DynamicProperty _property;
    /// <summary>
    /// Create a new instance of <see cref="SingletonFactory"/>
    /// </summary>
    public SingletonFactory(DynamicProperty property) {
        _property = property ?? throw new ArgumentNullException(nameof(property));
    }

    /// <inheritdoc />
    public object Create() {
        return _property.GetValue();
    }
}
    
/// <inheritdoc />
public class SingletonFactory<T> : IFactory<T> {
    private readonly DynamicProperty _property;
    /// <summary>
    /// Create a new instance of <see cref="SingletonFactory"/>
    /// </summary>
    public SingletonFactory(DynamicProperty property) {
        _property = property ?? throw new ArgumentNullException(nameof(property));
    }

    /// <inheritdoc />
    object IFactory.Create() => Create();

    /// <inheritdoc />
    public T Create() {
        return (T)_property.GetValue();
    }
}