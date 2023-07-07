using UnityEngine;

public class MapElement : MonoBehaviour
{
    public int id;
    public ElementRule[] rules = new ElementRule[4]
    {
        new ElementRule(Direction.Up),
        new ElementRule(Direction.Down),
        new ElementRule(Direction.Left),
        new ElementRule(Direction.Right),
    };
}

