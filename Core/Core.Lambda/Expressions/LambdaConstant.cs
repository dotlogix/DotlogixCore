using System;
using System.Linq.Expressions;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Expressions; 

/// <summary>
///     An implementation of the <see cref="Lambda" /> interface
/// </summary>
public class Lambda {
    /// <inheritdoc />
    public Expression Body { get; }

    /// <inheritdoc />
    public Type Type { get; }

    /// <summary>
    /// Creates a new instance of <see cref="Lambda"/>
    /// </summary>
    protected Lambda(Expression body) : this(body, body.Type) {
    }

    /// <summary>
    /// Creates a new instance of <see cref="Lambda"/>
    /// </summary>
    protected Lambda(Expression body, Type type) {
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Body = body ?? throw new ArgumentNullException(nameof(body));

        if(type.IsAssignableFrom(body.Type) == false) {
            throw new ArgumentException($"Type of expression {body.Type.GetFriendlyGenericName()} is not assignable to lambda type {type.GetFriendlyGenericName()}");
        }
    }

    public static implicit operator Expression(Lambda lambda) => lambda.Body;
    public static implicit operator Lambda(Expression expression) => new(expression);
}
    
/// <summary>
///     An implementation of the <see cref="Lambda{TValue}" /> interface
/// </summary>
public class Lambda<TValue> : Lambda {
    /// <summary>
    /// Creates a new instance of <see cref="Lambda{TValue}"/>
    /// </summary>
    protected Lambda(Expression body) : base(body, typeof(TValue)) {
    }
        
    public static implicit operator Lambda<TValue>(Expression expression) => new(expression);
    public static implicit operator Lambda<TValue>(TValue value) => new(Expression.Constant(value, typeof(TValue)));
}