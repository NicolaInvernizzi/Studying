using System;

[Serializable]
public class MapElement
{
    public int id;
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
