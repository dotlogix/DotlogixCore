namespace DotLogix.Architecture.Infrastructure.Decorators {
    public class InsertOnlyAttribute : EntitySetDecoratorAttribute
    {
        public InsertOnlyAttribute(int priority = int.MaxValue) : base(typeof(InsertOnlyEntitySetDecorator<>), priority) { }
    }
}