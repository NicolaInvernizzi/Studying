using UnityEngine;

[CreateAssetMenu(fileName = "New Element", menuName = "MapElement")]
public class Element : ScriptableObject
{
    public GameObject prefab;
    public Rule[] rules = new Rule[4]
    {
        new Rule(Direction.Up),
        new Rule(Direction.Down),
        new Rule(Direction.Left),
        new Rule(Direction.Right),
    };
}
