using DotLogix.Architecture.Infrastructure.Decorators;

namespace DotLogix.Architecture.Infrastructure.Attributes {
    /// <summary>
    /// An <see cref="EntitySetDecoratorAttribute"/> to apply an <see cref="InsertOnlyEntitySetDecorator{TEntity}"/> to applied class
    /// </summary>
    public class InsertOnlyAttribute : EntitySetDecoratorAttribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="InsertOnlyAttribute"/>
        /// </summary>
        public InsertOnlyAttribute(int priority = int.MaxValue) : base(typeof(InsertOnlyEntitySetDecorator<>), priority) { }
    }
}