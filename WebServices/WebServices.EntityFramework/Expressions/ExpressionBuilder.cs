using System;
using System.Linq.Expressions;

namespace DotLogix.WebServices.EntityFramework.Expressions {
    public class ExpressionBuilder {
        public Type Type { get; }
        public Expression Body { get; }

        public ExpressionBuilder(Type type, Expression expression) {
            Type = type ?? expression.Type;
            Body = expression;
            
            if(expression.Type != Type) {
                throw new ArgumentException($"Expected an expression of type {Type}", nameof(expression));
            }
        }
        
        public ExpressionBuilder(Expression expression) {
            Type = expression.Type;
            Body = expression;
        }
    }
    
    public class ExpressionBuilder<T> : ExpressionBuilder {
        public ExpressionBuilder(T value) : base(typeof(T), Expression.Constant(value, typeof(T))) { }
        public ExpressionBuilder(Expression expression) : base(typeof(T), expression) { }
        

        public static implicit operator ExpressionBuilder<T>(T value) {
            return new ExpressionBuilder<T>(Expression.Constant(value, typeof(T)));
        }
        
        public static implicit operator ExpressionBuilder<T>(Expression value) {
            return new ExpressionBuilder<T>(value);
        }
        
        public static implicit operator Expression(ExpressionBuilder<T> value) {
            return value.Body;
        }
    }
}