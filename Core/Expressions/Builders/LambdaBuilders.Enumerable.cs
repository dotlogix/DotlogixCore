// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  LambdaBuilders.Enumerable.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 09.06.2021 00:21
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotLogix.Core.Expressions {
    public static partial class LambdaBuilders {
        public static LambdaBuilder<TSource> Aggregate<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TSource, TSource>> func){
            return CallStatic(Enumerable.Aggregate<TSource>, source, func);
        }

        public static LambdaBuilder<TAccumulate> Aggregate<TSource, TAccumulate>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<TAccumulate> seed, LambdaBuilder<Func<TAccumulate, TSource, TAccumulate>> func){
            return CallStatic(Enumerable.Aggregate<TSource, TAccumulate>, source, seed, func);
        }

        public static LambdaBuilder<TResult> Aggregate<TSource, TAccumulate, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<TAccumulate> seed, LambdaBuilder<Func<TAccumulate, TSource, TAccumulate>> func, LambdaBuilder<Func<TAccumulate, TResult>> resultSelector){
            return CallStatic(Enumerable.Aggregate<TSource, TAccumulate, TResult>, source, seed, func, resultSelector);
        }

        public static LambdaBuilder<bool> All<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.All<TSource>, source, predicate);
        }

        public static LambdaBuilder<bool> Any<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.Any<TSource>, source);
        }

        public static LambdaBuilder<bool> Any<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.Any<TSource>, source, predicate);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Append<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<TSource> element){
            return CallStatic(Enumerable.Append<TSource>, source, element);
        }

        public static LambdaBuilder<IEnumerable<TSource>> AsEnumerable<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.AsEnumerable<TSource>, source);
        }

        public static LambdaBuilder<double> Average(this LambdaBuilder<IEnumerable<int>> source){
            return CallStatic(Enumerable.Average, source);
        }

        public static LambdaBuilder<double> Average(this LambdaBuilder<IEnumerable<long>> source){
            return CallStatic(Enumerable.Average, source);
        }

        public static LambdaBuilder<float> Average(this LambdaBuilder<IEnumerable<float>> source){
            return CallStatic(Enumerable.Average, source);
        }

        public static LambdaBuilder<double> Average(this LambdaBuilder<IEnumerable<double>> source){
            return CallStatic(Enumerable.Average, source);
        }

        public static LambdaBuilder<decimal> Average(this LambdaBuilder<IEnumerable<decimal>> source){
            return CallStatic(Enumerable.Average, source);
        }

        public static LambdaBuilder<double?> Average(this LambdaBuilder<IEnumerable<int?>> source){
            return CallStatic(Enumerable.Average, source);
        }

        public static LambdaBuilder<double?> Average(this LambdaBuilder<IEnumerable<long?>> source){
            return CallStatic(Enumerable.Average, source);
        }

        public static LambdaBuilder<float?> Average(this LambdaBuilder<IEnumerable<float?>> source){
            return CallStatic(Enumerable.Average, source);
        }

        public static LambdaBuilder<double?> Average(this LambdaBuilder<IEnumerable<double?>> source){
            return CallStatic(Enumerable.Average, source);
        }

        public static LambdaBuilder<decimal?> Average(this LambdaBuilder<IEnumerable<decimal?>> source){
            return CallStatic(Enumerable.Average, source);
        }

        public static LambdaBuilder<double> Average<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int>> selector){
            return CallStatic(Enumerable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<double?> Average<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int?>> selector){
            return CallStatic(Enumerable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<double> Average<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, long>> selector){
            return CallStatic(Enumerable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<double?> Average<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, long?>> selector){
            return CallStatic(Enumerable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<float> Average<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, float>> selector){
            return CallStatic(Enumerable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<float?> Average<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, float?>> selector){
            return CallStatic(Enumerable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<double> Average<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, double>> selector){
            return CallStatic(Enumerable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<double?> Average<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, double?>> selector){
            return CallStatic(Enumerable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<decimal> Average<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, decimal>> selector){
            return CallStatic(Enumerable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<decimal?> Average<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, decimal?>> selector){
            return CallStatic(Enumerable.Average<TSource>, source, selector);
        }

        public static LambdaBuilder<IEnumerable<TResult>> Cast<TResult>(this LambdaBuilder<IEnumerable> source){
            return CallStatic(Enumerable.Cast<TResult>, source);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Concat<TSource>(this LambdaBuilder<IEnumerable<TSource>> first, LambdaBuilder<IEnumerable<TSource>> second){
            return CallStatic(Enumerable.Concat<TSource>, first, second);
        }

        public static LambdaBuilder<bool> Contains<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<TSource> value){
            return CallStatic(Enumerable.Contains<TSource>, source, value);
        }

        public static LambdaBuilder<bool> Contains<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<TSource> value, LambdaBuilder<IEqualityComparer<TSource>> comparer){
            return CallStatic(Enumerable.Contains<TSource>, source, value, comparer);
        }

        public static LambdaBuilder<int> Count<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.Count<TSource>, source);
        }

        public static LambdaBuilder<int> Count<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.Count<TSource>, source, predicate);
        }

        public static LambdaBuilder<IEnumerable<TSource>> DefaultIfEmpty<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.DefaultIfEmpty<TSource>, source);
        }

        public static LambdaBuilder<IEnumerable<TSource>> DefaultIfEmpty<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<TSource> defaultValue){
            return CallStatic(Enumerable.DefaultIfEmpty<TSource>, source, defaultValue);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Distinct<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.Distinct<TSource>, source);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Distinct<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<IEqualityComparer<TSource>> comparer){
            return CallStatic(Enumerable.Distinct<TSource>, source, comparer);
        }

        public static LambdaBuilder<TSource> ElementAt<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<int> index){
            return CallStatic(Enumerable.ElementAt<TSource>, source, index);
        }

        public static LambdaBuilder<TSource> ElementAtOrDefault<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<int> index){
            return CallStatic(Enumerable.ElementAtOrDefault<TSource>, source, index);
        }

        public static LambdaBuilder<IEnumerable<TResult>> Empty<TResult>(){
            return CallStatic(Enumerable.Empty<TResult>);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Except<TSource>(this LambdaBuilder<IEnumerable<TSource>> first, LambdaBuilder<IEnumerable<TSource>> second){
            return CallStatic(Enumerable.Except<TSource>, first, second);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Except<TSource>(this LambdaBuilder<IEnumerable<TSource>> first, LambdaBuilder<IEnumerable<TSource>> second, LambdaBuilder<IEqualityComparer<TSource>> comparer){
            return CallStatic(Enumerable.Except<TSource>, first, second, comparer);
        }

        public static LambdaBuilder<TSource> First<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.First<TSource>, source);
        }

        public static LambdaBuilder<TSource> First<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.First<TSource>, source, predicate);
        }

        public static LambdaBuilder<TSource> FirstOrDefault<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.FirstOrDefault<TSource>, source);
        }

        public static LambdaBuilder<TSource> FirstOrDefault<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.FirstOrDefault<TSource>, source, predicate);
        }

        public static LambdaBuilder<IEnumerable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector){
            return CallStatic(Enumerable.GroupBy<TSource, TKey>, source, keySelector);
        }

        public static LambdaBuilder<IEnumerable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Enumerable.GroupBy<TSource, TKey>, source, keySelector, comparer);
        }

        public static LambdaBuilder<IEnumerable<IGrouping<TKey, TElement>>> GroupBy<TSource, TKey, TElement>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<Func<TSource, TElement>> elementSelector){
            return CallStatic(Enumerable.GroupBy<TSource, TKey, TElement>, source, keySelector, elementSelector);
        }

        public static LambdaBuilder<IEnumerable<TResult>> GroupBy<TSource, TKey, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector){
            return CallStatic(Enumerable.GroupBy<TSource, TKey, TResult>, source, keySelector, resultSelector);
        }

        public static LambdaBuilder<IEnumerable<IGrouping<TKey, TElement>>> GroupBy<TSource, TKey, TElement>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<Func<TSource, TElement>> elementSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Enumerable.GroupBy<TSource, TKey, TElement>, source, keySelector, elementSelector, comparer);
        }

        public static LambdaBuilder<IEnumerable<TResult>> GroupBy<TSource, TKey, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Enumerable.GroupBy<TSource, TKey, TResult>, source, keySelector, resultSelector, comparer);
        }

        public static LambdaBuilder<IEnumerable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<Func<TSource, TElement>> elementSelector, LambdaBuilder<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector){
            return CallStatic(Enumerable.GroupBy<TSource, TKey, TElement, TResult>, source, keySelector, elementSelector, resultSelector);
        }

        public static LambdaBuilder<IEnumerable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<Func<TSource, TElement>> elementSelector, LambdaBuilder<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Enumerable.GroupBy<TSource, TKey, TElement, TResult>, source, keySelector, elementSelector, resultSelector, comparer);
        }

        public static LambdaBuilder<IEnumerable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this LambdaBuilder<IEnumerable<TOuter>> outer, LambdaBuilder<IEnumerable<TInner>> inner, LambdaBuilder<Func<TOuter, TKey>> outerKeySelector, LambdaBuilder<Func<TInner, TKey>> innerKeySelector, LambdaBuilder<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector){
            return CallStatic(Enumerable.GroupJoin<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static LambdaBuilder<IEnumerable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this LambdaBuilder<IEnumerable<TOuter>> outer, LambdaBuilder<IEnumerable<TInner>> inner, LambdaBuilder<Func<TOuter, TKey>> outerKeySelector, LambdaBuilder<Func<TInner, TKey>> innerKeySelector, LambdaBuilder<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Enumerable.GroupJoin<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Intersect<TSource>(this LambdaBuilder<IEnumerable<TSource>> first, LambdaBuilder<IEnumerable<TSource>> second){
            return CallStatic(Enumerable.Intersect<TSource>, first, second);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Intersect<TSource>(this LambdaBuilder<IEnumerable<TSource>> first, LambdaBuilder<IEnumerable<TSource>> second, LambdaBuilder<IEqualityComparer<TSource>> comparer){
            return CallStatic(Enumerable.Intersect<TSource>, first, second, comparer);
        }

        public static LambdaBuilder<IEnumerable<TResult>> Join<TOuter, TInner, TKey, TResult>(this LambdaBuilder<IEnumerable<TOuter>> outer, LambdaBuilder<IEnumerable<TInner>> inner, LambdaBuilder<Func<TOuter, TKey>> outerKeySelector, LambdaBuilder<Func<TInner, TKey>> innerKeySelector, LambdaBuilder<Func<TOuter, TInner, TResult>> resultSelector){
            return CallStatic(Enumerable.Join<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static LambdaBuilder<IEnumerable<TResult>> Join<TOuter, TInner, TKey, TResult>(this LambdaBuilder<IEnumerable<TOuter>> outer, LambdaBuilder<IEnumerable<TInner>> inner, LambdaBuilder<Func<TOuter, TKey>> outerKeySelector, LambdaBuilder<Func<TInner, TKey>> innerKeySelector, LambdaBuilder<Func<TOuter, TInner, TResult>> resultSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Enumerable.Join<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static LambdaBuilder<TSource> Last<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.Last<TSource>, source);
        }

        public static LambdaBuilder<TSource> Last<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.Last<TSource>, source, predicate);
        }

        public static LambdaBuilder<TSource> LastOrDefault<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.LastOrDefault<TSource>, source);
        }

        public static LambdaBuilder<TSource> LastOrDefault<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.LastOrDefault<TSource>, source, predicate);
        }

        public static LambdaBuilder<long> LongCount<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.LongCount<TSource>, source);
        }

        public static LambdaBuilder<long> LongCount<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.LongCount<TSource>, source, predicate);
        }

        public static LambdaBuilder<int> Max(this LambdaBuilder<IEnumerable<int>> source){
            return CallStatic(Enumerable.Max, source);
        }

        public static LambdaBuilder<long> Max(this LambdaBuilder<IEnumerable<long>> source){
            return CallStatic(Enumerable.Max, source);
        }

        public static LambdaBuilder<double> Max(this LambdaBuilder<IEnumerable<double>> source){
            return CallStatic(Enumerable.Max, source);
        }

        public static LambdaBuilder<float> Max(this LambdaBuilder<IEnumerable<float>> source){
            return CallStatic(Enumerable.Max, source);
        }

        public static LambdaBuilder<decimal> Max(this LambdaBuilder<IEnumerable<decimal>> source){
            return CallStatic(Enumerable.Max, source);
        }

        public static LambdaBuilder<int?> Max(this LambdaBuilder<IEnumerable<int?>> source){
            return CallStatic(Enumerable.Max, source);
        }

        public static LambdaBuilder<long?> Max(this LambdaBuilder<IEnumerable<long?>> source){
            return CallStatic(Enumerable.Max, source);
        }

        public static LambdaBuilder<double?> Max(this LambdaBuilder<IEnumerable<double?>> source){
            return CallStatic(Enumerable.Max, source);
        }

        public static LambdaBuilder<float?> Max(this LambdaBuilder<IEnumerable<float?>> source){
            return CallStatic(Enumerable.Max, source);
        }

        public static LambdaBuilder<decimal?> Max(this LambdaBuilder<IEnumerable<decimal?>> source){
            return CallStatic(Enumerable.Max, source);
        }

        public static LambdaBuilder<TSource> Max<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.Max<TSource>, source);
        }

        public static LambdaBuilder<int> Max<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int>> selector){
            return CallStatic(Enumerable.Max<TSource>, source, selector);
        }

        public static LambdaBuilder<int?> Max<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int?>> selector){
            return CallStatic(Enumerable.Max<TSource>, source, selector);
        }

        public static LambdaBuilder<long> Max<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, long>> selector){
            return CallStatic(Enumerable.Max<TSource>, source, selector);
        }

        public static LambdaBuilder<long?> Max<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, long?>> selector){
            return CallStatic(Enumerable.Max<TSource>, source, selector);
        }

        public static LambdaBuilder<float> Max<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, float>> selector){
            return CallStatic(Enumerable.Max<TSource>, source, selector);
        }

        public static LambdaBuilder<float?> Max<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, float?>> selector){
            return CallStatic(Enumerable.Max<TSource>, source, selector);
        }

        public static LambdaBuilder<double> Max<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, double>> selector){
            return CallStatic(Enumerable.Max<TSource>, source, selector);
        }

        public static LambdaBuilder<double?> Max<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, double?>> selector){
            return CallStatic(Enumerable.Max<TSource>, source, selector);
        }

        public static LambdaBuilder<decimal> Max<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, decimal>> selector){
            return CallStatic(Enumerable.Max<TSource>, source, selector);
        }

        public static LambdaBuilder<decimal?> Max<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, decimal?>> selector){
            return CallStatic(Enumerable.Max<TSource>, source, selector);
        }

        public static LambdaBuilder<TResult> Max<TSource, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TResult>> selector){
            return CallStatic(Enumerable.Max<TSource, TResult>, source, selector);
        }

        public static LambdaBuilder<int> Min(this LambdaBuilder<IEnumerable<int>> source){
            return CallStatic(Enumerable.Min, source);
        }

        public static LambdaBuilder<long> Min(this LambdaBuilder<IEnumerable<long>> source){
            return CallStatic(Enumerable.Min, source);
        }

        public static LambdaBuilder<float> Min(this LambdaBuilder<IEnumerable<float>> source){
            return CallStatic(Enumerable.Min, source);
        }

        public static LambdaBuilder<double> Min(this LambdaBuilder<IEnumerable<double>> source){
            return CallStatic(Enumerable.Min, source);
        }

        public static LambdaBuilder<decimal> Min(this LambdaBuilder<IEnumerable<decimal>> source){
            return CallStatic(Enumerable.Min, source);
        }

        public static LambdaBuilder<int?> Min(this LambdaBuilder<IEnumerable<int?>> source){
            return CallStatic(Enumerable.Min, source);
        }

        public static LambdaBuilder<long?> Min(this LambdaBuilder<IEnumerable<long?>> source){
            return CallStatic(Enumerable.Min, source);
        }

        public static LambdaBuilder<float?> Min(this LambdaBuilder<IEnumerable<float?>> source){
            return CallStatic(Enumerable.Min, source);
        }

        public static LambdaBuilder<double?> Min(this LambdaBuilder<IEnumerable<double?>> source){
            return CallStatic(Enumerable.Min, source);
        }

        public static LambdaBuilder<decimal?> Min(this LambdaBuilder<IEnumerable<decimal?>> source){
            return CallStatic(Enumerable.Min, source);
        }

        public static LambdaBuilder<TSource> Min<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.Min<TSource>, source);
        }

        public static LambdaBuilder<int> Min<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int>> selector){
            return CallStatic(Enumerable.Min<TSource>, source, selector);
        }

        public static LambdaBuilder<int?> Min<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int?>> selector){
            return CallStatic(Enumerable.Min<TSource>, source, selector);
        }

        public static LambdaBuilder<long> Min<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, long>> selector){
            return CallStatic(Enumerable.Min<TSource>, source, selector);
        }

        public static LambdaBuilder<long?> Min<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, long?>> selector){
            return CallStatic(Enumerable.Min<TSource>, source, selector);
        }

        public static LambdaBuilder<float> Min<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, float>> selector){
            return CallStatic(Enumerable.Min<TSource>, source, selector);
        }

        public static LambdaBuilder<float?> Min<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, float?>> selector){
            return CallStatic(Enumerable.Min<TSource>, source, selector);
        }

        public static LambdaBuilder<double> Min<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, double>> selector){
            return CallStatic(Enumerable.Min<TSource>, source, selector);
        }

        public static LambdaBuilder<double?> Min<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, double?>> selector){
            return CallStatic(Enumerable.Min<TSource>, source, selector);
        }

        public static LambdaBuilder<decimal> Min<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, decimal>> selector){
            return CallStatic(Enumerable.Min<TSource>, source, selector);
        }

        public static LambdaBuilder<decimal?> Min<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, decimal?>> selector){
            return CallStatic(Enumerable.Min<TSource>, source, selector);
        }

        public static LambdaBuilder<TResult> Min<TSource, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TResult>> selector){
            return CallStatic(Enumerable.Min<TSource, TResult>, source, selector);
        }

        public static LambdaBuilder<IEnumerable<TResult>> OfType<TResult>(this LambdaBuilder<IEnumerable> source){
            return CallStatic(Enumerable.OfType<TResult>, source);
        }

        public static LambdaBuilder<IOrderedEnumerable<TSource>> OrderBy<TSource, TKey>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector){
            return CallStatic(Enumerable.OrderBy<TSource, TKey>, source, keySelector);
        }

        public static LambdaBuilder<IOrderedEnumerable<TSource>> OrderBy<TSource, TKey>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<IComparer<TKey>> comparer){
            return CallStatic(Enumerable.OrderBy<TSource, TKey>, source, keySelector, comparer);
        }

        public static LambdaBuilder<IOrderedEnumerable<TSource>> OrderByDescending<TSource, TKey>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector){
            return CallStatic(Enumerable.OrderByDescending<TSource, TKey>, source, keySelector);
        }

        public static LambdaBuilder<IOrderedEnumerable<TSource>> OrderByDescending<TSource, TKey>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<IComparer<TKey>> comparer){
            return CallStatic(Enumerable.OrderByDescending<TSource, TKey>, source, keySelector, comparer);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Prepend<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<TSource> element){
            return CallStatic(Enumerable.Prepend<TSource>, source, element);
        }

        public static LambdaBuilder<IEnumerable<int>> Range(this LambdaBuilder<int> start, LambdaBuilder<int> count){
            return CallStatic(Enumerable.Range, start, count);
        }

        public static LambdaBuilder<IEnumerable<TResult>> Repeat<TResult>(this LambdaBuilder<TResult> element, LambdaBuilder<int> count){
            return CallStatic(Enumerable.Repeat<TResult>, element, count);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Reverse<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.Reverse<TSource>, source);
        }

        public static LambdaBuilder<IEnumerable<TResult>> Select<TSource, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TResult>> selector){
            return CallStatic(Enumerable.Select<TSource, TResult>, source, selector);
        }

        public static LambdaBuilder<IEnumerable<TResult>> Select<TSource, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int, TResult>> selector){
            return CallStatic(Enumerable.Select<TSource, TResult>, source, selector);
        }

        public static LambdaBuilder<IEnumerable<TResult>> SelectMany<TSource, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, IEnumerable<TResult>>> selector){
            return CallStatic(Enumerable.SelectMany<TSource, TResult>, source, selector);
        }

        public static LambdaBuilder<IEnumerable<TResult>> SelectMany<TSource, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int, IEnumerable<TResult>>> selector){
            return CallStatic(Enumerable.SelectMany<TSource, TResult>, source, selector);
        }

        public static LambdaBuilder<IEnumerable<TResult>> SelectMany<TSource, TCollection, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector, LambdaBuilder<Func<TSource, TCollection, TResult>> resultSelector){
            return CallStatic(Enumerable.SelectMany<TSource, TCollection, TResult>, source, collectionSelector, resultSelector);
        }

        public static LambdaBuilder<IEnumerable<TResult>> SelectMany<TSource, TCollection, TResult>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, IEnumerable<TCollection>>> collectionSelector, LambdaBuilder<Func<TSource, TCollection, TResult>> resultSelector){
            return CallStatic(Enumerable.SelectMany<TSource, TCollection, TResult>, source, collectionSelector, resultSelector);
        }

        public static LambdaBuilder<bool> SequenceEqual<TSource>(this LambdaBuilder<IEnumerable<TSource>> first, LambdaBuilder<IEnumerable<TSource>> second){
            return CallStatic(Enumerable.SequenceEqual<TSource>, first, second);
        }

        public static LambdaBuilder<bool> SequenceEqual<TSource>(this LambdaBuilder<IEnumerable<TSource>> first, LambdaBuilder<IEnumerable<TSource>> second, LambdaBuilder<IEqualityComparer<TSource>> comparer){
            return CallStatic(Enumerable.SequenceEqual<TSource>, first, second, comparer);
        }

        public static LambdaBuilder<TSource> Single<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.Single<TSource>, source);
        }

        public static LambdaBuilder<TSource> Single<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.Single<TSource>, source, predicate);
        }

        public static LambdaBuilder<TSource> SingleOrDefault<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.SingleOrDefault<TSource>, source);
        }

        public static LambdaBuilder<TSource> SingleOrDefault<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.SingleOrDefault<TSource>, source, predicate);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Skip<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<int> count){
            return CallStatic(Enumerable.Skip<TSource>, source, count);
        }

        public static LambdaBuilder<IEnumerable<TSource>> SkipWhile<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.SkipWhile<TSource>, source, predicate);
        }

        public static LambdaBuilder<IEnumerable<TSource>> SkipWhile<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int, bool>> predicate){
            return CallStatic(Enumerable.SkipWhile<TSource>, source, predicate);
        }

        public static LambdaBuilder<int> Sum(this LambdaBuilder<IEnumerable<int>> source){
            return CallStatic(Enumerable.Sum, source);
        }

        public static LambdaBuilder<long> Sum(this LambdaBuilder<IEnumerable<long>> source){
            return CallStatic(Enumerable.Sum, source);
        }

        public static LambdaBuilder<float> Sum(this LambdaBuilder<IEnumerable<float>> source){
            return CallStatic(Enumerable.Sum, source);
        }

        public static LambdaBuilder<double> Sum(this LambdaBuilder<IEnumerable<double>> source){
            return CallStatic(Enumerable.Sum, source);
        }

        public static LambdaBuilder<decimal> Sum(this LambdaBuilder<IEnumerable<decimal>> source){
            return CallStatic(Enumerable.Sum, source);
        }

        public static LambdaBuilder<int?> Sum(this LambdaBuilder<IEnumerable<int?>> source){
            return CallStatic(Enumerable.Sum, source);
        }

        public static LambdaBuilder<long?> Sum(this LambdaBuilder<IEnumerable<long?>> source){
            return CallStatic(Enumerable.Sum, source);
        }

        public static LambdaBuilder<float?> Sum(this LambdaBuilder<IEnumerable<float?>> source){
            return CallStatic(Enumerable.Sum, source);
        }

        public static LambdaBuilder<double?> Sum(this LambdaBuilder<IEnumerable<double?>> source){
            return CallStatic(Enumerable.Sum, source);
        }

        public static LambdaBuilder<decimal?> Sum(this LambdaBuilder<IEnumerable<decimal?>> source){
            return CallStatic(Enumerable.Sum, source);
        }

        public static LambdaBuilder<int> Sum<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int>> selector){
            return CallStatic(Enumerable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<int?> Sum<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int?>> selector){
            return CallStatic(Enumerable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<long> Sum<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, long>> selector){
            return CallStatic(Enumerable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<long?> Sum<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, long?>> selector){
            return CallStatic(Enumerable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<float> Sum<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, float>> selector){
            return CallStatic(Enumerable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<float?> Sum<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, float?>> selector){
            return CallStatic(Enumerable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<double> Sum<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, double>> selector){
            return CallStatic(Enumerable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<double?> Sum<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, double?>> selector){
            return CallStatic(Enumerable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<decimal> Sum<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, decimal>> selector){
            return CallStatic(Enumerable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<decimal?> Sum<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, decimal?>> selector){
            return CallStatic(Enumerable.Sum<TSource>, source, selector);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Take<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<int> count){
            return CallStatic(Enumerable.Take<TSource>, source, count);
        }

        public static LambdaBuilder<IEnumerable<TSource>> TakeWhile<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.TakeWhile<TSource>, source, predicate);
        }

        public static LambdaBuilder<IEnumerable<TSource>> TakeWhile<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int, bool>> predicate){
            return CallStatic(Enumerable.TakeWhile<TSource>, source, predicate);
        }

        public static LambdaBuilder<IOrderedEnumerable<TSource>> ThenBy<TSource, TKey>(this LambdaBuilder<IOrderedEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector){
            return CallStatic(Enumerable.ThenBy<TSource, TKey>, source, keySelector);
        }

        public static LambdaBuilder<IOrderedEnumerable<TSource>> ThenBy<TSource, TKey>(this LambdaBuilder<IOrderedEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<IComparer<TKey>> comparer){
            return CallStatic(Enumerable.ThenBy<TSource, TKey>, source, keySelector, comparer);
        }

        public static LambdaBuilder<IOrderedEnumerable<TSource>> ThenByDescending<TSource, TKey>(this LambdaBuilder<IOrderedEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector){
            return CallStatic(Enumerable.ThenByDescending<TSource, TKey>, source, keySelector);
        }

        public static LambdaBuilder<IOrderedEnumerable<TSource>> ThenByDescending<TSource, TKey>(this LambdaBuilder<IOrderedEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<IComparer<TKey>> comparer){
            return CallStatic(Enumerable.ThenByDescending<TSource, TKey>, source, keySelector, comparer);
        }

        public static LambdaBuilder<TSource[]> ToArray<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.ToArray<TSource>, source);
        }

        public static LambdaBuilder<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector){
            return CallStatic(Enumerable.ToDictionary<TSource, TKey>, source, keySelector);
        }

        public static LambdaBuilder<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Enumerable.ToDictionary<TSource, TKey>, source, keySelector, comparer);
        }

        public static LambdaBuilder<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<Func<TSource, TElement>> elementSelector){
            return CallStatic(Enumerable.ToDictionary<TSource, TKey, TElement>, source, keySelector, elementSelector);
        }

        public static LambdaBuilder<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<Func<TSource, TElement>> elementSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Enumerable.ToDictionary<TSource, TKey, TElement>, source, keySelector, elementSelector, comparer);
        }

        public static LambdaBuilder<List<TSource>> ToList<TSource>(this LambdaBuilder<IEnumerable<TSource>> source){
            return CallStatic(Enumerable.ToList<TSource>, source);
        }

        public static LambdaBuilder<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector){
            return CallStatic(Enumerable.ToLookup<TSource, TKey>, source, keySelector);
        }

        public static LambdaBuilder<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Enumerable.ToLookup<TSource, TKey>, source, keySelector, comparer);
        }

        public static LambdaBuilder<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<Func<TSource, TElement>> elementSelector){
            return CallStatic(Enumerable.ToLookup<TSource, TKey, TElement>, source, keySelector, elementSelector);
        }

        public static LambdaBuilder<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, TKey>> keySelector, LambdaBuilder<Func<TSource, TElement>> elementSelector, LambdaBuilder<IEqualityComparer<TKey>> comparer){
            return CallStatic(Enumerable.ToLookup<TSource, TKey, TElement>, source, keySelector, elementSelector, comparer);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Union<TSource>(this LambdaBuilder<IEnumerable<TSource>> first, LambdaBuilder<IEnumerable<TSource>> second){
            return CallStatic(Enumerable.Union<TSource>, first, second);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Union<TSource>(this LambdaBuilder<IEnumerable<TSource>> first, LambdaBuilder<IEnumerable<TSource>> second, LambdaBuilder<IEqualityComparer<TSource>> comparer){
            return CallStatic(Enumerable.Union<TSource>, first, second, comparer);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Where<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, bool>> predicate){
            return CallStatic(Enumerable.Where<TSource>, source, predicate);
        }

        public static LambdaBuilder<IEnumerable<TSource>> Where<TSource>(this LambdaBuilder<IEnumerable<TSource>> source, LambdaBuilder<Func<TSource, int, bool>> predicate){
            return CallStatic(Enumerable.Where<TSource>, source, predicate);
        }

        public static LambdaBuilder<IEnumerable<TResult>> Zip<TFirst, TSecond, TResult>(this LambdaBuilder<IEnumerable<TFirst>> first, LambdaBuilder<IEnumerable<TSecond>> second, LambdaBuilder<Func<TFirst, TSecond, TResult>> resultSelector){
            return CallStatic(Enumerable.Zip<TFirst, TSecond, TResult>, first, second, resultSelector);
        }

    }
}