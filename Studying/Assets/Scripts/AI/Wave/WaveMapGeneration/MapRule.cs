using System;

[Serializable]
public class Rule
{
    public Direction direction;
    public Element[] constraints;

    public Rule (Direction direction)
    {
        this.direction = direction;
    }
}
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
