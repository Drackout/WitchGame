using System;

public static class PlayerResources
{
    private static int[] stones;
    private static int gold;

    public static int Gold
    {
        get => gold;
        set
        {
            gold = Math.Max(0, value);
        }
    }

    static PlayerResources()
    {
        int elementCount = Enum.GetNames(typeof(Element)).Length;
        stones = new int[elementCount];
    }

    public static int GetStones(Element element)
    {
        return stones[(int)element];
    }

    public static void SetStones(Element element, int value)
    {
        stones[(int)element] = Math.Max(0, value);
    }
}
