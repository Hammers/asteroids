using System;
using System.Collections.Generic;

public static class ListExtensions
{
    public static T RandomItem<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new IndexOutOfRangeException("Cannot select a random item from an empty list");
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
    
}