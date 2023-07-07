using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NewVertex
{
    public int id;
    public List<Edge> edges;
    public int mapElement = -1;
    public int[] possibleElements;
    public NewVertex(int id, int[] possibleElements)
    {
        this.id = id;
        this.possibleElements = possibleElements;
        edges = new List<Edge>();
    }

    public void UpdateAdjacent(MapElement mapElement)
    {
        foreach (Edge edge in edges)
            edge.adjacentVertex.ModifyPossibleElements(mapElement.rules.First(r => r.direction == edge.id).constraints);
    }
    public void ModifyPossibleElements(int[] toIntersect)
    {
        if (mapElement != -1)
            return;

        possibleElements = possibleElements.Intersect(toIntersect).ToArray();
    }
    public void SetRandomElement()
    {
        mapElement = possibleElements[Random.Range(0, possibleElements.Length)];
        possibleElements = null;
        //MapGeneration.instance.RemoveVertex(id);
    }
    public void AddEdge(Direction id, int adjacentNewVertex, int weight)
    {
        edges.Add(new Edge(id, adjacentNewVertex, weight));
    }
    public string Print1(bool printWeight)
    {
        StringBuilder str = new StringBuilder();
        str.Append($"V[{id}]: ");

        foreach (Edge edge in edges)
        {
            str.Append($" [{edge.id} - {edge.adjacentVertex.id}]");
            if (printWeight)
                str.Append($"[w{edge.weight}]");
        }
        str.Append("\n");
        return str.ToString();
    }
    public string Print2()
    {
        StringBuilder str = new StringBuilder();

        str.Append($" id [{this.id}] - Element [{mapElement}] - Possibles ");

        if (possibleElements != null)
        {
            foreach (int i in this.possibleElements)
                str.Append($"[{i}]");
        }
        else
            str.Append("null");

        str.Append("\n");
        return str.ToString();
    }
}
