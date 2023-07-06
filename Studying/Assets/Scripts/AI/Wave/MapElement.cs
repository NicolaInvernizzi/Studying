using System;
using UnityEngine;

[Serializable]
public class MapElement
{
    public int id;
    public GameObject prefab;
    public ElementRule[] rules;
}

[Serializable]
public class ElementRule
{
    public Direction direction;
    public int[] constraints;
}

[Serializable]
public class MapRule
{
    public Direction direction;
    public int[] constraints;
}
