using System;
using System.Collections.Generic;
using System.Linq;

namespace DGCLib_Base
{
    public static class LinqExtensions
    {
        private static T ThrowIfNull<T>(this T value, string variableName) where T : class
        {
            if (value == null)
            {
                throw new NullReferenceException(string.Format("Value is Null: {0}", variableName));
            }

            return value;
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            source.ThrowIfNull("source");
            selector.ThrowIfNull("selector");
            comparer.ThrowIfNull("comparer");
            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                TSource min = sourceIterator.Current;
                TKey minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    TSource candidate = sourceIterator.Current;
                    TKey candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            source.ThrowIfNull("source");
            selector.ThrowIfNull("selector");
            comparer.ThrowIfNull("comparer");
            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                TSource max = sourceIterator.Current;
                TKey minKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    TSource candidate = sourceIterator.Current;
                    TKey candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) > 0)
                    {
                        max = candidate;
                        minKey = candidateProjected;
                    }
                }
                return max;
            }
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> selector)
        {
            source.ThrowIfNull("source");
            selector.ThrowIfNull("selector");
            foreach (TSource src in source)
            {
                selector(src);
            }
        }

        public static bool HasAtLeast<TSource>(this IEnumerable<TSource> source, int elemCount)
        {
            if (source == null)
                return false;

            if (elemCount < 0)
                throw new ArgumentOutOfRangeException("elemCount", "ElemCount must be non negative");

            return source.Take(elemCount).Count() == elemCount;
        }

        public static IEnumerable<T> AsIEnumerable<T>(this T obj)
        {
            return Enumerable.Repeat(obj, 1);
        }
    }
}