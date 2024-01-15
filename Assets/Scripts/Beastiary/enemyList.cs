using System;
using System.Collections.Generic;
using UnityEngine;


public struct enemyList
{
    public static enemyList None => new enemyList("image", "Name", "Lore", Element.None, Element.None, Element.None);

    public string Img { get; }
    public String Name { get; }
    public String Lore { get; }
    public Element Element { get; }
    public Element Strong { get; }
    public Element Weak { get; }

    public enemyList(string img, String name, String lore, Element element, Element strong, Element weak)
    {
        Img = img;
        Name = name;
        Lore = lore;
        Element = element;
        Strong = strong;
        Weak = weak;
    }

    public override string ToString()
    {
        return $"{Img} {Name} {Lore} {Element} {Strong} {Weak}";
    }
}

