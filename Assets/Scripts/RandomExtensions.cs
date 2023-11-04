using System;
using System.Collections.Generic;

public static class RandomExtensions
{
    public static IList<T> Shuffle<T>(this Random random, IList<T> list)
    {
        int nElements = list.Count;  
        while (nElements > 1) {  
            nElements--;  
            int rng = random.Next(nElements + 1);  
            T value = list[rng];  
            list[rng] = list[nElements];  
            list[nElements] = value;  
        }
        return list;
    }
}
