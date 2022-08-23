using System;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Core.Utils.Factories; 

/// <inheritdoc />
public class DynamicArgsFactory : IArgsFactory
{
    private readonly DynamicCtor _ctor;
    /// <summary>
    /// Create a new instance of <see cref="DynamicArgsFactory"/>
    /// </summary>
    public DynamicArgsFactory(DynamicCtor ctor) {
        _ctor = ctor;
    }

    /// <inheritdoc />
    public object GetInstance(params object[] args)
    {
        return _ctor.Invoke(args);
    }
}
    
/// <inheritdoc />
public class DynamicArgsFactory<T> : IArgsFactory<T> {
    private readonly DynamicCtor _ctor;
    /// <summary>
    /// Create a new instance of <see cref="DynamicArgsFactory"/>
    /// </summary>
    public DynamicArgsFactory(DynamicCtor ctor) {
        _ctor = ctor;

        if(ctor.ReflectedType.IsAssignableTo<T>() == false)
            throw new ArgumentException($"Type {typeof(T).Name} is not assignable to generic type {ctor.ReflectedType.Name}");
    }

    /// <inheritdoc />
    object IArgsFactory.GetInstance(params object[] args) => GetInstance(args);

    /// <inheritdoc />
    public T GetInstance(params object[] args)
    {
        return (T)_ctor.Invoke(args);
    }
}