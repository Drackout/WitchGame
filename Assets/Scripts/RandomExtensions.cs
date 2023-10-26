using System;
using System.Collections.Generic;

public static class RandomExtensions
{
    public static IList<T> Shuffle<T>(this Random random, IList<T> list)
    {
        return list;
    }
}
