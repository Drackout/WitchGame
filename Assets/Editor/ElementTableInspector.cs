using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(ElementTable))]
public class ElementTablePropertyDrawer : MatrixIntPropertyDrawer
{
    protected override VisualElement MakeLabel(int index)
    {
        Element element = (Element)(index + 1);

        string iconName = element switch
        {
            Element.Fire => "UI_Fire_Stone",
            Element.Grass => "UI_Grass_Stone",
            Element.Water => "UI_Water_Stone",
            _ => "UI_Coins_Small",
        };
        string iconPath = $"Assets/Art/{iconName}.png";

        Texture2D icon = (Texture2D)AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath);

        VisualElement root = new VisualElement();
        root.style.flexDirection = FlexDirection.Row;
        root.style.justifyContent = Justify.Center;

        Image image = new Image();
        image.style.width = 15;
        image.style.height = 15;
        image.image = icon;

        root.Add(image);

        return root;
    }
}
