using UnityEngine;

public class MapElement : MonoBehaviour
{
    public int id;
    public Rule[] rules = new Rule[4]
    {
        new Rule(Direction.Up),
        new Rule(Direction.Down),
        new Rule(Direction.Left),
        new Rule(Direction.Right),
    };
}

