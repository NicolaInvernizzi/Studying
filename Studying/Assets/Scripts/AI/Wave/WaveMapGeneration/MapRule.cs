using System;

[Serializable]
public class MapRule
{
    public Direction direction;
    public int[] constraints;
}

[Serializable]
public class ElementRule
{
    public Direction direction;
    public int[] constraints;

    public ElementRule (Direction direction)
    {
        this.direction = direction;
    }
}
