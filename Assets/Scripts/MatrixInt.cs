using System;
using UnityEngine;

[Serializable]
public class MatrixInt
{
    [SerializeField] private int[] m;
    [SerializeField] private int size;

    public int Size => size;

    public int At(int r, int c)
    {
        return m[r * size + c];
    }

    public void Set(int r, int c, int val)
    {
        m[r * size + c] = val;
    }
}
