using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Vertex
{
    public readonly int id;
    public List<Edge> edges { get; private set; }
    public int mapElement = -1;
    public int[] possibleElements;
    public Vertex(int id, int[] possibleElements)
    {
        this.id = id;
        this.possibleElements = possibleElements;
        edges = new List<Edge>();
    }

    public void UpdateAdjacent()
    {
        foreach(Edge edge in edges)
        {
            MapElement mpE = AdvanceGraph.mapElements.First(e => e.id == mapElement);
            edge.adjacentVertex.ModifyPossibleElements(mpE.rules.First(r => r.direction == edge.id).constraints);
        }
    }
    public void ModifyPossibleElements(int[] toIntersect)
    {
        if (mapElement == -1)
            return;
        possibleElements = possibleElements.Intersect(toIntersect).ToArray();
    }
    public void SetRandomElement()
    {
        mapElement = possibleElements[Random.Range(0, possibleElements.Length)];
    }
    public bool AddEdge(Direction id, int adjacentVertex, int weight)
    {
        if (!ExistEdge(adjacentVertex, weight, out Edge edge))
        {
            edges.Add(new Edge(id, adjacentVertex, weight));
            return true;
        }
        Debug.LogWarning($"Can't Add Edge [{this.id} -> {adjacentVertex} (w {weight})]: there's already one");
        return false;
    }
    public bool RemoveEdge(int adjacentVertex, int weight)
    {
        if (ExistEdge(adjacentVertex, weight, out Edge edge))
        {
            edges.Remove(edge);
            return true;
        }
        Debug.LogWarning($"Can't Remove Edge [{this.id} -> {adjacentVertex} (w {weight}]]: it doesn't exist");
        return false;
    }
    public void RemoveAllEdges()
    {
        edges.Clear();
        Debug.Log($"Vertex {this.id} edges cleared");
    }
    public int CountEdges(int adjacentVertex)
    {
        int c = 0;
        foreach (Edge edge in edges)
        {
            if (edge.adjacentVertex.id == adjacentVertex)
                c++;
        }
        return c;
    }
    public string Print(bool printWeight)
    {
        StringBuilder str = new StringBuilder();
        str.Append($"V[{id}]: ");

        foreach(Edge edge in edges)
        {
            str.Append($" [{edge.id} - {edge.adjacentVertex.id}]");
            if (printWeight)
                str.Append($"[w{edge.weight}]");
        }
        str.Append("\n");
        return str.ToString();
    }
    bool ExistEdge(int adjacentVertex, int weight, out Edge edge)
    {
        edge = edges.Find(e => e.adjacentVertex.id == adjacentVertex && e.weight == weight);
        if (edge != null)
            return true;
        return false;
    }
}
