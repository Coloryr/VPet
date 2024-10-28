using System;
using System.Collections.Generic;

namespace HKW.HKWUtils;

/// <summary>
/// 比较工具
/// </summary>
/// <typeparam name="T">比较类型</typeparam>
public class CompareUtils<T>(Func<T, object> comparer) : IEqualityComparer<T>, IEquatable<CompareUtils<T>>
{
    /// <summary>
    /// (Comparer, CompareUtils)
    /// </summary>
    public static Dictionary<Func<T, object>, CompareUtils<T>> Comparers = [];

    public static CompareUtils<T> Create(Func<T, object> comparer)
    {
        if (Comparers.TryGetValue(comparer, out var value) is false)
            value = Comparers[comparer] = new CompareUtils<T>(comparer);
        return value;
    }

    public Func<T, object> Comparer { get; set; } = comparer;

    public bool Equals(T? x, T? y)
    {
        if (x == null || y == null)
        {
            return false;
        }
        return Comparer(x).Equals(Comparer(y));
    }

    public int GetHashCode(T obj)
    {
        return Comparer(obj).GetHashCode();
    }

    public bool Equals(CompareUtils<T>? other)
    {
        return Comparer.Equals(other?.Comparer);
    }
}
