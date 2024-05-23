using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UGYST 
{
    public static bool IsBetween(this float testValue, float bound1, float bound2)
    {
        if (bound1 > bound2)
            return testValue >= bound2 && testValue <= bound1;
        return testValue >= bound1 && testValue <= bound2;
    }
    public static bool IsInsideSquare(this Vector2 testValue, Corners squareCorners)
    {
        if(testValue.x.IsBetween(squareCorners.BottomLeft.x, squareCorners.BottomRight.x)
            &&
            testValue.y.IsBetween(squareCorners.TopLeft.y, squareCorners.BottomLeft.y)
            )
        {
            return true;
        }
        return false;
    }

    public static Vector2 ToVector2(this Vector3 vector)
    {
        return vector;
    }

    public static Corners GetCorners(this RectTransform transform)
    {
        Vector2[] returnArray = new Vector2[4];

        float halfWidth = transform.sizeDelta.x / 2;
        float halfHeight = transform.sizeDelta.y / 2;

        //TopLeft
        returnArray[0] = new Vector2(
            transform.position.x - halfWidth,
            transform.position.y + halfHeight);

        //TopRight
        returnArray[1] = new Vector2(
            transform.position.x + halfWidth,
            transform.position.y + halfHeight);

        //BottomLeft
        returnArray[2] = new Vector2(
            transform.position.x - halfWidth,
            transform.position.y - halfHeight);

        //BottomRight
        returnArray[3] = new Vector2(
            transform.position.x + halfWidth,
            transform.position.y - halfHeight);

        return new Corners(
            returnArray[0],returnArray[1],
            returnArray[2],returnArray[3]);
    }
    public static float HalfWidth(this RectTransform transform)
    {
        return transform.sizeDelta.x / 2;
    }
    public static float HalfHeight(this RectTransform transform)
    {
        return transform.sizeDelta.y / 2;
    }

    public static void GetInterfaces<T>(out List<T> resultList, GameObject objectToSearch) where T : class
    {
        MonoBehaviour[] list = objectToSearch.GetComponents<MonoBehaviour>();
        resultList = new List<T>();
        foreach (MonoBehaviour mb in list)
        {
            if (mb is T)
            {
                //found one
                resultList.Add((T)((System.Object)mb));
            }
        }
    }

    public static string printy<T>(this IEnumerable<T> enumerable)
    {
        string returnstring = "";
        foreach(T t in enumerable)
        {
            returnstring += t.ToString() + " ";
        }
        Debug.Log(returnstring);
        return returnstring;
    }

}

public struct Corners
{
    public Corners(Vector2 topLeft, Vector2 topRight,
        Vector2 bottomLeft, Vector2 bottomRight)
    {
        TopLeft = topLeft;
        TopRight = topRight;
        BottomLeft = bottomLeft;
        BottomRight = bottomRight;
    }

    public Vector2 TopLeft { get; }
    public Vector2 TopRight { get; }
    public Vector2 BottomLeft { get; }
    public Vector2 BottomRight { get; } 
}
