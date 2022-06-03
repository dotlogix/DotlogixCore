

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace DotLogix.Core.Expressions {
    [SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
    public static partial class Lambdas {        
        public static Lambda<IQueryable<T>> FromQueryable<T>(IQueryable<T> value) {
            return From<IQueryable<T>>(value.Expression);
        }
        public static Lambda<IQueryable> FromQueryable(IQueryable value) {
            return From<IQueryable>(value.Expression);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Aggregate{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TSource, TSource}})"/>
        public static Lambda<TSource> Aggregate<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TSource, TSource>> func) {
            return CallStatic(Queryable.Aggregate<TSource>, source, Quote(func));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Aggregate{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TSource, TSource}})"/>
        public static Lambda<TSource> Aggregate<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TSource, TSource>>> func) {
            return CallStatic(Queryable.Aggregate<TSource>, source, func);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Aggregate{TSource, TAccumulate}(System.Linq.IQueryable{TSource}, TAccumulate, System.Linq.Expressions.Expression{System.Func{TAccumulate, TSource, TAccumulate}})"/>
        public static Lambda<TAccumulate> Aggregate<TSource, TAccumulate>(this Lambda<IQueryable<TSource>> source, Lambda<TAccumulate> seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func) {
            return CallStatic(Queryable.Aggregate<TSource, TAccumulate>, source, seed, Quote(func));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Aggregate{TSource, TAccumulate}(System.Linq.IQueryable{TSource}, TAccumulate, System.Linq.Expressions.Expression{System.Func{TAccumulate, TSource, TAccumulate}})"/>
        public static Lambda<TAccumulate> Aggregate<TSource, TAccumulate>(this Lambda<IQueryable<TSource>> source, Lambda<TAccumulate> seed, Lambda<Expression<Func<TAccumulate, TSource, TAccumulate>>> func) {
            return CallStatic(Queryable.Aggregate<TSource, TAccumulate>, source, seed, func);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Aggregate{TSource, TAccumulate, TResult}(System.Linq.IQueryable{TSource}, TAccumulate, System.Linq.Expressions.Expression{System.Func{TAccumulate, TSource, TAccumulate}}, System.Linq.Expressions.Expression{System.Func{TAccumulate, TResult}})"/>
        public static Lambda<TResult> Aggregate<TSource, TAccumulate, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<TAccumulate> seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector) {
            return CallStatic(Queryable.Aggregate<TSource, TAccumulate, TResult>, source, seed, Quote(func), Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Aggregate{TSource, TAccumulate, TResult}(System.Linq.IQueryable{TSource}, TAccumulate, System.Linq.Expressions.Expression{System.Func{TAccumulate, TSource, TAccumulate}}, System.Linq.Expressions.Expression{System.Func{TAccumulate, TResult}})"/>
        public static Lambda<TResult> Aggregate<TSource, TAccumulate, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<TAccumulate> seed, Lambda<Expression<Func<TAccumulate, TSource, TAccumulate>>> func, Lambda<Expression<Func<TAccumulate, TResult>>> selector) {
            return CallStatic(Queryable.Aggregate<TSource, TAccumulate, TResult>, source, seed, func, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.All{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<bool> All<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.All<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.All{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<bool> All<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.All<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Any{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<bool> Any<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.Any<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Any{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<bool> Any<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.Any<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Any{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<bool> Any<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.Any<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.AsQueryable(System.Collections.IEnumerable)"/>
        public static Lambda<IQueryable> AsQueryable(this Lambda<IEnumerable> source) {
            return CallStatic(Queryable.AsQueryable, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.AsQueryable{TElement}(System.Collections.Generic.IEnumerable{TElement})"/>
        public static Lambda<IQueryable<TElement>> AsQueryable<TElement>(this Lambda<IEnumerable<TElement>> source) {
            return CallStatic(Queryable.AsQueryable<TElement>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average(System.Linq.IQueryable{int})"/>
        public static Lambda<double> Average(this Lambda<IQueryable<int>> source) {
            return CallStatic(Queryable.Average, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average(System.Linq.IQueryable{int?})"/>
        public static Lambda<double?> Average(this Lambda<IQueryable<int?>> source) {
            return CallStatic(Queryable.Average, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average(System.Linq.IQueryable{long})"/>
        public static Lambda<double> Average(this Lambda<IQueryable<long>> source) {
            return CallStatic(Queryable.Average, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average(System.Linq.IQueryable{long?})"/>
        public static Lambda<double?> Average(this Lambda<IQueryable<long?>> source) {
            return CallStatic(Queryable.Average, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average(System.Linq.IQueryable{float})"/>
        public static Lambda<float> Average(this Lambda<IQueryable<float>> source) {
            return CallStatic(Queryable.Average, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average(System.Linq.IQueryable{float?})"/>
        public static Lambda<float?> Average(this Lambda<IQueryable<float?>> source) {
            return CallStatic(Queryable.Average, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average(System.Linq.IQueryable{double})"/>
        public static Lambda<double> Average(this Lambda<IQueryable<double>> source) {
            return CallStatic(Queryable.Average, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average(System.Linq.IQueryable{double?})"/>
        public static Lambda<double?> Average(this Lambda<IQueryable<double?>> source) {
            return CallStatic(Queryable.Average, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average(System.Linq.IQueryable{decimal})"/>
        public static Lambda<decimal> Average(this Lambda<IQueryable<decimal>> source) {
            return CallStatic(Queryable.Average, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average(System.Linq.IQueryable{decimal?})"/>
        public static Lambda<decimal?> Average(this Lambda<IQueryable<decimal?>> source) {
            return CallStatic(Queryable.Average, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int}})"/>
        public static Lambda<double> Average<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, int>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int}})"/>
        public static Lambda<double> Average<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, int>>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int?}})"/>
        public static Lambda<double?> Average<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, int?>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int?}})"/>
        public static Lambda<double?> Average<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, int?>>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, float}})"/>
        public static Lambda<float> Average<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, float>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, float}})"/>
        public static Lambda<float> Average<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, float>>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, float?}})"/>
        public static Lambda<float?> Average<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, float?>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, float?}})"/>
        public static Lambda<float?> Average<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, float?>>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, long}})"/>
        public static Lambda<double> Average<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, long>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, long}})"/>
        public static Lambda<double> Average<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, long>>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, long?}})"/>
        public static Lambda<double?> Average<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, long?>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, long?}})"/>
        public static Lambda<double?> Average<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, long?>>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, double}})"/>
        public static Lambda<double> Average<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, double>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, double}})"/>
        public static Lambda<double> Average<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, double>>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, double?}})"/>
        public static Lambda<double?> Average<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, double?>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, double?}})"/>
        public static Lambda<double?> Average<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, double?>>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, decimal}})"/>
        public static Lambda<decimal> Average<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, decimal>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, decimal}})"/>
        public static Lambda<decimal> Average<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, decimal>>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, decimal?}})"/>
        public static Lambda<decimal?> Average<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, decimal?>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Average{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, decimal?}})"/>
        public static Lambda<decimal?> Average<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, decimal?>>> selector) {
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Cast{TResult}(System.Linq.IQueryable)"/>
        public static Lambda<IQueryable<TResult>> Cast<TResult>(this Lambda<IQueryable> source) {
            return CallStatic(Queryable.Cast<TResult>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Concat{TSource}(System.Linq.IQueryable{TSource}, System.Collections.Generic.IEnumerable{TSource})"/>
        public static Lambda<IQueryable<TSource>> Concat<TSource>(this Lambda<IQueryable<TSource>> source1, Lambda<IEnumerable<TSource>> source2) {
            return CallStatic(Queryable.Concat<TSource>, source1, source2);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Contains{TSource}(System.Linq.IQueryable{TSource}, TSource)"/>
        public static Lambda<bool> Contains<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<TSource> item) {
            return CallStatic(Queryable.Contains<TSource>, source, item);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Contains{TSource}(System.Linq.IQueryable{TSource}, TSource, System.Collections.Generic.IEqualityComparer{TSource})"/>
        public static Lambda<bool> Contains<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<TSource> item, Lambda<IEqualityComparer<TSource>> comparer) {
            return CallStatic(Queryable.Contains<TSource>, source, item, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Count{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<int> Count<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.Count<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Count{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<int> Count<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.Count<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Count{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<int> Count<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.Count<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.DefaultIfEmpty{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<IQueryable<TSource>> DefaultIfEmpty<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.DefaultIfEmpty<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.DefaultIfEmpty{TSource}(System.Linq.IQueryable{TSource}, TSource)"/>
        public static Lambda<IQueryable<TSource>> DefaultIfEmpty<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<TSource> defaultValue) {
            return CallStatic(Queryable.DefaultIfEmpty<TSource>, source, defaultValue);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Distinct{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<IQueryable<TSource>> Distinct<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.Distinct<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Distinct{TSource}(System.Linq.IQueryable{TSource}, System.Collections.Generic.IEqualityComparer{TSource})"/>
        public static Lambda<IQueryable<TSource>> Distinct<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<IEqualityComparer<TSource>> comparer) {
            return CallStatic(Queryable.Distinct<TSource>, source, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.ElementAt{TSource}(System.Linq.IQueryable{TSource}, int)"/>
        public static Lambda<TSource> ElementAt<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<int> index) {
            return CallStatic(Queryable.ElementAt<TSource>, source, index);
        }

        /// <inheritdoc cref="System.Linq.Queryable.ElementAtOrDefault{TSource}(System.Linq.IQueryable{TSource}, int)"/>
        public static Lambda<TSource> ElementAtOrDefault<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<int> index) {
            return CallStatic(Queryable.ElementAtOrDefault<TSource>, source, index);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Except{TSource}(System.Linq.IQueryable{TSource}, System.Collections.Generic.IEnumerable{TSource})"/>
        public static Lambda<IQueryable<TSource>> Except<TSource>(this Lambda<IQueryable<TSource>> source1, Lambda<IEnumerable<TSource>> source2) {
            return CallStatic(Queryable.Except<TSource>, source1, source2);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Except{TSource}(System.Linq.IQueryable{TSource}, System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEqualityComparer{TSource})"/>
        public static Lambda<IQueryable<TSource>> Except<TSource>(this Lambda<IQueryable<TSource>> source1, Lambda<IEnumerable<TSource>> source2, Lambda<IEqualityComparer<TSource>> comparer) {
            return CallStatic(Queryable.Except<TSource>, source1, source2, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.First{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<TSource> First<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.First<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.First{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<TSource> First<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.First<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.First{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<TSource> First<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.First<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.FirstOrDefault{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<TSource> FirstOrDefault<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.FirstOrDefault<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.FirstOrDefault{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<TSource> FirstOrDefault<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.FirstOrDefault<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.FirstOrDefault{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<TSource> FirstOrDefault<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.FirstOrDefault<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}})"/>
        public static Lambda<IQueryable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector) {
            return CallStatic(Queryable.GroupBy<TSource, TKey>, source, Quote(keySelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}})"/>
        public static Lambda<IQueryable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector) {
            return CallStatic(Queryable.GroupBy<TSource, TKey>, source, keySelector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Collections.Generic.IEqualityComparer{TKey})"/>
        public static Lambda<IQueryable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Lambda<IEqualityComparer<TKey>> comparer) {
            return CallStatic(Queryable.GroupBy<TSource, TKey>, source, Quote(keySelector), comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Collections.Generic.IEqualityComparer{TKey})"/>
        public static Lambda<IQueryable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector, Lambda<IEqualityComparer<TKey>> comparer) {
            return CallStatic(Queryable.GroupBy<TSource, TKey>, source, keySelector, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey, TElement}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Linq.Expressions.Expression{System.Func{TSource, TElement}})"/>
        public static Lambda<IQueryable<IGrouping<TKey, TElement>>> GroupBy<TSource, TKey, TElement>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector) {
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement>, source, Quote(keySelector), Quote(elementSelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey, TElement}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Linq.Expressions.Expression{System.Func{TSource, TElement}})"/>
        public static Lambda<IQueryable<IGrouping<TKey, TElement>>> GroupBy<TSource, TKey, TElement>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector, Lambda<Expression<Func<TSource, TElement>>> elementSelector) {
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement>, source, keySelector, elementSelector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Linq.Expressions.Expression{System.Func{TKey, System.Collections.Generic.IEnumerable{TSource}, TResult}})"/>
        public static Lambda<IQueryable<TResult>> GroupBy<TSource, TKey, TResult>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector) {
            return CallStatic(Queryable.GroupBy<TSource, TKey, TResult>, source, Quote(keySelector), Quote(resultSelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Linq.Expressions.Expression{System.Func{TKey, System.Collections.Generic.IEnumerable{TSource}, TResult}})"/>
        public static Lambda<IQueryable<TResult>> GroupBy<TSource, TKey, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector, Lambda<Expression<Func<TKey, IEnumerable<TSource>, TResult>>> resultSelector) {
            return CallStatic(Queryable.GroupBy<TSource, TKey, TResult>, source, keySelector, resultSelector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey, TElement}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Linq.Expressions.Expression{System.Func{TSource, TElement}}, System.Collections.Generic.IEqualityComparer{TKey})"/>
        public static Lambda<IQueryable<IGrouping<TKey, TElement>>> GroupBy<TSource, TKey, TElement>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Lambda<IEqualityComparer<TKey>> comparer) {
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement>, source, Quote(keySelector), Quote(elementSelector), comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey, TElement}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Linq.Expressions.Expression{System.Func{TSource, TElement}}, System.Collections.Generic.IEqualityComparer{TKey})"/>
        public static Lambda<IQueryable<IGrouping<TKey, TElement>>> GroupBy<TSource, TKey, TElement>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector, Lambda<Expression<Func<TSource, TElement>>> elementSelector, Lambda<IEqualityComparer<TKey>> comparer) {
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement>, source, keySelector, elementSelector, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Linq.Expressions.Expression{System.Func{TKey, System.Collections.Generic.IEnumerable{TSource}, TResult}}, System.Collections.Generic.IEqualityComparer{TKey})"/>
        public static Lambda<IQueryable<TResult>> GroupBy<TSource, TKey, TResult>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, Lambda<IEqualityComparer<TKey>> comparer) {
            return CallStatic(Queryable.GroupBy<TSource, TKey, TResult>, source, Quote(keySelector), Quote(resultSelector), comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Linq.Expressions.Expression{System.Func{TKey, System.Collections.Generic.IEnumerable{TSource}, TResult}}, System.Collections.Generic.IEqualityComparer{TKey})"/>
        public static Lambda<IQueryable<TResult>> GroupBy<TSource, TKey, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector, Lambda<Expression<Func<TKey, IEnumerable<TSource>, TResult>>> resultSelector, Lambda<IEqualityComparer<TKey>> comparer) {
            return CallStatic(Queryable.GroupBy<TSource, TKey, TResult>, source, keySelector, resultSelector, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey, TElement, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Linq.Expressions.Expression{System.Func{TSource, TElement}}, System.Linq.Expressions.Expression{System.Func{TKey, System.Collections.Generic.IEnumerable{TElement}, TResult}})"/>
        public static Lambda<IQueryable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector) {
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement, TResult>, source, Quote(keySelector), Quote(elementSelector), Quote(resultSelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey, TElement, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Linq.Expressions.Expression{System.Func{TSource, TElement}}, System.Linq.Expressions.Expression{System.Func{TKey, System.Collections.Generic.IEnumerable{TElement}, TResult}})"/>
        public static Lambda<IQueryable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector, Lambda<Expression<Func<TSource, TElement>>> elementSelector, Lambda<Expression<Func<TKey, IEnumerable<TElement>, TResult>>> resultSelector) {
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement, TResult>, source, keySelector, elementSelector, resultSelector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey, TElement, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Linq.Expressions.Expression{System.Func{TSource, TElement}}, System.Linq.Expressions.Expression{System.Func{TKey, System.Collections.Generic.IEnumerable{TElement}, TResult}}, System.Collections.Generic.IEqualityComparer{TKey})"/>
        public static Lambda<IQueryable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, Lambda<IEqualityComparer<TKey>> comparer) {
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement, TResult>, source, Quote(keySelector), Quote(elementSelector), Quote(resultSelector), comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupBy{TSource, TKey, TElement, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Linq.Expressions.Expression{System.Func{TSource, TElement}}, System.Linq.Expressions.Expression{System.Func{TKey, System.Collections.Generic.IEnumerable{TElement}, TResult}}, System.Collections.Generic.IEqualityComparer{TKey})"/>
        public static Lambda<IQueryable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector, Lambda<Expression<Func<TSource, TElement>>> elementSelector, Lambda<Expression<Func<TKey, IEnumerable<TElement>, TResult>>> resultSelector, Lambda<IEqualityComparer<TKey>> comparer) {
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement, TResult>, source, keySelector, elementSelector, resultSelector, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupJoin{TOuter, TInner, TKey, TResult}(System.Linq.IQueryable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Linq.Expressions.Expression{System.Func{TOuter, TKey}}, System.Linq.Expressions.Expression{System.Func{TInner, TKey}}, System.Linq.Expressions.Expression{System.Func{TOuter, System.Collections.Generic.IEnumerable{TInner}, TResult}})"/>
        public static Lambda<IQueryable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this Lambda<IQueryable<TOuter>> outer, Lambda<IEnumerable<TInner>> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector) {
            return CallStatic(Queryable.GroupJoin<TOuter, TInner, TKey, TResult>, outer, inner, Quote(outerKeySelector), Quote(innerKeySelector), Quote(resultSelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupJoin{TOuter, TInner, TKey, TResult}(System.Linq.IQueryable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Linq.Expressions.Expression{System.Func{TOuter, TKey}}, System.Linq.Expressions.Expression{System.Func{TInner, TKey}}, System.Linq.Expressions.Expression{System.Func{TOuter, System.Collections.Generic.IEnumerable{TInner}, TResult}})"/>
        public static Lambda<IQueryable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this Lambda<IQueryable<TOuter>> outer, Lambda<IEnumerable<TInner>> inner, Lambda<Expression<Func<TOuter, TKey>>> outerKeySelector, Lambda<Expression<Func<TInner, TKey>>> innerKeySelector, Lambda<Expression<Func<TOuter, IEnumerable<TInner>, TResult>>> resultSelector) {
            return CallStatic(Queryable.GroupJoin<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupJoin{TOuter, TInner, TKey, TResult}(System.Linq.IQueryable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Linq.Expressions.Expression{System.Func{TOuter, TKey}}, System.Linq.Expressions.Expression{System.Func{TInner, TKey}}, System.Linq.Expressions.Expression{System.Func{TOuter, System.Collections.Generic.IEnumerable{TInner}, TResult}}, System.Collections.Generic.IEqualityComparer{TKey})"/>
        public static Lambda<IQueryable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this Lambda<IQueryable<TOuter>> outer, Lambda<IEnumerable<TInner>> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, Lambda<IEqualityComparer<TKey>> comparer) {
            return CallStatic(Queryable.GroupJoin<TOuter, TInner, TKey, TResult>, outer, inner, Quote(outerKeySelector), Quote(innerKeySelector), Quote(resultSelector), comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.GroupJoin{TOuter, TInner, TKey, TResult}(System.Linq.IQueryable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Linq.Expressions.Expression{System.Func{TOuter, TKey}}, System.Linq.Expressions.Expression{System.Func{TInner, TKey}}, System.Linq.Expressions.Expression{System.Func{TOuter, System.Collections.Generic.IEnumerable{TInner}, TResult}}, System.Collections.Generic.IEqualityComparer{TKey})"/>
        public static Lambda<IQueryable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this Lambda<IQueryable<TOuter>> outer, Lambda<IEnumerable<TInner>> inner, Lambda<Expression<Func<TOuter, TKey>>> outerKeySelector, Lambda<Expression<Func<TInner, TKey>>> innerKeySelector, Lambda<Expression<Func<TOuter, IEnumerable<TInner>, TResult>>> resultSelector, Lambda<IEqualityComparer<TKey>> comparer) {
            return CallStatic(Queryable.GroupJoin<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Intersect{TSource}(System.Linq.IQueryable{TSource}, System.Collections.Generic.IEnumerable{TSource})"/>
        public static Lambda<IQueryable<TSource>> Intersect<TSource>(this Lambda<IQueryable<TSource>> source1, Lambda<IEnumerable<TSource>> source2) {
            return CallStatic(Queryable.Intersect<TSource>, source1, source2);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Intersect{TSource}(System.Linq.IQueryable{TSource}, System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEqualityComparer{TSource})"/>
        public static Lambda<IQueryable<TSource>> Intersect<TSource>(this Lambda<IQueryable<TSource>> source1, Lambda<IEnumerable<TSource>> source2, Lambda<IEqualityComparer<TSource>> comparer) {
            return CallStatic(Queryable.Intersect<TSource>, source1, source2, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Join{TOuter, TInner, TKey, TResult}(System.Linq.IQueryable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Linq.Expressions.Expression{System.Func{TOuter, TKey}}, System.Linq.Expressions.Expression{System.Func{TInner, TKey}}, System.Linq.Expressions.Expression{System.Func{TOuter, TInner, TResult}})"/>
        public static Lambda<IQueryable<TResult>> Join<TOuter, TInner, TKey, TResult>(this Lambda<IQueryable<TOuter>> outer, Lambda<IEnumerable<TInner>> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector) {
            return CallStatic(Queryable.Join<TOuter, TInner, TKey, TResult>, outer, inner, Quote(outerKeySelector), Quote(innerKeySelector), Quote(resultSelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Join{TOuter, TInner, TKey, TResult}(System.Linq.IQueryable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Linq.Expressions.Expression{System.Func{TOuter, TKey}}, System.Linq.Expressions.Expression{System.Func{TInner, TKey}}, System.Linq.Expressions.Expression{System.Func{TOuter, TInner, TResult}})"/>
        public static Lambda<IQueryable<TResult>> Join<TOuter, TInner, TKey, TResult>(this Lambda<IQueryable<TOuter>> outer, Lambda<IEnumerable<TInner>> inner, Lambda<Expression<Func<TOuter, TKey>>> outerKeySelector, Lambda<Expression<Func<TInner, TKey>>> innerKeySelector, Lambda<Expression<Func<TOuter, TInner, TResult>>> resultSelector) {
            return CallStatic(Queryable.Join<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Join{TOuter, TInner, TKey, TResult}(System.Linq.IQueryable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Linq.Expressions.Expression{System.Func{TOuter, TKey}}, System.Linq.Expressions.Expression{System.Func{TInner, TKey}}, System.Linq.Expressions.Expression{System.Func{TOuter, TInner, TResult}}, System.Collections.Generic.IEqualityComparer{TKey})"/>
        public static Lambda<IQueryable<TResult>> Join<TOuter, TInner, TKey, TResult>(this Lambda<IQueryable<TOuter>> outer, Lambda<IEnumerable<TInner>> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, Lambda<IEqualityComparer<TKey>> comparer) {
            return CallStatic(Queryable.Join<TOuter, TInner, TKey, TResult>, outer, inner, Quote(outerKeySelector), Quote(innerKeySelector), Quote(resultSelector), comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Join{TOuter, TInner, TKey, TResult}(System.Linq.IQueryable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Linq.Expressions.Expression{System.Func{TOuter, TKey}}, System.Linq.Expressions.Expression{System.Func{TInner, TKey}}, System.Linq.Expressions.Expression{System.Func{TOuter, TInner, TResult}}, System.Collections.Generic.IEqualityComparer{TKey})"/>
        public static Lambda<IQueryable<TResult>> Join<TOuter, TInner, TKey, TResult>(this Lambda<IQueryable<TOuter>> outer, Lambda<IEnumerable<TInner>> inner, Lambda<Expression<Func<TOuter, TKey>>> outerKeySelector, Lambda<Expression<Func<TInner, TKey>>> innerKeySelector, Lambda<Expression<Func<TOuter, TInner, TResult>>> resultSelector, Lambda<IEqualityComparer<TKey>> comparer) {
            return CallStatic(Queryable.Join<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Last{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<TSource> Last<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.Last<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Last{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<TSource> Last<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.Last<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Last{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<TSource> Last<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.Last<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.LastOrDefault{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<TSource> LastOrDefault<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.LastOrDefault<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.LastOrDefault{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<TSource> LastOrDefault<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.LastOrDefault<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.LastOrDefault{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<TSource> LastOrDefault<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.LastOrDefault<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.LongCount{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<long> LongCount<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.LongCount<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.LongCount{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<long> LongCount<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.LongCount<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.LongCount{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<long> LongCount<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.LongCount<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Max{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<TSource> Max<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.Max<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Max{TSource, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TResult}})"/>
        public static Lambda<TResult> Max<TSource, TResult>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TResult>> selector) {
            return CallStatic(Queryable.Max<TSource, TResult>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Max{TSource, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TResult}})"/>
        public static Lambda<TResult> Max<TSource, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TResult>>> selector) {
            return CallStatic(Queryable.Max<TSource, TResult>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Min{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<TSource> Min<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.Min<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Min{TSource, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TResult}})"/>
        public static Lambda<TResult> Min<TSource, TResult>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TResult>> selector) {
            return CallStatic(Queryable.Min<TSource, TResult>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Min{TSource, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TResult}})"/>
        public static Lambda<TResult> Min<TSource, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TResult>>> selector) {
            return CallStatic(Queryable.Min<TSource, TResult>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.OfType{TResult}(System.Linq.IQueryable)"/>
        public static Lambda<IQueryable<TResult>> OfType<TResult>(this Lambda<IQueryable> source) {
            return CallStatic(Queryable.OfType<TResult>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.OrderBy{TSource, TKey}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}})"/>
        public static Lambda<IOrderedQueryable<TSource>> OrderBy<TSource, TKey>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector) {
            return CallStatic(Queryable.OrderBy<TSource, TKey>, source, Quote(keySelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.OrderBy{TSource, TKey}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}})"/>
        public static Lambda<IOrderedQueryable<TSource>> OrderBy<TSource, TKey>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector) {
            return CallStatic(Queryable.OrderBy<TSource, TKey>, source, keySelector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.OrderBy{TSource, TKey}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Collections.Generic.IComparer{TKey})"/>
        public static Lambda<IOrderedQueryable<TSource>> OrderBy<TSource, TKey>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Lambda<IComparer<TKey>> comparer) {
            return CallStatic(Queryable.OrderBy<TSource, TKey>, source, Quote(keySelector), comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.OrderBy{TSource, TKey}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Collections.Generic.IComparer{TKey})"/>
        public static Lambda<IOrderedQueryable<TSource>> OrderBy<TSource, TKey>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector, Lambda<IComparer<TKey>> comparer) {
            return CallStatic(Queryable.OrderBy<TSource, TKey>, source, keySelector, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.OrderByDescending{TSource, TKey}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}})"/>
        public static Lambda<IOrderedQueryable<TSource>> OrderByDescending<TSource, TKey>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector) {
            return CallStatic(Queryable.OrderByDescending<TSource, TKey>, source, Quote(keySelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.OrderByDescending{TSource, TKey}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}})"/>
        public static Lambda<IOrderedQueryable<TSource>> OrderByDescending<TSource, TKey>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector) {
            return CallStatic(Queryable.OrderByDescending<TSource, TKey>, source, keySelector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.OrderByDescending{TSource, TKey}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Collections.Generic.IComparer{TKey})"/>
        public static Lambda<IOrderedQueryable<TSource>> OrderByDescending<TSource, TKey>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Lambda<IComparer<TKey>> comparer) {
            return CallStatic(Queryable.OrderByDescending<TSource, TKey>, source, Quote(keySelector), comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.OrderByDescending{TSource, TKey}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Collections.Generic.IComparer{TKey})"/>
        public static Lambda<IOrderedQueryable<TSource>> OrderByDescending<TSource, TKey>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector, Lambda<IComparer<TKey>> comparer) {
            return CallStatic(Queryable.OrderByDescending<TSource, TKey>, source, keySelector, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Reverse{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<IQueryable<TSource>> Reverse<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.Reverse<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Select{TSource, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TResult}})"/>
        public static Lambda<IQueryable<TResult>> Select<TSource, TResult>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, TResult>> selector) {
            return CallStatic(Queryable.Select<TSource, TResult>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Select{TSource, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TResult}})"/>
        public static Lambda<IQueryable<TResult>> Select<TSource, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, TResult>>> selector) {
            return CallStatic(Queryable.Select<TSource, TResult>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Select{TSource, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int, TResult}})"/>
        public static Lambda<IQueryable<TResult>> Select<TSource, TResult>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, int, TResult>> selector) {
            return CallStatic(Queryable.Select<TSource, TResult>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Select{TSource, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int, TResult}})"/>
        public static Lambda<IQueryable<TResult>> Select<TSource, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, int, TResult>>> selector) {
            return CallStatic(Queryable.Select<TSource, TResult>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.SelectMany{TSource, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, System.Collections.Generic.IEnumerable{TResult}}})"/>
        public static Lambda<IQueryable<TResult>> SelectMany<TSource, TResult>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, IEnumerable<TResult>>> selector) {
            return CallStatic(Queryable.SelectMany<TSource, TResult>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.SelectMany{TSource, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, System.Collections.Generic.IEnumerable{TResult}}})"/>
        public static Lambda<IQueryable<TResult>> SelectMany<TSource, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, IEnumerable<TResult>>>> selector) {
            return CallStatic(Queryable.SelectMany<TSource, TResult>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.SelectMany{TSource, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int, System.Collections.Generic.IEnumerable{TResult}}})"/>
        public static Lambda<IQueryable<TResult>> SelectMany<TSource, TResult>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, int, IEnumerable<TResult>>> selector) {
            return CallStatic(Queryable.SelectMany<TSource, TResult>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.SelectMany{TSource, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int, System.Collections.Generic.IEnumerable{TResult}}})"/>
        public static Lambda<IQueryable<TResult>> SelectMany<TSource, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, int, IEnumerable<TResult>>>> selector) {
            return CallStatic(Queryable.SelectMany<TSource, TResult>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.SelectMany{TSource, TCollection, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int, System.Collections.Generic.IEnumerable{TCollection}}}, System.Linq.Expressions.Expression{System.Func{TSource, TCollection, TResult}})"/>
        public static Lambda<IQueryable<TResult>> SelectMany<TSource, TCollection, TResult>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector) {
            return CallStatic(Queryable.SelectMany<TSource, TCollection, TResult>, source, Quote(collectionSelector), Quote(resultSelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.SelectMany{TSource, TCollection, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int, System.Collections.Generic.IEnumerable{TCollection}}}, System.Linq.Expressions.Expression{System.Func{TSource, TCollection, TResult}})"/>
        public static Lambda<IQueryable<TResult>> SelectMany<TSource, TCollection, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, int, IEnumerable<TCollection>>>> collectionSelector, Lambda<Expression<Func<TSource, TCollection, TResult>>> resultSelector) {
            return CallStatic(Queryable.SelectMany<TSource, TCollection, TResult>, source, collectionSelector, resultSelector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.SelectMany{TSource, TCollection, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, System.Collections.Generic.IEnumerable{TCollection}}}, System.Linq.Expressions.Expression{System.Func{TSource, TCollection, TResult}})"/>
        public static Lambda<IQueryable<TResult>> SelectMany<TSource, TCollection, TResult>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector) {
            return CallStatic(Queryable.SelectMany<TSource, TCollection, TResult>, source, Quote(collectionSelector), Quote(resultSelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.SelectMany{TSource, TCollection, TResult}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, System.Collections.Generic.IEnumerable{TCollection}}}, System.Linq.Expressions.Expression{System.Func{TSource, TCollection, TResult}})"/>
        public static Lambda<IQueryable<TResult>> SelectMany<TSource, TCollection, TResult>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, IEnumerable<TCollection>>>> collectionSelector, Lambda<Expression<Func<TSource, TCollection, TResult>>> resultSelector) {
            return CallStatic(Queryable.SelectMany<TSource, TCollection, TResult>, source, collectionSelector, resultSelector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.SequenceEqual{TSource}(System.Linq.IQueryable{TSource}, System.Collections.Generic.IEnumerable{TSource})"/>
        public static Lambda<bool> SequenceEqual<TSource>(this Lambda<IQueryable<TSource>> source1, Lambda<IEnumerable<TSource>> source2) {
            return CallStatic(Queryable.SequenceEqual<TSource>, source1, source2);
        }

        /// <inheritdoc cref="System.Linq.Queryable.SequenceEqual{TSource}(System.Linq.IQueryable{TSource}, System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEqualityComparer{TSource})"/>
        public static Lambda<bool> SequenceEqual<TSource>(this Lambda<IQueryable<TSource>> source1, Lambda<IEnumerable<TSource>> source2, Lambda<IEqualityComparer<TSource>> comparer) {
            return CallStatic(Queryable.SequenceEqual<TSource>, source1, source2, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Single{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<TSource> Single<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.Single<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Single{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<TSource> Single<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.Single<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Single{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<TSource> Single<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.Single<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.SingleOrDefault{TSource}(System.Linq.IQueryable{TSource})"/>
        public static Lambda<TSource> SingleOrDefault<TSource>(this Lambda<IQueryable<TSource>> source) {
            return CallStatic(Queryable.SingleOrDefault<TSource>, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.SingleOrDefault{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<TSource> SingleOrDefault<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.SingleOrDefault<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.SingleOrDefault{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<TSource> SingleOrDefault<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.SingleOrDefault<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Skip{TSource}(System.Linq.IQueryable{TSource}, int)"/>
        public static Lambda<IQueryable<TSource>> Skip<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<int> count) {
            return CallStatic(Queryable.Skip<TSource>, source, count);
        }

        /// <inheritdoc cref="System.Linq.Queryable.SkipWhile{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<IQueryable<TSource>> SkipWhile<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.SkipWhile<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.SkipWhile{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<IQueryable<TSource>> SkipWhile<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.SkipWhile<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.SkipWhile{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int, bool}})"/>
        public static Lambda<IQueryable<TSource>> SkipWhile<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, int, bool>> predicate) {
            return CallStatic(Queryable.SkipWhile<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.SkipWhile{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int, bool}})"/>
        public static Lambda<IQueryable<TSource>> SkipWhile<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, int, bool>>> predicate) {
            return CallStatic(Queryable.SkipWhile<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum(System.Linq.IQueryable{int})"/>
        public static Lambda<int> Sum(this Lambda<IQueryable<int>> source) {
            return CallStatic(Queryable.Sum, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum(System.Linq.IQueryable{int?})"/>
        public static Lambda<int?> Sum(this Lambda<IQueryable<int?>> source) {
            return CallStatic(Queryable.Sum, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum(System.Linq.IQueryable{long})"/>
        public static Lambda<long> Sum(this Lambda<IQueryable<long>> source) {
            return CallStatic(Queryable.Sum, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum(System.Linq.IQueryable{long?})"/>
        public static Lambda<long?> Sum(this Lambda<IQueryable<long?>> source) {
            return CallStatic(Queryable.Sum, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum(System.Linq.IQueryable{float})"/>
        public static Lambda<float> Sum(this Lambda<IQueryable<float>> source) {
            return CallStatic(Queryable.Sum, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum(System.Linq.IQueryable{float?})"/>
        public static Lambda<float?> Sum(this Lambda<IQueryable<float?>> source) {
            return CallStatic(Queryable.Sum, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum(System.Linq.IQueryable{double})"/>
        public static Lambda<double> Sum(this Lambda<IQueryable<double>> source) {
            return CallStatic(Queryable.Sum, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum(System.Linq.IQueryable{double?})"/>
        public static Lambda<double?> Sum(this Lambda<IQueryable<double?>> source) {
            return CallStatic(Queryable.Sum, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum(System.Linq.IQueryable{decimal})"/>
        public static Lambda<decimal> Sum(this Lambda<IQueryable<decimal>> source) {
            return CallStatic(Queryable.Sum, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum(System.Linq.IQueryable{decimal?})"/>
        public static Lambda<decimal?> Sum(this Lambda<IQueryable<decimal?>> source) {
            return CallStatic(Queryable.Sum, source);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int}})"/>
        public static Lambda<int> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, int>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int}})"/>
        public static Lambda<int> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, int>>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int?}})"/>
        public static Lambda<int?> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, int?>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int?}})"/>
        public static Lambda<int?> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, int?>>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, long}})"/>
        public static Lambda<long> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, long>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, long}})"/>
        public static Lambda<long> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, long>>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, long?}})"/>
        public static Lambda<long?> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, long?>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, long?}})"/>
        public static Lambda<long?> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, long?>>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, float}})"/>
        public static Lambda<float> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, float>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, float}})"/>
        public static Lambda<float> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, float>>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, float?}})"/>
        public static Lambda<float?> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, float?>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, float?}})"/>
        public static Lambda<float?> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, float?>>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, double}})"/>
        public static Lambda<double> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, double>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, double}})"/>
        public static Lambda<double> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, double>>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, double?}})"/>
        public static Lambda<double?> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, double?>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, double?}})"/>
        public static Lambda<double?> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, double?>>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, decimal}})"/>
        public static Lambda<decimal> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, decimal>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, decimal}})"/>
        public static Lambda<decimal> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, decimal>>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, decimal?}})"/>
        public static Lambda<decimal?> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, decimal?>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Sum{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, decimal?}})"/>
        public static Lambda<decimal?> Sum<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, decimal?>>> selector) {
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Take{TSource}(System.Linq.IQueryable{TSource}, int)"/>
        public static Lambda<IQueryable<TSource>> Take<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<int> count) {
            return CallStatic(Queryable.Take<TSource>, source, count);
        }

        /// <inheritdoc cref="System.Linq.Queryable.TakeWhile{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<IQueryable<TSource>> TakeWhile<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.TakeWhile<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.TakeWhile{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<IQueryable<TSource>> TakeWhile<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.TakeWhile<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.TakeWhile{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int, bool}})"/>
        public static Lambda<IQueryable<TSource>> TakeWhile<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, int, bool>> predicate) {
            return CallStatic(Queryable.TakeWhile<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.TakeWhile{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int, bool}})"/>
        public static Lambda<IQueryable<TSource>> TakeWhile<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, int, bool>>> predicate) {
            return CallStatic(Queryable.TakeWhile<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.ThenBy{TSource, TKey}(System.Linq.IOrderedQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}})"/>
        public static Lambda<IOrderedQueryable<TSource>> ThenBy<TSource, TKey>(this Lambda<IOrderedQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector) {
            return CallStatic(Queryable.ThenBy<TSource, TKey>, source, Quote(keySelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.ThenBy{TSource, TKey}(System.Linq.IOrderedQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}})"/>
        public static Lambda<IOrderedQueryable<TSource>> ThenBy<TSource, TKey>(this Lambda<IOrderedQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector) {
            return CallStatic(Queryable.ThenBy<TSource, TKey>, source, keySelector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.ThenBy{TSource, TKey}(System.Linq.IOrderedQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Collections.Generic.IComparer{TKey})"/>
        public static Lambda<IOrderedQueryable<TSource>> ThenBy<TSource, TKey>(this Lambda<IOrderedQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Lambda<IComparer<TKey>> comparer) {
            return CallStatic(Queryable.ThenBy<TSource, TKey>, source, Quote(keySelector), comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.ThenBy{TSource, TKey}(System.Linq.IOrderedQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Collections.Generic.IComparer{TKey})"/>
        public static Lambda<IOrderedQueryable<TSource>> ThenBy<TSource, TKey>(this Lambda<IOrderedQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector, Lambda<IComparer<TKey>> comparer) {
            return CallStatic(Queryable.ThenBy<TSource, TKey>, source, keySelector, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.ThenByDescending{TSource, TKey}(System.Linq.IOrderedQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}})"/>
        public static Lambda<IOrderedQueryable<TSource>> ThenByDescending<TSource, TKey>(this Lambda<IOrderedQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector) {
            return CallStatic(Queryable.ThenByDescending<TSource, TKey>, source, Quote(keySelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.ThenByDescending{TSource, TKey}(System.Linq.IOrderedQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}})"/>
        public static Lambda<IOrderedQueryable<TSource>> ThenByDescending<TSource, TKey>(this Lambda<IOrderedQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector) {
            return CallStatic(Queryable.ThenByDescending<TSource, TKey>, source, keySelector);
        }

        /// <inheritdoc cref="System.Linq.Queryable.ThenByDescending{TSource, TKey}(System.Linq.IOrderedQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Collections.Generic.IComparer{TKey})"/>
        public static Lambda<IOrderedQueryable<TSource>> ThenByDescending<TSource, TKey>(this Lambda<IOrderedQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Lambda<IComparer<TKey>> comparer) {
            return CallStatic(Queryable.ThenByDescending<TSource, TKey>, source, Quote(keySelector), comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.ThenByDescending{TSource, TKey}(System.Linq.IOrderedQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, TKey}}, System.Collections.Generic.IComparer{TKey})"/>
        public static Lambda<IOrderedQueryable<TSource>> ThenByDescending<TSource, TKey>(this Lambda<IOrderedQueryable<TSource>> source, Lambda<Expression<Func<TSource, TKey>>> keySelector, Lambda<IComparer<TKey>> comparer) {
            return CallStatic(Queryable.ThenByDescending<TSource, TKey>, source, keySelector, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Union{TSource}(System.Linq.IQueryable{TSource}, System.Collections.Generic.IEnumerable{TSource})"/>
        public static Lambda<IQueryable<TSource>> Union<TSource>(this Lambda<IQueryable<TSource>> source1, Lambda<IEnumerable<TSource>> source2) {
            return CallStatic(Queryable.Union<TSource>, source1, source2);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Union{TSource}(System.Linq.IQueryable{TSource}, System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEqualityComparer{TSource})"/>
        public static Lambda<IQueryable<TSource>> Union<TSource>(this Lambda<IQueryable<TSource>> source1, Lambda<IEnumerable<TSource>> source2, Lambda<IEqualityComparer<TSource>> comparer) {
            return CallStatic(Queryable.Union<TSource>, source1, source2, comparer);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Where{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<IQueryable<TSource>> Where<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            return CallStatic(Queryable.Where<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Where{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, bool}})"/>
        public static Lambda<IQueryable<TSource>> Where<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, bool>>> predicate) {
            return CallStatic(Queryable.Where<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Where{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int, bool}})"/>
        public static Lambda<IQueryable<TSource>> Where<TSource>(this Lambda<IQueryable<TSource>> source, Expression<Func<TSource, int, bool>> predicate) {
            return CallStatic(Queryable.Where<TSource>, source, Quote(predicate));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Where{TSource}(System.Linq.IQueryable{TSource}, System.Linq.Expressions.Expression{System.Func{TSource, int, bool}})"/>
        public static Lambda<IQueryable<TSource>> Where<TSource>(this Lambda<IQueryable<TSource>> source, Lambda<Expression<Func<TSource, int, bool>>> predicate) {
            return CallStatic(Queryable.Where<TSource>, source, predicate);
        }

        /// <inheritdoc cref="System.Linq.Queryable.Zip{TFirst, TSecond, TResult}(System.Linq.IQueryable{TFirst}, System.Collections.Generic.IEnumerable{TSecond}, System.Linq.Expressions.Expression{System.Func{TFirst, TSecond, TResult}})"/>
        public static Lambda<IQueryable<TResult>> Zip<TFirst, TSecond, TResult>(this Lambda<IQueryable<TFirst>> source1, Lambda<IEnumerable<TSecond>> source2, Expression<Func<TFirst, TSecond, TResult>> resultSelector) {
            return CallStatic(Queryable.Zip<TFirst, TSecond, TResult>, source1, source2, Quote(resultSelector));
        }

        /// <inheritdoc cref="System.Linq.Queryable.Zip{TFirst, TSecond, TResult}(System.Linq.IQueryable{TFirst}, System.Collections.Generic.IEnumerable{TSecond}, System.Linq.Expressions.Expression{System.Func{TFirst, TSecond, TResult}})"/>
        public static Lambda<IQueryable<TResult>> Zip<TFirst, TSecond, TResult>(this Lambda<IQueryable<TFirst>> source1, Lambda<IEnumerable<TSecond>> source2, Lambda<Expression<Func<TFirst, TSecond, TResult>>> resultSelector) {
            return CallStatic(Queryable.Zip<TFirst, TSecond, TResult>, source1, source2, resultSelector);
        }

    }
}