using System;

namespace DotLogix.Core.Utils.Factories; 

/// <summary>
/// An attribute to create a new instance of <see cref="IArgsFactory"/>
/// </summary>
public class ArgsFactoryAttribute : Attribute
{
    private readonly IArgsFactory _factory;

    /// <summary>
    /// Creates a new instance of <see cref="ArgsFactoryAttribute"/> using an existing <see cref="IFactory"/>
    /// </summary>
    /// <param name="factory">The instance</param>
    public ArgsFactoryAttribute(IArgsFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <summary>
    /// Creates a new instance of <see cref="FactoryAttribute"/> using a constructor with the specified type arguments
    /// </summary>
    /// <param name="type">The type</param>
    /// <param name="parameterTypes">The parameter types used by the constructor</param>
    /// <param name="constraintType">An optional constraint type to ensure type compatibility</param>
    protected ArgsFactoryAttribute(Type type, Type[] parameterTypes, Type constraintType = null) : this(Factories.UseCtor(type, parameterTypes, constraintType)) { }

    /// <summary>
    /// Creates a new instance of <see cref="FactoryAttribute"/> using a delegate method
    /// </summary>
    /// <param name="instantiateFunc">The factory method</param>
    protected ArgsFactoryAttribute(Func<object[], object> instantiateFunc) : this(Factories.UseDelegate(instantiateFunc)) { }

    /// <summary>
    /// Get or create a new instance using the configured method
    /// </summary>
    /// <param name="args">The arguments to create a new instance</param>
    /// <returns></returns>
    protected object GetInstance(params object[] args)
    {
        return _factory.GetInstance(args);
    }

    /// <summary>
    /// Get or create a new instance as a specific type using the configured method
    /// </summary>
    /// <param name="args">The arguments to create a new instance</param>
    /// <typeparam name="TInstance">The target type</typeparam>
    /// <returns>The instance if the provided type is compatible, otherwise <value>default</value></returns>
    protected TInstance GetInstance<TInstance>(params object[] args)
    {
        return _factory.GetInstance(args) is TInstance instance ? instance : default;
    }
}