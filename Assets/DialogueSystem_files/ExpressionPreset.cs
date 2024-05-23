using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Scriptables/ExpressionPreset")]
[System.Serializable]
public class ExpressionPreset : ScriptableObject
{
    [field:SerializeField]
    public List<Expression> Emotions { get; private set; }

    public void AddNewExpression()
    {
        if(Emotions == null) Emotions = new List<Expression>();
        Emotions.Add(new Expression()); 
    }
}
