// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LambdaBuilders.Querable.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.06.2021 00:20
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DotLogix.Core.Expressions {
    public static partial class LambdaBuilders {        
        public static LambdaBuilder<IQueryable<T>> FromQueryable<T>(IQueryable<T> value) {
            return From<IQueryable<T>>(value.Expression);
        }
        public static LambdaBuilder<IQueryable> FromQueryable(IQueryable value) {
            return From<IQueryable>(value.Expression);
        }

        public static LambdaBuilder<TSource> Aggregate<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TSource, TSource>> func){
            return CallStatic(Queryable.Aggregate<TSource>, source, Quote(func));
        }
        public static LambdaBuilder<TSource> Aggregate<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TSource, TSource>>> func){
            return CallStatic(Queryable.Aggregate<TSource>, source, func);
        }

        public static LambdaBuilder<TAccumulate> Aggregate<TSource, TAccumulate>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<TAccumulate> seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func){
            return CallStatic(Queryable.Aggregate<TSource, TAccumulate>, source, seed, Quote(func));
        }
        public static LambdaBuilder<TAccumulate> Aggregate<TSource, TAccumulate>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<TAccumulate> seed, LambdaBuilder<Expression<Func<TAccumulate, TSource, TAccumulate>>> func){
            return CallStatic(Queryable.Aggregate<TSource, TAccumulate>, source, seed, func);
        }

        public static LambdaBuilder<TResult> Aggregate<TSource, TAccumulate, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<TAccumulate> seed, Expression<Func<TAccumulate, TSource, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector){
            return CallStatic(Queryable.Aggregate<TSource, TAccumulate, TResult>, source, seed, Quote(func), Quote(selector));
        }
        public static LambdaBuilder<TResult> Aggregate<TSource, TAccumulate, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<TAccumulate> seed, LambdaBuilder<Expression<Func<TAccumulate, TSource, TAccumulate>>> func, LambdaBuilder<Expression<Func<TAccumulate, TResult>>> selector){
            return CallStatic(Queryable.Aggregate<TSource, TAccumulate, TResult>, source, seed, func, selector);
        }

        public static LambdaBuilder<bool> All<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.All<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<bool> All<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.All<TSource>, source, predicate);
        }

        public static LambdaBuilder<bool> Any<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.Any<TSource>, source);
        }

        public static LambdaBuilder<bool> Any<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.Any<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<bool> Any<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.Any<TSource>, source, predicate);
        }

        public static LambdaBuilder<IQueryable> AsQueryable(this LambdaBuilder<IEnumerable> source){
            return CallStatic(Queryable.AsQueryable, source);
        }

        public static LambdaBuilder<IQueryable<TElement>> AsQueryable<TElement>(this LambdaBuilder<IEnumerable<TElement>> source){
            return CallStatic(Queryable.AsQueryable<TElement>, source);
        }

        public static LambdaBuilder<double> Average(this LambdaBuilder<IQueryable<int>> source){
            return CallStatic(Queryable.Average, source);
        }

        public static LambdaBuilder<double?> Average(this LambdaBuilder<IQueryable<int?>> source){
            return CallStatic(Queryable.Average, source);
        }

        public static LambdaBuilder<double> Average(this LambdaBuilder<IQueryable<long>> source){
            return CallStatic(Queryable.Average, source);
        }

        public static LambdaBuilder<double?> Average(this LambdaBuilder<IQueryable<long?>> source){
            return CallStatic(Queryable.Average, source);
        }

        public static LambdaBuilder<float> Average(this LambdaBuilder<IQueryable<float>> source){
            return CallStatic(Queryable.Average, source);
        }

        public static LambdaBuilder<float?> Average(this LambdaBuilder<IQueryable<float?>> source){
            return CallStatic(Queryable.Average, source);
        }

        public static LambdaBuilder<double> Average(this LambdaBuilder<IQueryable<double>> source){
            return CallStatic(Queryable.Average, source);
        }

        public static LambdaBuilder<double?> Average(this LambdaBuilder<IQueryable<double?>> source){
            return CallStatic(Queryable.Average, source);
        }

        public static LambdaBuilder<decimal> Average(this LambdaBuilder<IQueryable<decimal>> source){
            return CallStatic(Queryable.Average, source);
        }

        public static LambdaBuilder<decimal?> Average(this LambdaBuilder<IQueryable<decimal?>> source){
            return CallStatic(Queryable.Average, source);
        }

        public static LambdaBuilder<double> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, int>> selector){
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<double> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, int>>> selector){
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<double?> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, int?>> selector){
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<double?> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, int?>>> selector){
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<float> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, float>> selector){
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<float> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, float>>> selector){
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<float?> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, float?>> selector){
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<float?> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, float?>>> selector){
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<double> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, long>> selector){
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<double> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, long>>> selector){
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<double?> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, long?>> selector){
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<double?> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, long?>>> selector){
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<double> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, double>> selector){
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<double> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, double>>> selector){
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<double?> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, double?>> selector){
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<double?> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, double?>>> selector){
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<decimal> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, decimal>> selector){
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<decimal> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, decimal>>> selector){
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<decimal?> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, decimal?>> selector){
            return CallStatic(Queryable.Average<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<decimal?> Average<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, decimal?>>> selector){
            return CallStatic(Queryable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<IQueryable<TResult>> Cast<TResult>(this LambdaBuilder<IQueryable> source){
            return CallStatic(Queryable.Cast<TResult>, source);
        }

        public static LambdaBuilder<IQueryable<TSource>> Concat<TSource>(this LambdaBuilder<IQueryable<TSource>> source1, LambdaBuilder<IEnumerable<TSource>> source2){
            return CallStatic(Queryable.Concat<TSource>, source1, source2);
        }

        public static LambdaBuilder<bool> Contains<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<TSource> item){
            return CallStatic(Queryable.Contains<TSource>, source, item);
        }

        public static LambdaBuilder<bool> Contains<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<TSource> item, LambdaBuilder<IEqualityComparer<TSource>> comparer){
            return CallStatic(Queryable.Contains<TSource>, source, item, comparer);
        }

        public static LambdaBuilder<int> Count<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.Count<TSource>, source);
        }

        public static LambdaBuilder<int> Count<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.Count<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<int> Count<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.Count<TSource>, source, predicate);
        }

        public static LambdaBuilder<IQueryable<TSource>> DefaultIfEmpty<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.DefaultIfEmpty<TSource>, source);
        }

        public static LambdaBuilder<IQueryable<TSource>> DefaultIfEmpty<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<TSource> defaultValue){
            return CallStatic(Queryable.DefaultIfEmpty<TSource>, source, defaultValue);
        }

        public static LambdaBuilder<IQueryable<TSource>> Distinct<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.Distinct<TSource>, source);
        }

        public static LambdaBuilder<IQueryable<TSource>> Distinct<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<IEqualityComparer<TSource>> comparer){
            return CallStatic(Queryable.Distinct<TSource>, source, comparer);
        }

        public static LambdaBuilder<TSource> ElementAt<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<int> index){
            return CallStatic(Queryable.ElementAt<TSource>, source, index);
        }

        public static LambdaBuilder<TSource> ElementAtOrDefault<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<int> index){
            return CallStatic(Queryable.ElementAtOrDefault<TSource>, source, index);
        }

        public static LambdaBuilder<IQueryable<TSource>> Except<TSource>(this LambdaBuilder<IQueryable<TSource>> source1, LambdaBuilder<IEnumerable<TSource>> source2){
            return CallStatic(Queryable.Except<TSource>, source1, source2);
        }

        public static LambdaBuilder<IQueryable<TSource>> Except<TSource>(this LambdaBuilder<IQueryable<TSource>> source1, LambdaBuilder<IEnumerable<TSource>> source2, LambdaBuilder<IEqualityComparer<TSource>> comparer){
            return CallStatic(Queryable.Except<TSource>, source1, source2, comparer);
        }

        public static LambdaBuilder<TSource> First<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.First<TSource>, source);
        }

        public static LambdaBuilder<TSource> First<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.First<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<TSource> First<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.First<TSource>, source, predicate);
        }

        public static LambdaBuilder<TSource> FirstOrDefault<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.FirstOrDefault<TSource>, source);
        }

        public static LambdaBuilder<TSource> FirstOrDefault<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.FirstOrDefault<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<TSource> FirstOrDefault<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.FirstOrDefault<TSource>, source, predicate);
        }

        public static LambdaBuilder<IQueryable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector){
            return CallStatic(Queryable.GroupBy<TSource, TKey>, source, Quote(keySelector));
        }
        public static LambdaBuilder<IQueryable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector){
            return CallStatic(Queryable.GroupBy<TSource, TKey>, source, keySelector);
        }

        public static LambdaBuilder<IQueryable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Queryable.GroupBy<TSource, TKey>, source, Quote(keySelector), comparer);
        }
        public static LambdaBuilder<IQueryable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Queryable.GroupBy<TSource, TKey>, source, keySelector, comparer);
        }

        public static LambdaBuilder<IQueryable<IGrouping<TKey, TElement>>> GroupBy<TSource, TKey, TElement>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector){
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement>, source, Quote(keySelector), Quote(elementSelector));
        }
        public static LambdaBuilder<IQueryable<IGrouping<TKey, TElement>>> GroupBy<TSource, TKey, TElement>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector, LambdaBuilder<Expression<Func<TSource, TElement>>> elementSelector){
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement>, source, keySelector, elementSelector);
        }

        public static LambdaBuilder<IQueryable<TResult>> GroupBy<TSource, TKey, TResult>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector){
            return CallStatic(Queryable.GroupBy<TSource, TKey, TResult>, source, Quote(keySelector), Quote(resultSelector));
        }
        public static LambdaBuilder<IQueryable<TResult>> GroupBy<TSource, TKey, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector, LambdaBuilder<Expression<Func<TKey, IEnumerable<TSource>, TResult>>> resultSelector){
            return CallStatic(Queryable.GroupBy<TSource, TKey, TResult>, source, keySelector, resultSelector);
        }

        public static LambdaBuilder<IQueryable<IGrouping<TKey, TElement>>> GroupBy<TSource, TKey, TElement>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement>, source, Quote(keySelector), Quote(elementSelector), comparer);
        }
        public static LambdaBuilder<IQueryable<IGrouping<TKey, TElement>>> GroupBy<TSource, TKey, TElement>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector, LambdaBuilder<Expression<Func<TSource, TElement>>> elementSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement>, source, keySelector, elementSelector, comparer);
        }

        public static LambdaBuilder<IQueryable<TResult>> GroupBy<TSource, TKey, TResult>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Queryable.GroupBy<TSource, TKey, TResult>, source, Quote(keySelector), Quote(resultSelector), comparer);
        }
        public static LambdaBuilder<IQueryable<TResult>> GroupBy<TSource, TKey, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector, LambdaBuilder<Expression<Func<TKey, IEnumerable<TSource>, TResult>>> resultSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Queryable.GroupBy<TSource, TKey, TResult>, source, keySelector, resultSelector, comparer);
        }

        public static LambdaBuilder<IQueryable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector){
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement, TResult>, source, Quote(keySelector), Quote(elementSelector), Quote(resultSelector));
        }
        public static LambdaBuilder<IQueryable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector, LambdaBuilder<Expression<Func<TSource, TElement>>> elementSelector, LambdaBuilder<Expression<Func<TKey, IEnumerable<TElement>, TResult>>> resultSelector){
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement, TResult>, source, keySelector, elementSelector, resultSelector);
        }

        public static LambdaBuilder<IQueryable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement, TResult>, source, Quote(keySelector), Quote(elementSelector), Quote(resultSelector), comparer);
        }
        public static LambdaBuilder<IQueryable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector, LambdaBuilder<Expression<Func<TSource, TElement>>> elementSelector, LambdaBuilder<Expression<Func<TKey, IEnumerable<TElement>, TResult>>> resultSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Queryable.GroupBy<TSource, TKey, TElement, TResult>, source, keySelector, elementSelector, resultSelector, comparer);
        }

        public static LambdaBuilder<IQueryable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this LambdaBuilder<IQueryable<TOuter>> outer, LambdaBuilder<IEnumerable<TInner>> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector){
            return CallStatic(Queryable.GroupJoin<TOuter, TInner, TKey, TResult>, outer, inner, Quote(outerKeySelector), Quote(innerKeySelector), Quote(resultSelector));
        }
        public static LambdaBuilder<IQueryable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this LambdaBuilder<IQueryable<TOuter>> outer, LambdaBuilder<IEnumerable<TInner>> inner, LambdaBuilder<Expression<Func<TOuter, TKey>>> outerKeySelector, LambdaBuilder<Expression<Func<TInner, TKey>>> innerKeySelector, LambdaBuilder<Expression<Func<TOuter, IEnumerable<TInner>, TResult>>> resultSelector){
            return CallStatic(Queryable.GroupJoin<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static LambdaBuilder<IQueryable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this LambdaBuilder<IQueryable<TOuter>> outer, LambdaBuilder<IEnumerable<TInner>> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Queryable.GroupJoin<TOuter, TInner, TKey, TResult>, outer, inner, Quote(outerKeySelector), Quote(innerKeySelector), Quote(resultSelector), comparer);
        }
        public static LambdaBuilder<IQueryable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this LambdaBuilder<IQueryable<TOuter>> outer, LambdaBuilder<IEnumerable<TInner>> inner, LambdaBuilder<Expression<Func<TOuter, TKey>>> outerKeySelector, LambdaBuilder<Expression<Func<TInner, TKey>>> innerKeySelector, LambdaBuilder<Expression<Func<TOuter, IEnumerable<TInner>, TResult>>> resultSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Queryable.GroupJoin<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static LambdaBuilder<IQueryable<TSource>> Intersect<TSource>(this LambdaBuilder<IQueryable<TSource>> source1, LambdaBuilder<IEnumerable<TSource>> source2){
            return CallStatic(Queryable.Intersect<TSource>, source1, source2);
        }

        public static LambdaBuilder<IQueryable<TSource>> Intersect<TSource>(this LambdaBuilder<IQueryable<TSource>> source1, LambdaBuilder<IEnumerable<TSource>> source2, LambdaBuilder<IEqualityComparer<TSource>> comparer){
            return CallStatic(Queryable.Intersect<TSource>, source1, source2, comparer);
        }

        public static LambdaBuilder<IQueryable<TResult>> Join<TOuter, TInner, TKey, TResult>(this LambdaBuilder<IQueryable<TOuter>> outer, LambdaBuilder<IEnumerable<TInner>> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector){
            return CallStatic(Queryable.Join<TOuter, TInner, TKey, TResult>, outer, inner, Quote(outerKeySelector), Quote(innerKeySelector), Quote(resultSelector));
        }
        public static LambdaBuilder<IQueryable<TResult>> Join<TOuter, TInner, TKey, TResult>(this LambdaBuilder<IQueryable<TOuter>> outer, LambdaBuilder<IEnumerable<TInner>> inner, LambdaBuilder<Expression<Func<TOuter, TKey>>> outerKeySelector, LambdaBuilder<Expression<Func<TInner, TKey>>> innerKeySelector, LambdaBuilder<Expression<Func<TOuter, TInner, TResult>>> resultSelector){
            return CallStatic(Queryable.Join<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static LambdaBuilder<IQueryable<TResult>> Join<TOuter, TInner, TKey, TResult>(this LambdaBuilder<IQueryable<TOuter>> outer, LambdaBuilder<IEnumerable<TInner>> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Queryable.Join<TOuter, TInner, TKey, TResult>, outer, inner, Quote(outerKeySelector), Quote(innerKeySelector), Quote(resultSelector), comparer);
        }
        public static LambdaBuilder<IQueryable<TResult>> Join<TOuter, TInner, TKey, TResult>(this LambdaBuilder<IQueryable<TOuter>> outer, LambdaBuilder<IEnumerable<TInner>> inner, LambdaBuilder<Expression<Func<TOuter, TKey>>> outerKeySelector, LambdaBuilder<Expression<Func<TInner, TKey>>> innerKeySelector, LambdaBuilder<Expression<Func<TOuter, TInner, TResult>>> resultSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Queryable.Join<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static LambdaBuilder<TSource> Last<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.Last<TSource>, source);
        }

        public static LambdaBuilder<TSource> Last<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.Last<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<TSource> Last<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.Last<TSource>, source, predicate);
        }

        public static LambdaBuilder<TSource> LastOrDefault<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.LastOrDefault<TSource>, source);
        }

        public static LambdaBuilder<TSource> LastOrDefault<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.LastOrDefault<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<TSource> LastOrDefault<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.LastOrDefault<TSource>, source, predicate);
        }

        public static LambdaBuilder<long> LongCount<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.LongCount<TSource>, source);
        }

        public static LambdaBuilder<long> LongCount<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.LongCount<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<long> LongCount<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.LongCount<TSource>, source, predicate);
        }

        public static LambdaBuilder<TSource> Max<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.Max<TSource>, source);
        }

        public static LambdaBuilder<TResult> Max<TSource, TResult>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TResult>> selector){
            return CallStatic(Queryable.Max<TSource, TResult>, source, Quote(selector));
        }
        public static LambdaBuilder<TResult> Max<TSource, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TResult>>> selector){
            return CallStatic(Queryable.Max<TSource, TResult>, source, selector);
        }

        public static LambdaBuilder<TSource> Min<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.Min<TSource>, source);
        }

        public static LambdaBuilder<TResult> Min<TSource, TResult>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TResult>> selector){
            return CallStatic(Queryable.Min<TSource, TResult>, source, Quote(selector));
        }
        public static LambdaBuilder<TResult> Min<TSource, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TResult>>> selector){
            return CallStatic(Queryable.Min<TSource, TResult>, source, selector);
        }

        public static LambdaBuilder<IQueryable<TResult>> OfType<TResult>(this LambdaBuilder<IQueryable> source){
            return CallStatic(Queryable.OfType<TResult>, source);
        }

        public static LambdaBuilder<IOrderedQueryable<TSource>> OrderBy<TSource, TKey>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector){
            return CallStatic(Queryable.OrderBy<TSource, TKey>, source, Quote(keySelector));
        }
        public static LambdaBuilder<IOrderedQueryable<TSource>> OrderBy<TSource, TKey>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector){
            return CallStatic(Queryable.OrderBy<TSource, TKey>, source, keySelector);
        }

        public static LambdaBuilder<IOrderedQueryable<TSource>> OrderBy<TSource, TKey>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, LambdaBuilder<IComparer<TKey>> comparer){
            return CallStatic(Queryable.OrderBy<TSource, TKey>, source, Quote(keySelector), comparer);
        }
        public static LambdaBuilder<IOrderedQueryable<TSource>> OrderBy<TSource, TKey>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector, LambdaBuilder<IComparer<TKey>> comparer){
            return CallStatic(Queryable.OrderBy<TSource, TKey>, source, keySelector, comparer);
        }

        public static LambdaBuilder<IOrderedQueryable<TSource>> OrderByDescending<TSource, TKey>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector){
            return CallStatic(Queryable.OrderByDescending<TSource, TKey>, source, Quote(keySelector));
        }
        public static LambdaBuilder<IOrderedQueryable<TSource>> OrderByDescending<TSource, TKey>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector){
            return CallStatic(Queryable.OrderByDescending<TSource, TKey>, source, keySelector);
        }

        public static LambdaBuilder<IOrderedQueryable<TSource>> OrderByDescending<TSource, TKey>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, LambdaBuilder<IComparer<TKey>> comparer){
            return CallStatic(Queryable.OrderByDescending<TSource, TKey>, source, Quote(keySelector), comparer);
        }
        public static LambdaBuilder<IOrderedQueryable<TSource>> OrderByDescending<TSource, TKey>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector, LambdaBuilder<IComparer<TKey>> comparer){
            return CallStatic(Queryable.OrderByDescending<TSource, TKey>, source, keySelector, comparer);
        }

        public static LambdaBuilder<IQueryable<TSource>> Reverse<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.Reverse<TSource>, source);
        }

        public static LambdaBuilder<IQueryable<TResult>> Select<TSource, TResult>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, TResult>> selector){
            return CallStatic(Queryable.Select<TSource, TResult>, source, Quote(selector));
        }
        public static LambdaBuilder<IQueryable<TResult>> Select<TSource, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TResult>>> selector){
            return CallStatic(Queryable.Select<TSource, TResult>, source, selector);
        }

        public static LambdaBuilder<IQueryable<TResult>> Select<TSource, TResult>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, int, TResult>> selector){
            return CallStatic(Queryable.Select<TSource, TResult>, source, Quote(selector));
        }
        public static LambdaBuilder<IQueryable<TResult>> Select<TSource, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, int, TResult>>> selector){
            return CallStatic(Queryable.Select<TSource, TResult>, source, selector);
        }

        public static LambdaBuilder<IQueryable<TResult>> SelectMany<TSource, TResult>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, IEnumerable<TResult>>> selector){
            return CallStatic(Queryable.SelectMany<TSource, TResult>, source, Quote(selector));
        }
        public static LambdaBuilder<IQueryable<TResult>> SelectMany<TSource, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, IEnumerable<TResult>>>> selector){
            return CallStatic(Queryable.SelectMany<TSource, TResult>, source, selector);
        }

        public static LambdaBuilder<IQueryable<TResult>> SelectMany<TSource, TResult>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, int, IEnumerable<TResult>>> selector){
            return CallStatic(Queryable.SelectMany<TSource, TResult>, source, Quote(selector));
        }
        public static LambdaBuilder<IQueryable<TResult>> SelectMany<TSource, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, int, IEnumerable<TResult>>>> selector){
            return CallStatic(Queryable.SelectMany<TSource, TResult>, source, selector);
        }

        public static LambdaBuilder<IQueryable<TResult>> SelectMany<TSource, TCollection, TResult>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector){
            return CallStatic(Queryable.SelectMany<TSource, TCollection, TResult>, source, Quote(collectionSelector), Quote(resultSelector));
        }
        public static LambdaBuilder<IQueryable<TResult>> SelectMany<TSource, TCollection, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, int, IEnumerable<TCollection>>>> collectionSelector, LambdaBuilder<Expression<Func<TSource, TCollection, TResult>>> resultSelector){
            return CallStatic(Queryable.SelectMany<TSource, TCollection, TResult>, source, collectionSelector, resultSelector);
        }

        public static LambdaBuilder<IQueryable<TResult>> SelectMany<TSource, TCollection, TResult>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector){
            return CallStatic(Queryable.SelectMany<TSource, TCollection, TResult>, source, Quote(collectionSelector), Quote(resultSelector));
        }
        public static LambdaBuilder<IQueryable<TResult>> SelectMany<TSource, TCollection, TResult>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, IEnumerable<TCollection>>>> collectionSelector, LambdaBuilder<Expression<Func<TSource, TCollection, TResult>>> resultSelector){
            return CallStatic(Queryable.SelectMany<TSource, TCollection, TResult>, source, collectionSelector, resultSelector);
        }

        public static LambdaBuilder<bool> SequenceEqual<TSource>(this LambdaBuilder<IQueryable<TSource>> source1, LambdaBuilder<IEnumerable<TSource>> source2){
            return CallStatic(Queryable.SequenceEqual<TSource>, source1, source2);
        }

        public static LambdaBuilder<bool> SequenceEqual<TSource>(this LambdaBuilder<IQueryable<TSource>> source1, LambdaBuilder<IEnumerable<TSource>> source2, LambdaBuilder<IEqualityComparer<TSource>> comparer){
            return CallStatic(Queryable.SequenceEqual<TSource>, source1, source2, comparer);
        }

        public static LambdaBuilder<TSource> Single<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.Single<TSource>, source);
        }

        public static LambdaBuilder<TSource> Single<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.Single<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<TSource> Single<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.Single<TSource>, source, predicate);
        }

        public static LambdaBuilder<TSource> SingleOrDefault<TSource>(this LambdaBuilder<IQueryable<TSource>> source){
            return CallStatic(Queryable.SingleOrDefault<TSource>, source);
        }

        public static LambdaBuilder<TSource> SingleOrDefault<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.SingleOrDefault<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<TSource> SingleOrDefault<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.SingleOrDefault<TSource>, source, predicate);
        }

        public static LambdaBuilder<IQueryable<TSource>> Skip<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<int> count){
            return CallStatic(Queryable.Skip<TSource>, source, count);
        }

        public static LambdaBuilder<IQueryable<TSource>> SkipWhile<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.SkipWhile<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<IQueryable<TSource>> SkipWhile<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.SkipWhile<TSource>, source, predicate);
        }

        public static LambdaBuilder<IQueryable<TSource>> SkipWhile<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, int, bool>> predicate){
            return CallStatic(Queryable.SkipWhile<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<IQueryable<TSource>> SkipWhile<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, int, bool>>> predicate){
            return CallStatic(Queryable.SkipWhile<TSource>, source, predicate);
        }

        public static LambdaBuilder<int> Sum(this LambdaBuilder<IQueryable<int>> source){
            return CallStatic(Queryable.Sum, source);
        }

        public static LambdaBuilder<int?> Sum(this LambdaBuilder<IQueryable<int?>> source){
            return CallStatic(Queryable.Sum, source);
        }

        public static LambdaBuilder<long> Sum(this LambdaBuilder<IQueryable<long>> source){
            return CallStatic(Queryable.Sum, source);
        }

        public static LambdaBuilder<long?> Sum(this LambdaBuilder<IQueryable<long?>> source){
            return CallStatic(Queryable.Sum, source);
        }

        public static LambdaBuilder<float> Sum(this LambdaBuilder<IQueryable<float>> source){
            return CallStatic(Queryable.Sum, source);
        }

        public static LambdaBuilder<float?> Sum(this LambdaBuilder<IQueryable<float?>> source){
            return CallStatic(Queryable.Sum, source);
        }

        public static LambdaBuilder<double> Sum(this LambdaBuilder<IQueryable<double>> source){
            return CallStatic(Queryable.Sum, source);
        }

        public static LambdaBuilder<double?> Sum(this LambdaBuilder<IQueryable<double?>> source){
            return CallStatic(Queryable.Sum, source);
        }

        public static LambdaBuilder<decimal> Sum(this LambdaBuilder<IQueryable<decimal>> source){
            return CallStatic(Queryable.Sum, source);
        }

        public static LambdaBuilder<decimal?> Sum(this LambdaBuilder<IQueryable<decimal?>> source){
            return CallStatic(Queryable.Sum, source);
        }

        public static LambdaBuilder<int> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, int>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<int> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, int>>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<int?> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, int?>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<int?> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, int?>>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<long> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, long>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<long> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, long>>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<long?> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, long?>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<long?> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, long?>>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<float> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, float>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<float> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, float>>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<float?> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, float?>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<float?> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, float?>>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<double> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, double>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<double> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, double>>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<double?> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, double?>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<double?> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, double?>>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<decimal> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, decimal>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<decimal> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, decimal>>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<decimal?> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, decimal?>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, Quote(selector));
        }
        public static LambdaBuilder<decimal?> Sum<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, decimal?>>> selector){
            return CallStatic(Queryable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<IQueryable<TSource>> Take<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<int> count){
            return CallStatic(Queryable.Take<TSource>, source, count);
        }

        public static LambdaBuilder<IQueryable<TSource>> TakeWhile<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.TakeWhile<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<IQueryable<TSource>> TakeWhile<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.TakeWhile<TSource>, source, predicate);
        }

        public static LambdaBuilder<IQueryable<TSource>> TakeWhile<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, int, bool>> predicate){
            return CallStatic(Queryable.TakeWhile<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<IQueryable<TSource>> TakeWhile<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, int, bool>>> predicate){
            return CallStatic(Queryable.TakeWhile<TSource>, source, predicate);
        }

        public static LambdaBuilder<IOrderedQueryable<TSource>> ThenBy<TSource, TKey>(this LambdaBuilder<IOrderedQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector){
            return CallStatic(Queryable.ThenBy<TSource, TKey>, source, Quote(keySelector));
        }
        public static LambdaBuilder<IOrderedQueryable<TSource>> ThenBy<TSource, TKey>(this LambdaBuilder<IOrderedQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector){
            return CallStatic(Queryable.ThenBy<TSource, TKey>, source, keySelector);
        }

        public static LambdaBuilder<IOrderedQueryable<TSource>> ThenBy<TSource, TKey>(this LambdaBuilder<IOrderedQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, LambdaBuilder<IComparer<TKey>> comparer){
            return CallStatic(Queryable.ThenBy<TSource, TKey>, source, Quote(keySelector), comparer);
        }
        public static LambdaBuilder<IOrderedQueryable<TSource>> ThenBy<TSource, TKey>(this LambdaBuilder<IOrderedQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector, LambdaBuilder<IComparer<TKey>> comparer){
            return CallStatic(Queryable.ThenBy<TSource, TKey>, source, keySelector, comparer);
        }

        public static LambdaBuilder<IOrderedQueryable<TSource>> ThenByDescending<TSource, TKey>(this LambdaBuilder<IOrderedQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector){
            return CallStatic(Queryable.ThenByDescending<TSource, TKey>, source, Quote(keySelector));
        }
        public static LambdaBuilder<IOrderedQueryable<TSource>> ThenByDescending<TSource, TKey>(this LambdaBuilder<IOrderedQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector){
            return CallStatic(Queryable.ThenByDescending<TSource, TKey>, source, keySelector);
        }

        public static LambdaBuilder<IOrderedQueryable<TSource>> ThenByDescending<TSource, TKey>(this LambdaBuilder<IOrderedQueryable<TSource>> source, Expression<Func<TSource, TKey>> keySelector, LambdaBuilder<IComparer<TKey>> comparer){
            return CallStatic(Queryable.ThenByDescending<TSource, TKey>, source, Quote(keySelector), comparer);
        }
        public static LambdaBuilder<IOrderedQueryable<TSource>> ThenByDescending<TSource, TKey>(this LambdaBuilder<IOrderedQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, TKey>>> keySelector, LambdaBuilder<IComparer<TKey>> comparer){
            return CallStatic(Queryable.ThenByDescending<TSource, TKey>, source, keySelector, comparer);
        }

        public static LambdaBuilder<IQueryable<TSource>> Union<TSource>(this LambdaBuilder<IQueryable<TSource>> source1, LambdaBuilder<IEnumerable<TSource>> source2){
            return CallStatic(Queryable.Union<TSource>, source1, source2);
        }

        public static LambdaBuilder<IQueryable<TSource>> Union<TSource>(this LambdaBuilder<IQueryable<TSource>> source1, LambdaBuilder<IEnumerable<TSource>> source2, LambdaBuilder<IEqualityComparer<TSource>> comparer){
            return CallStatic(Queryable.Union<TSource>, source1, source2, comparer);
        }

        public static LambdaBuilder<IQueryable<TSource>> Where<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, bool>> predicate){
            return CallStatic(Queryable.Where<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<IQueryable<TSource>> Where<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, bool>>> predicate){
            return CallStatic(Queryable.Where<TSource>, source, predicate);
        }

        public static LambdaBuilder<IQueryable<TSource>> Where<TSource>(this LambdaBuilder<IQueryable<TSource>> source, Expression<Func<TSource, int, bool>> predicate){
            return CallStatic(Queryable.Where<TSource>, source, Quote(predicate));
        }
        public static LambdaBuilder<IQueryable<TSource>> Where<TSource>(this LambdaBuilder<IQueryable<TSource>> source, LambdaBuilder<Expression<Func<TSource, int, bool>>> predicate){
            return CallStatic(Queryable.Where<TSource>, source, predicate);
        }

        public static LambdaBuilder<IQueryable<TResult>> Zip<TFirst, TSecond, TResult>(this LambdaBuilder<IQueryable<TFirst>> source1, LambdaBuilder<IEnumerable<TSecond>> source2, Expression<Func<TFirst, TSecond, TResult>> resultSelector){
            return CallStatic(Queryable.Zip<TFirst, TSecond, TResult>, source1, source2, Quote(resultSelector));
        }
        public static LambdaBuilder<IQueryable<TResult>> Zip<TFirst, TSecond, TResult>(this LambdaBuilder<IQueryable<TFirst>> source1, LambdaBuilder<IEnumerable<TSecond>> source2, LambdaBuilder<Expression<Func<TFirst, TSecond, TResult>>> resultSelector){
            return CallStatic(Queryable.Zip<TFirst, TSecond, TResult>, source1, source2, resultSelector);
        }

    }
}