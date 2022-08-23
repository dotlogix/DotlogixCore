

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DotLogix.Core.Expressions; 

[SuppressMessage("ReSharper", "RedundantTypeArgumentsOfMethod")]
public static partial class Lambdas {
    /// <inheritdoc cref="System.Linq.Enumerable.Aggregate{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TSource, TSource})"/>
    public static Lambda<TSource> Aggregate<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TSource, TSource>> func) {
        return CallStatic(Enumerable.Aggregate<TSource>, source, func);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Aggregate{TSource, TAccumulate}(System.Collections.Generic.IEnumerable{TSource}, TAccumulate, System.Func{TAccumulate, TSource, TAccumulate})"/>
    public static Lambda<TAccumulate> Aggregate<TSource, TAccumulate>(this Lambda<IEnumerable<TSource>> source, Lambda<TAccumulate> seed, Lambda<Func<TAccumulate, TSource, TAccumulate>> func) {
        return CallStatic(Enumerable.Aggregate<TSource, TAccumulate>, source, seed, func);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Aggregate{TSource, TAccumulate, TResult}(System.Collections.Generic.IEnumerable{TSource}, TAccumulate, System.Func{TAccumulate, TSource, TAccumulate}, System.Func{TAccumulate, TResult})"/>
    public static Lambda<TResult> Aggregate<TSource, TAccumulate, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<TAccumulate> seed, Lambda<Func<TAccumulate, TSource, TAccumulate>> func, Lambda<Func<TAccumulate, TResult>> resultSelector) {
        return CallStatic(Enumerable.Aggregate<TSource, TAccumulate, TResult>, source, seed, func, resultSelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.All{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<bool> All<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.All<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Any{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<bool> Any<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.Any<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Any{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<bool> Any<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.Any<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Append{TSource}(System.Collections.Generic.IEnumerable{TSource}, TSource)"/>
    public static Lambda<IEnumerable<TSource>> Append<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<TSource> element) {
        return CallStatic(Enumerable.Append<TSource>, source, element);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.AsEnumerable{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<IEnumerable<TSource>> AsEnumerable<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.AsEnumerable<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average(System.Collections.Generic.IEnumerable{int})"/>
    public static Lambda<double> Average(this Lambda<IEnumerable<int>> source) {
        return CallStatic(Enumerable.Average, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average(System.Collections.Generic.IEnumerable{long})"/>
    public static Lambda<double> Average(this Lambda<IEnumerable<long>> source) {
        return CallStatic(Enumerable.Average, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average(System.Collections.Generic.IEnumerable{float})"/>
    public static Lambda<float> Average(this Lambda<IEnumerable<float>> source) {
        return CallStatic(Enumerable.Average, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average(System.Collections.Generic.IEnumerable{double})"/>
    public static Lambda<double> Average(this Lambda<IEnumerable<double>> source) {
        return CallStatic(Enumerable.Average, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average(System.Collections.Generic.IEnumerable{decimal})"/>
    public static Lambda<decimal> Average(this Lambda<IEnumerable<decimal>> source) {
        return CallStatic(Enumerable.Average, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average(System.Collections.Generic.IEnumerable{int?})"/>
    public static Lambda<double?> Average(this Lambda<IEnumerable<int?>> source) {
        return CallStatic(Enumerable.Average, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average(System.Collections.Generic.IEnumerable{long?})"/>
    public static Lambda<double?> Average(this Lambda<IEnumerable<long?>> source) {
        return CallStatic(Enumerable.Average, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average(System.Collections.Generic.IEnumerable{float?})"/>
    public static Lambda<float?> Average(this Lambda<IEnumerable<float?>> source) {
        return CallStatic(Enumerable.Average, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average(System.Collections.Generic.IEnumerable{double?})"/>
    public static Lambda<double?> Average(this Lambda<IEnumerable<double?>> source) {
        return CallStatic(Enumerable.Average, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average(System.Collections.Generic.IEnumerable{decimal?})"/>
    public static Lambda<decimal?> Average(this Lambda<IEnumerable<decimal?>> source) {
        return CallStatic(Enumerable.Average, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int})"/>
    public static Lambda<double> Average<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int>> selector) {
        return CallStatic(Enumerable.Average<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int?})"/>
    public static Lambda<double?> Average<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int?>> selector) {
        return CallStatic(Enumerable.Average<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, long})"/>
    public static Lambda<double> Average<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, long>> selector) {
        return CallStatic(Enumerable.Average<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, long?})"/>
    public static Lambda<double?> Average<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, long?>> selector) {
        return CallStatic(Enumerable.Average<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, float})"/>
    public static Lambda<float> Average<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, float>> selector) {
        return CallStatic(Enumerable.Average<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, float?})"/>
    public static Lambda<float?> Average<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, float?>> selector) {
        return CallStatic(Enumerable.Average<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, double})"/>
    public static Lambda<double> Average<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, double>> selector) {
        return CallStatic(Enumerable.Average<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, double?})"/>
    public static Lambda<double?> Average<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, double?>> selector) {
        return CallStatic(Enumerable.Average<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, decimal})"/>
    public static Lambda<decimal> Average<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, decimal>> selector) {
        return CallStatic(Enumerable.Average<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Average{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, decimal?})"/>
    public static Lambda<decimal?> Average<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, decimal?>> selector) {
        return CallStatic(Enumerable.Average<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Cast{TResult}(System.Collections.IEnumerable)"/>
    public static Lambda<IEnumerable<TResult>> Cast<TResult>(this Lambda<IEnumerable> source) {
        return CallStatic(Enumerable.Cast<TResult>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Concat{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<IEnumerable<TSource>> Concat<TSource>(this Lambda<IEnumerable<TSource>> first, Lambda<IEnumerable<TSource>> second) {
        return CallStatic(Enumerable.Concat<TSource>, first, second);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Contains{TSource}(System.Collections.Generic.IEnumerable{TSource}, TSource)"/>
    public static Lambda<bool> Contains<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<TSource> value) {
        return CallStatic(Enumerable.Contains<TSource>, source, value);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Contains{TSource}(System.Collections.Generic.IEnumerable{TSource}, TSource, System.Collections.Generic.IEqualityComparer{TSource})"/>
    public static Lambda<bool> Contains<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<TSource> value, Lambda<IEqualityComparer<TSource>> comparer) {
        return CallStatic(Enumerable.Contains<TSource>, source, value, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Count{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<int> Count<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.Count<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Count{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<int> Count<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.Count<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.DefaultIfEmpty{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<IEnumerable<TSource>> DefaultIfEmpty<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.DefaultIfEmpty<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.DefaultIfEmpty{TSource}(System.Collections.Generic.IEnumerable{TSource}, TSource)"/>
    public static Lambda<IEnumerable<TSource>> DefaultIfEmpty<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<TSource> defaultValue) {
        return CallStatic(Enumerable.DefaultIfEmpty<TSource>, source, defaultValue);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Distinct{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<IEnumerable<TSource>> Distinct<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.Distinct<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Distinct{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEqualityComparer{TSource})"/>
    public static Lambda<IEnumerable<TSource>> Distinct<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<IEqualityComparer<TSource>> comparer) {
        return CallStatic(Enumerable.Distinct<TSource>, source, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ElementAt{TSource}(System.Collections.Generic.IEnumerable{TSource}, int)"/>
    public static Lambda<TSource> ElementAt<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<int> index) {
        return CallStatic(Enumerable.ElementAt<TSource>, source, index);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ElementAtOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource}, int)"/>
    public static Lambda<TSource> ElementAtOrDefault<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<int> index) {
        return CallStatic(Enumerable.ElementAtOrDefault<TSource>, source, index);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Empty{TResult}()"/>
    public static Lambda<IEnumerable<TResult>> Empty<TResult>() {
        return CallStatic(Enumerable.Empty<TResult>);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Except{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<IEnumerable<TSource>> Except<TSource>(this Lambda<IEnumerable<TSource>> first, Lambda<IEnumerable<TSource>> second) {
        return CallStatic(Enumerable.Except<TSource>, first, second);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Except{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEqualityComparer{TSource})"/>
    public static Lambda<IEnumerable<TSource>> Except<TSource>(this Lambda<IEnumerable<TSource>> first, Lambda<IEnumerable<TSource>> second, Lambda<IEqualityComparer<TSource>> comparer) {
        return CallStatic(Enumerable.Except<TSource>, first, second, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.First{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<TSource> First<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.First<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.First{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<TSource> First<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.First<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.FirstOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<TSource> FirstOrDefault<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.FirstOrDefault<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.FirstOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<TSource> FirstOrDefault<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.FirstOrDefault<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.GroupBy{TSource, TKey}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey})"/>
    public static Lambda<IEnumerable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector) {
        return CallStatic(Enumerable.GroupBy<TSource, TKey>, source, keySelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.GroupBy{TSource, TKey}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Collections.Generic.IEqualityComparer{TKey})"/>
    public static Lambda<IEnumerable<IGrouping<TKey, TSource>>> GroupBy<TSource, TKey>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<IEqualityComparer<TKey>> comparer) {
        return CallStatic(Enumerable.GroupBy<TSource, TKey>, source, keySelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.GroupBy{TSource, TKey, TElement}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Func{TSource, TElement})"/>
    public static Lambda<IEnumerable<IGrouping<TKey, TElement>>> GroupBy<TSource, TKey, TElement>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<Func<TSource, TElement>> elementSelector) {
        return CallStatic(Enumerable.GroupBy<TSource, TKey, TElement>, source, keySelector, elementSelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.GroupBy{TSource, TKey, TResult}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Func{TKey, System.Collections.Generic.IEnumerable{TSource}, TResult})"/>
    public static Lambda<IEnumerable<TResult>> GroupBy<TSource, TKey, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector) {
        return CallStatic(Enumerable.GroupBy<TSource, TKey, TResult>, source, keySelector, resultSelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.GroupBy{TSource, TKey, TElement}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Func{TSource, TElement}, System.Collections.Generic.IEqualityComparer{TKey})"/>
    public static Lambda<IEnumerable<IGrouping<TKey, TElement>>> GroupBy<TSource, TKey, TElement>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<Func<TSource, TElement>> elementSelector, Lambda<IEqualityComparer<TKey>> comparer) {
        return CallStatic(Enumerable.GroupBy<TSource, TKey, TElement>, source, keySelector, elementSelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.GroupBy{TSource, TKey, TResult}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Func{TKey, System.Collections.Generic.IEnumerable{TSource}, TResult}, System.Collections.Generic.IEqualityComparer{TKey})"/>
    public static Lambda<IEnumerable<TResult>> GroupBy<TSource, TKey, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector, Lambda<IEqualityComparer<TKey>> comparer) {
        return CallStatic(Enumerable.GroupBy<TSource, TKey, TResult>, source, keySelector, resultSelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.GroupBy{TSource, TKey, TElement, TResult}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Func{TSource, TElement}, System.Func{TKey, System.Collections.Generic.IEnumerable{TElement}, TResult})"/>
    public static Lambda<IEnumerable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<Func<TSource, TElement>> elementSelector, Lambda<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector) {
        return CallStatic(Enumerable.GroupBy<TSource, TKey, TElement, TResult>, source, keySelector, elementSelector, resultSelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.GroupBy{TSource, TKey, TElement, TResult}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Func{TSource, TElement}, System.Func{TKey, System.Collections.Generic.IEnumerable{TElement}, TResult}, System.Collections.Generic.IEqualityComparer{TKey})"/>
    public static Lambda<IEnumerable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<Func<TSource, TElement>> elementSelector, Lambda<Func<TKey, IEnumerable<TElement>, TResult>> resultSelector, Lambda<IEqualityComparer<TKey>> comparer) {
        return CallStatic(Enumerable.GroupBy<TSource, TKey, TElement, TResult>, source, keySelector, elementSelector, resultSelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.GroupJoin{TOuter, TInner, TKey, TResult}(System.Collections.Generic.IEnumerable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Func{TOuter, TKey}, System.Func{TInner, TKey}, System.Func{TOuter, System.Collections.Generic.IEnumerable{TInner}, TResult})"/>
    public static Lambda<IEnumerable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this Lambda<IEnumerable<TOuter>> outer, Lambda<IEnumerable<TInner>> inner, Lambda<Func<TOuter, TKey>> outerKeySelector, Lambda<Func<TInner, TKey>> innerKeySelector, Lambda<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector) {
        return CallStatic(Enumerable.GroupJoin<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.GroupJoin{TOuter, TInner, TKey, TResult}(System.Collections.Generic.IEnumerable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Func{TOuter, TKey}, System.Func{TInner, TKey}, System.Func{TOuter, System.Collections.Generic.IEnumerable{TInner}, TResult}, System.Collections.Generic.IEqualityComparer{TKey})"/>
    public static Lambda<IEnumerable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this Lambda<IEnumerable<TOuter>> outer, Lambda<IEnumerable<TInner>> inner, Lambda<Func<TOuter, TKey>> outerKeySelector, Lambda<Func<TInner, TKey>> innerKeySelector, Lambda<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector, Lambda<IEqualityComparer<TKey>> comparer) {
        return CallStatic(Enumerable.GroupJoin<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Intersect{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<IEnumerable<TSource>> Intersect<TSource>(this Lambda<IEnumerable<TSource>> first, Lambda<IEnumerable<TSource>> second) {
        return CallStatic(Enumerable.Intersect<TSource>, first, second);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Intersect{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEqualityComparer{TSource})"/>
    public static Lambda<IEnumerable<TSource>> Intersect<TSource>(this Lambda<IEnumerable<TSource>> first, Lambda<IEnumerable<TSource>> second, Lambda<IEqualityComparer<TSource>> comparer) {
        return CallStatic(Enumerable.Intersect<TSource>, first, second, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Join{TOuter, TInner, TKey, TResult}(System.Collections.Generic.IEnumerable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Func{TOuter, TKey}, System.Func{TInner, TKey}, System.Func{TOuter, TInner, TResult})"/>
    public static Lambda<IEnumerable<TResult>> Join<TOuter, TInner, TKey, TResult>(this Lambda<IEnumerable<TOuter>> outer, Lambda<IEnumerable<TInner>> inner, Lambda<Func<TOuter, TKey>> outerKeySelector, Lambda<Func<TInner, TKey>> innerKeySelector, Lambda<Func<TOuter, TInner, TResult>> resultSelector) {
        return CallStatic(Enumerable.Join<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Join{TOuter, TInner, TKey, TResult}(System.Collections.Generic.IEnumerable{TOuter}, System.Collections.Generic.IEnumerable{TInner}, System.Func{TOuter, TKey}, System.Func{TInner, TKey}, System.Func{TOuter, TInner, TResult}, System.Collections.Generic.IEqualityComparer{TKey})"/>
    public static Lambda<IEnumerable<TResult>> Join<TOuter, TInner, TKey, TResult>(this Lambda<IEnumerable<TOuter>> outer, Lambda<IEnumerable<TInner>> inner, Lambda<Func<TOuter, TKey>> outerKeySelector, Lambda<Func<TInner, TKey>> innerKeySelector, Lambda<Func<TOuter, TInner, TResult>> resultSelector, Lambda<IEqualityComparer<TKey>> comparer) {
        return CallStatic(Enumerable.Join<TOuter, TInner, TKey, TResult>, outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Last{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<TSource> Last<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.Last<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Last{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<TSource> Last<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.Last<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.LastOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<TSource> LastOrDefault<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.LastOrDefault<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.LastOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<TSource> LastOrDefault<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.LastOrDefault<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.LongCount{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<long> LongCount<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.LongCount<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.LongCount{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<long> LongCount<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.LongCount<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max(System.Collections.Generic.IEnumerable{int})"/>
    public static Lambda<int> Max(this Lambda<IEnumerable<int>> source) {
        return CallStatic(Enumerable.Max, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max(System.Collections.Generic.IEnumerable{long})"/>
    public static Lambda<long> Max(this Lambda<IEnumerable<long>> source) {
        return CallStatic(Enumerable.Max, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max(System.Collections.Generic.IEnumerable{double})"/>
    public static Lambda<double> Max(this Lambda<IEnumerable<double>> source) {
        return CallStatic(Enumerable.Max, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max(System.Collections.Generic.IEnumerable{float})"/>
    public static Lambda<float> Max(this Lambda<IEnumerable<float>> source) {
        return CallStatic(Enumerable.Max, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max(System.Collections.Generic.IEnumerable{decimal})"/>
    public static Lambda<decimal> Max(this Lambda<IEnumerable<decimal>> source) {
        return CallStatic(Enumerable.Max, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max(System.Collections.Generic.IEnumerable{int?})"/>
    public static Lambda<int?> Max(this Lambda<IEnumerable<int?>> source) {
        return CallStatic(Enumerable.Max, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max(System.Collections.Generic.IEnumerable{long?})"/>
    public static Lambda<long?> Max(this Lambda<IEnumerable<long?>> source) {
        return CallStatic(Enumerable.Max, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max(System.Collections.Generic.IEnumerable{double?})"/>
    public static Lambda<double?> Max(this Lambda<IEnumerable<double?>> source) {
        return CallStatic(Enumerable.Max, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max(System.Collections.Generic.IEnumerable{float?})"/>
    public static Lambda<float?> Max(this Lambda<IEnumerable<float?>> source) {
        return CallStatic(Enumerable.Max, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max(System.Collections.Generic.IEnumerable{decimal?})"/>
    public static Lambda<decimal?> Max(this Lambda<IEnumerable<decimal?>> source) {
        return CallStatic(Enumerable.Max, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<TSource> Max<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.Max<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int})"/>
    public static Lambda<int> Max<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int>> selector) {
        return CallStatic(Enumerable.Max<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int?})"/>
    public static Lambda<int?> Max<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int?>> selector) {
        return CallStatic(Enumerable.Max<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, long})"/>
    public static Lambda<long> Max<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, long>> selector) {
        return CallStatic(Enumerable.Max<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, long?})"/>
    public static Lambda<long?> Max<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, long?>> selector) {
        return CallStatic(Enumerable.Max<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, float})"/>
    public static Lambda<float> Max<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, float>> selector) {
        return CallStatic(Enumerable.Max<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, float?})"/>
    public static Lambda<float?> Max<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, float?>> selector) {
        return CallStatic(Enumerable.Max<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, double})"/>
    public static Lambda<double> Max<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, double>> selector) {
        return CallStatic(Enumerable.Max<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, double?})"/>
    public static Lambda<double?> Max<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, double?>> selector) {
        return CallStatic(Enumerable.Max<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, decimal})"/>
    public static Lambda<decimal> Max<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, decimal>> selector) {
        return CallStatic(Enumerable.Max<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, decimal?})"/>
    public static Lambda<decimal?> Max<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, decimal?>> selector) {
        return CallStatic(Enumerable.Max<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Max{TSource, TResult}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TResult})"/>
    public static Lambda<TResult> Max<TSource, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TResult>> selector) {
        return CallStatic(Enumerable.Max<TSource, TResult>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min(System.Collections.Generic.IEnumerable{int})"/>
    public static Lambda<int> Min(this Lambda<IEnumerable<int>> source) {
        return CallStatic(Enumerable.Min, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min(System.Collections.Generic.IEnumerable{long})"/>
    public static Lambda<long> Min(this Lambda<IEnumerable<long>> source) {
        return CallStatic(Enumerable.Min, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min(System.Collections.Generic.IEnumerable{float})"/>
    public static Lambda<float> Min(this Lambda<IEnumerable<float>> source) {
        return CallStatic(Enumerable.Min, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min(System.Collections.Generic.IEnumerable{double})"/>
    public static Lambda<double> Min(this Lambda<IEnumerable<double>> source) {
        return CallStatic(Enumerable.Min, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min(System.Collections.Generic.IEnumerable{decimal})"/>
    public static Lambda<decimal> Min(this Lambda<IEnumerable<decimal>> source) {
        return CallStatic(Enumerable.Min, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min(System.Collections.Generic.IEnumerable{int?})"/>
    public static Lambda<int?> Min(this Lambda<IEnumerable<int?>> source) {
        return CallStatic(Enumerable.Min, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min(System.Collections.Generic.IEnumerable{long?})"/>
    public static Lambda<long?> Min(this Lambda<IEnumerable<long?>> source) {
        return CallStatic(Enumerable.Min, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min(System.Collections.Generic.IEnumerable{float?})"/>
    public static Lambda<float?> Min(this Lambda<IEnumerable<float?>> source) {
        return CallStatic(Enumerable.Min, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min(System.Collections.Generic.IEnumerable{double?})"/>
    public static Lambda<double?> Min(this Lambda<IEnumerable<double?>> source) {
        return CallStatic(Enumerable.Min, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min(System.Collections.Generic.IEnumerable{decimal?})"/>
    public static Lambda<decimal?> Min(this Lambda<IEnumerable<decimal?>> source) {
        return CallStatic(Enumerable.Min, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<TSource> Min<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.Min<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int})"/>
    public static Lambda<int> Min<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int>> selector) {
        return CallStatic(Enumerable.Min<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int?})"/>
    public static Lambda<int?> Min<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int?>> selector) {
        return CallStatic(Enumerable.Min<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, long})"/>
    public static Lambda<long> Min<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, long>> selector) {
        return CallStatic(Enumerable.Min<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, long?})"/>
    public static Lambda<long?> Min<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, long?>> selector) {
        return CallStatic(Enumerable.Min<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, float})"/>
    public static Lambda<float> Min<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, float>> selector) {
        return CallStatic(Enumerable.Min<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, float?})"/>
    public static Lambda<float?> Min<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, float?>> selector) {
        return CallStatic(Enumerable.Min<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, double})"/>
    public static Lambda<double> Min<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, double>> selector) {
        return CallStatic(Enumerable.Min<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, double?})"/>
    public static Lambda<double?> Min<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, double?>> selector) {
        return CallStatic(Enumerable.Min<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, decimal})"/>
    public static Lambda<decimal> Min<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, decimal>> selector) {
        return CallStatic(Enumerable.Min<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, decimal?})"/>
    public static Lambda<decimal?> Min<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, decimal?>> selector) {
        return CallStatic(Enumerable.Min<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Min{TSource, TResult}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TResult})"/>
    public static Lambda<TResult> Min<TSource, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TResult>> selector) {
        return CallStatic(Enumerable.Min<TSource, TResult>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.OfType{TResult}(System.Collections.IEnumerable)"/>
    public static Lambda<IEnumerable<TResult>> OfType<TResult>(this Lambda<IEnumerable> source) {
        return CallStatic(Enumerable.OfType<TResult>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.OrderBy{TSource, TKey}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey})"/>
    public static Lambda<IOrderedEnumerable<TSource>> OrderBy<TSource, TKey>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector) {
        return CallStatic(Enumerable.OrderBy<TSource, TKey>, source, keySelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.OrderBy{TSource, TKey}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Collections.Generic.IComparer{TKey})"/>
    public static Lambda<IOrderedEnumerable<TSource>> OrderBy<TSource, TKey>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<IComparer<TKey>> comparer) {
        return CallStatic(Enumerable.OrderBy<TSource, TKey>, source, keySelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.OrderByDescending{TSource, TKey}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey})"/>
    public static Lambda<IOrderedEnumerable<TSource>> OrderByDescending<TSource, TKey>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector) {
        return CallStatic(Enumerable.OrderByDescending<TSource, TKey>, source, keySelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.OrderByDescending{TSource, TKey}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Collections.Generic.IComparer{TKey})"/>
    public static Lambda<IOrderedEnumerable<TSource>> OrderByDescending<TSource, TKey>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<IComparer<TKey>> comparer) {
        return CallStatic(Enumerable.OrderByDescending<TSource, TKey>, source, keySelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Prepend{TSource}(System.Collections.Generic.IEnumerable{TSource}, TSource)"/>
    public static Lambda<IEnumerable<TSource>> Prepend<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<TSource> element) {
        return CallStatic(Enumerable.Prepend<TSource>, source, element);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Range(int, int)"/>
    public static Lambda<IEnumerable<int>> Range(this Lambda<int> start, Lambda<int> count) {
        return CallStatic(Enumerable.Range, start, count);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Repeat{TResult}(TResult, int)"/>
    public static Lambda<IEnumerable<TResult>> Repeat<TResult>(this Lambda<TResult> element, Lambda<int> count) {
        return CallStatic(Enumerable.Repeat<TResult>, element, count);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Reverse{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<IEnumerable<TSource>> Reverse<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.Reverse<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Select{TSource, TResult}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TResult})"/>
    public static Lambda<IEnumerable<TResult>> Select<TSource, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TResult>> selector) {
        return CallStatic(Enumerable.Select<TSource, TResult>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Select{TSource, TResult}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int, TResult})"/>
    public static Lambda<IEnumerable<TResult>> Select<TSource, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int, TResult>> selector) {
        return CallStatic(Enumerable.Select<TSource, TResult>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.SelectMany{TSource, TResult}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, System.Collections.Generic.IEnumerable{TResult}})"/>
    public static Lambda<IEnumerable<TResult>> SelectMany<TSource, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, IEnumerable<TResult>>> selector) {
        return CallStatic(Enumerable.SelectMany<TSource, TResult>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.SelectMany{TSource, TResult}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int, System.Collections.Generic.IEnumerable{TResult}})"/>
    public static Lambda<IEnumerable<TResult>> SelectMany<TSource, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int, IEnumerable<TResult>>> selector) {
        return CallStatic(Enumerable.SelectMany<TSource, TResult>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.SelectMany{TSource, TCollection, TResult}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int, System.Collections.Generic.IEnumerable{TCollection}}, System.Func{TSource, TCollection, TResult})"/>
    public static Lambda<IEnumerable<TResult>> SelectMany<TSource, TCollection, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector, Lambda<Func<TSource, TCollection, TResult>> resultSelector) {
        return CallStatic(Enumerable.SelectMany<TSource, TCollection, TResult>, source, collectionSelector, resultSelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.SelectMany{TSource, TCollection, TResult}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, System.Collections.Generic.IEnumerable{TCollection}}, System.Func{TSource, TCollection, TResult})"/>
    public static Lambda<IEnumerable<TResult>> SelectMany<TSource, TCollection, TResult>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Lambda<Func<TSource, TCollection, TResult>> resultSelector) {
        return CallStatic(Enumerable.SelectMany<TSource, TCollection, TResult>, source, collectionSelector, resultSelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.SequenceEqual{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<bool> SequenceEqual<TSource>(this Lambda<IEnumerable<TSource>> first, Lambda<IEnumerable<TSource>> second) {
        return CallStatic(Enumerable.SequenceEqual<TSource>, first, second);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.SequenceEqual{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEqualityComparer{TSource})"/>
    public static Lambda<bool> SequenceEqual<TSource>(this Lambda<IEnumerable<TSource>> first, Lambda<IEnumerable<TSource>> second, Lambda<IEqualityComparer<TSource>> comparer) {
        return CallStatic(Enumerable.SequenceEqual<TSource>, first, second, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Single{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<TSource> Single<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.Single<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Single{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<TSource> Single<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.Single<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.SingleOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<TSource> SingleOrDefault<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.SingleOrDefault<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.SingleOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<TSource> SingleOrDefault<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.SingleOrDefault<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Skip{TSource}(System.Collections.Generic.IEnumerable{TSource}, int)"/>
    public static Lambda<IEnumerable<TSource>> Skip<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<int> count) {
        return CallStatic(Enumerable.Skip<TSource>, source, count);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.SkipWhile{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<IEnumerable<TSource>> SkipWhile<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.SkipWhile<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.SkipWhile{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int, bool})"/>
    public static Lambda<IEnumerable<TSource>> SkipWhile<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int, bool>> predicate) {
        return CallStatic(Enumerable.SkipWhile<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum(System.Collections.Generic.IEnumerable{int})"/>
    public static Lambda<int> Sum(this Lambda<IEnumerable<int>> source) {
        return CallStatic(Enumerable.Sum, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum(System.Collections.Generic.IEnumerable{long})"/>
    public static Lambda<long> Sum(this Lambda<IEnumerable<long>> source) {
        return CallStatic(Enumerable.Sum, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum(System.Collections.Generic.IEnumerable{float})"/>
    public static Lambda<float> Sum(this Lambda<IEnumerable<float>> source) {
        return CallStatic(Enumerable.Sum, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum(System.Collections.Generic.IEnumerable{double})"/>
    public static Lambda<double> Sum(this Lambda<IEnumerable<double>> source) {
        return CallStatic(Enumerable.Sum, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum(System.Collections.Generic.IEnumerable{decimal})"/>
    public static Lambda<decimal> Sum(this Lambda<IEnumerable<decimal>> source) {
        return CallStatic(Enumerable.Sum, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum(System.Collections.Generic.IEnumerable{int?})"/>
    public static Lambda<int?> Sum(this Lambda<IEnumerable<int?>> source) {
        return CallStatic(Enumerable.Sum, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum(System.Collections.Generic.IEnumerable{long?})"/>
    public static Lambda<long?> Sum(this Lambda<IEnumerable<long?>> source) {
        return CallStatic(Enumerable.Sum, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum(System.Collections.Generic.IEnumerable{float?})"/>
    public static Lambda<float?> Sum(this Lambda<IEnumerable<float?>> source) {
        return CallStatic(Enumerable.Sum, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum(System.Collections.Generic.IEnumerable{double?})"/>
    public static Lambda<double?> Sum(this Lambda<IEnumerable<double?>> source) {
        return CallStatic(Enumerable.Sum, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum(System.Collections.Generic.IEnumerable{decimal?})"/>
    public static Lambda<decimal?> Sum(this Lambda<IEnumerable<decimal?>> source) {
        return CallStatic(Enumerable.Sum, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int})"/>
    public static Lambda<int> Sum<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int>> selector) {
        return CallStatic(Enumerable.Sum<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int?})"/>
    public static Lambda<int?> Sum<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int?>> selector) {
        return CallStatic(Enumerable.Sum<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, long})"/>
    public static Lambda<long> Sum<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, long>> selector) {
        return CallStatic(Enumerable.Sum<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, long?})"/>
    public static Lambda<long?> Sum<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, long?>> selector) {
        return CallStatic(Enumerable.Sum<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, float})"/>
    public static Lambda<float> Sum<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, float>> selector) {
        return CallStatic(Enumerable.Sum<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, float?})"/>
    public static Lambda<float?> Sum<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, float?>> selector) {
        return CallStatic(Enumerable.Sum<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, double})"/>
    public static Lambda<double> Sum<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, double>> selector) {
        return CallStatic(Enumerable.Sum<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, double?})"/>
    public static Lambda<double?> Sum<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, double?>> selector) {
        return CallStatic(Enumerable.Sum<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, decimal})"/>
    public static Lambda<decimal> Sum<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, decimal>> selector) {
        return CallStatic(Enumerable.Sum<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Sum{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, decimal?})"/>
    public static Lambda<decimal?> Sum<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, decimal?>> selector) {
        return CallStatic(Enumerable.Sum<TSource>, source, selector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Take{TSource}(System.Collections.Generic.IEnumerable{TSource}, int)"/>
    public static Lambda<IEnumerable<TSource>> Take<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<int> count) {
        return CallStatic(Enumerable.Take<TSource>, source, count);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.TakeWhile{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<IEnumerable<TSource>> TakeWhile<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.TakeWhile<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.TakeWhile{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int, bool})"/>
    public static Lambda<IEnumerable<TSource>> TakeWhile<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int, bool>> predicate) {
        return CallStatic(Enumerable.TakeWhile<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ThenBy{TSource, TKey}(System.Linq.IOrderedEnumerable{TSource}, System.Func{TSource, TKey})"/>
    public static Lambda<IOrderedEnumerable<TSource>> ThenBy<TSource, TKey>(this Lambda<IOrderedEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector) {
        return CallStatic(Enumerable.ThenBy<TSource, TKey>, source, keySelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ThenBy{TSource, TKey}(System.Linq.IOrderedEnumerable{TSource}, System.Func{TSource, TKey}, System.Collections.Generic.IComparer{TKey})"/>
    public static Lambda<IOrderedEnumerable<TSource>> ThenBy<TSource, TKey>(this Lambda<IOrderedEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<IComparer<TKey>> comparer) {
        return CallStatic(Enumerable.ThenBy<TSource, TKey>, source, keySelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ThenByDescending{TSource, TKey}(System.Linq.IOrderedEnumerable{TSource}, System.Func{TSource, TKey})"/>
    public static Lambda<IOrderedEnumerable<TSource>> ThenByDescending<TSource, TKey>(this Lambda<IOrderedEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector) {
        return CallStatic(Enumerable.ThenByDescending<TSource, TKey>, source, keySelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ThenByDescending{TSource, TKey}(System.Linq.IOrderedEnumerable{TSource}, System.Func{TSource, TKey}, System.Collections.Generic.IComparer{TKey})"/>
    public static Lambda<IOrderedEnumerable<TSource>> ThenByDescending<TSource, TKey>(this Lambda<IOrderedEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<IComparer<TKey>> comparer) {
        return CallStatic(Enumerable.ThenByDescending<TSource, TKey>, source, keySelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ToArray{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<TSource[]> ToArray<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.ToArray<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ToDictionary{TSource, TKey}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey})"/>
    public static Lambda<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector) {
        return CallStatic(Enumerable.ToDictionary<TSource, TKey>, source, keySelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ToDictionary{TSource, TKey}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Collections.Generic.IEqualityComparer{TKey})"/>
    public static Lambda<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<IEqualityComparer<TKey>> comparer) {
        return CallStatic(Enumerable.ToDictionary<TSource, TKey>, source, keySelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ToDictionary{TSource, TKey, TElement}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Func{TSource, TElement})"/>
    public static Lambda<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<Func<TSource, TElement>> elementSelector) {
        return CallStatic(Enumerable.ToDictionary<TSource, TKey, TElement>, source, keySelector, elementSelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ToDictionary{TSource, TKey, TElement}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Func{TSource, TElement}, System.Collections.Generic.IEqualityComparer{TKey})"/>
    public static Lambda<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<Func<TSource, TElement>> elementSelector, Lambda<IEqualityComparer<TKey>> comparer) {
        return CallStatic(Enumerable.ToDictionary<TSource, TKey, TElement>, source, keySelector, elementSelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ToList{TSource}(System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<List<TSource>> ToList<TSource>(this Lambda<IEnumerable<TSource>> source) {
        return CallStatic(Enumerable.ToList<TSource>, source);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ToLookup{TSource, TKey}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey})"/>
    public static Lambda<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector) {
        return CallStatic(Enumerable.ToLookup<TSource, TKey>, source, keySelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ToLookup{TSource, TKey}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Collections.Generic.IEqualityComparer{TKey})"/>
    public static Lambda<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<IEqualityComparer<TKey>> comparer) {
        return CallStatic(Enumerable.ToLookup<TSource, TKey>, source, keySelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ToLookup{TSource, TKey, TElement}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Func{TSource, TElement})"/>
    public static Lambda<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<Func<TSource, TElement>> elementSelector) {
        return CallStatic(Enumerable.ToLookup<TSource, TKey, TElement>, source, keySelector, elementSelector);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.ToLookup{TSource, TKey, TElement}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey}, System.Func{TSource, TElement}, System.Collections.Generic.IEqualityComparer{TKey})"/>
    public static Lambda<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, TKey>> keySelector, Lambda<Func<TSource, TElement>> elementSelector, Lambda<IEqualityComparer<TKey>> comparer) {
        return CallStatic(Enumerable.ToLookup<TSource, TKey, TElement>, source, keySelector, elementSelector, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Union{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource})"/>
    public static Lambda<IEnumerable<TSource>> Union<TSource>(this Lambda<IEnumerable<TSource>> first, Lambda<IEnumerable<TSource>> second) {
        return CallStatic(Enumerable.Union<TSource>, first, second);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Union{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEqualityComparer{TSource})"/>
    public static Lambda<IEnumerable<TSource>> Union<TSource>(this Lambda<IEnumerable<TSource>> first, Lambda<IEnumerable<TSource>> second, Lambda<IEqualityComparer<TSource>> comparer) {
        return CallStatic(Enumerable.Union<TSource>, first, second, comparer);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Where{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, bool})"/>
    public static Lambda<IEnumerable<TSource>> Where<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, bool>> predicate) {
        return CallStatic(Enumerable.Where<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Where{TSource}(System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, int, bool})"/>
    public static Lambda<IEnumerable<TSource>> Where<TSource>(this Lambda<IEnumerable<TSource>> source, Lambda<Func<TSource, int, bool>> predicate) {
        return CallStatic(Enumerable.Where<TSource>, source, predicate);
    }

    /// <inheritdoc cref="System.Linq.Enumerable.Zip{TFirst, TSecond, TResult}(System.Collections.Generic.IEnumerable{TFirst}, System.Collections.Generic.IEnumerable{TSecond}, System.Func{TFirst, TSecond, TResult})"/>
    public static Lambda<IEnumerable<TResult>> Zip<TFirst, TSecond, TResult>(this Lambda<IEnumerable<TFirst>> first, Lambda<IEnumerable<TSecond>> second, Lambda<Func<TFirst, TSecond, TResult>> resultSelector) {
        return CallStatic(Enumerable.Zip<TFirst, TSecond, TResult>, first, second, resultSelector);
    }

}