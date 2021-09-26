using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using DotLogix.Core.Extensions;

namespace DotLogix.WebServices.EntityFramework.Expressions {
    public static class StringExpression {
        private static readonly Type Type = typeof(string);
        private static readonly Type[] SingleTypeArray = Type.CreateArray();
        
        private static readonly MethodInfo ToLowerMethod = Type.GetMethod(nameof(string.ToLower), Type.EmptyTypes);
        private static readonly MethodInfo ToUpperMethod = Type.GetMethod(nameof(string.ToUpper), Type.EmptyTypes);
        
        private static readonly MethodInfo StartsWithMethod = Type.GetMethod(nameof(string.StartsWith), SingleTypeArray);
        private static readonly MethodInfo EndsWithMethod = Type.GetMethod(nameof(string.EndsWith), SingleTypeArray);
        private static readonly MethodInfo ContainsMethod = Type.GetMethod(nameof(string.Contains), SingleTypeArray);
        private static readonly MethodInfo IndexOfMethod = Type.GetMethod(nameof(string.IndexOf), SingleTypeArray);
        private static readonly MethodInfo ConcatMethod = Type.GetMethod(nameof(string.Concat), SingleTypeArray);
        private static readonly MethodInfo ConcatParamsMethod = Type.GetMethod(nameof(string.Concat), typeof(string[]).CreateArray());
        private static readonly MethodInfo SubstringMethod = Type.GetMethod(nameof(string.Substring), typeof(int).CreateArray(2));
        private static readonly MethodInfo TrimMethod = Type.GetMethod(nameof(string.Trim), typeof(char).CreateArray());
        private static readonly MethodInfo TrimStartMethod = Type.GetMethod(nameof(string.TrimStart), typeof(char).CreateArray());
        private static readonly MethodInfo TrimEndMethod = Type.GetMethod(nameof(string.TrimEnd), typeof(char).CreateArray());
        
        
        private static readonly MethodInfo RegexIsMatchMethod = typeof(Regex).GetMethod(nameof(Regex.IsMatch), new []{ Type, Type, typeof(RegexOptions)});

        public static ExpressionBuilder<string> ToLower(this ExpressionBuilder<string> instance) {
            return Expression.Call(instance, ToLowerMethod);
        }
        public static ExpressionBuilder<string> ToUpper(this ExpressionBuilder<string> instance) {
            return Expression.Call(instance, ToUpperMethod);
        }
        public static ExpressionBuilder<bool> StartsWith(this ExpressionBuilder<string> instance, ExpressionBuilder<string> other) {
            return Expression.Call(instance, StartsWithMethod, other);
        }
        public static ExpressionBuilder<bool> EndsWith(this ExpressionBuilder<string> instance, ExpressionBuilder<string> other) {
            return Expression.Call(instance, EndsWithMethod, other);
        }
        public static ExpressionBuilder<bool> Contains(this ExpressionBuilder<string> instance, ExpressionBuilder<string> other) {
            return Expression.Call(instance, ContainsMethod, other);
        }
        public static ExpressionBuilder<int> IndexOf(this ExpressionBuilder<string> instance, ExpressionBuilder<string> other) {
            return Expression.Call(instance, IndexOfMethod, other);
        }
        public static ExpressionBuilder<string> Concat(this ExpressionBuilder<string> instance, ExpressionBuilder<string> other) {
            return Expression.Call(instance, ConcatMethod, other);
        }
        public static ExpressionBuilder<string> Concat(this ExpressionBuilder<string> instance, ExpressionBuilder<string[]> other) {
            return Expression.Call(instance, ConcatParamsMethod, other);
        }
        public static ExpressionBuilder<string> Substring(this ExpressionBuilder<string> instance, ExpressionBuilder<int> index, ExpressionBuilder<int> count) {
            return Expression.Call(instance, SubstringMethod, index, count);
        }
        public static ExpressionBuilder<string> Trim(this ExpressionBuilder<string> instance, ExpressionBuilder<char> character = null) {
            return Expression.Call(instance, TrimMethod, (character ?? ' '));
        }
        public static ExpressionBuilder<string> TrimStart(this ExpressionBuilder<string> instance, ExpressionBuilder<char> character = null) {
            return Expression.Call(instance, TrimStartMethod, (character ?? ' '));
        }
        public static ExpressionBuilder<string> TrimEnd(this ExpressionBuilder<string> instance, ExpressionBuilder<char> character = null) {
            return Expression.Call(instance, TrimEndMethod, (character ?? ' '));
        }
        public static ExpressionBuilder<bool> IsRegexMatch(this ExpressionBuilder<string> instance, ExpressionBuilder<string> pattern, ExpressionBuilder<RegexOptions> options = null) {
            return Expression.Call(RegexIsMatchMethod, instance, pattern, options ?? RegexOptions.None);
        }
    }
}