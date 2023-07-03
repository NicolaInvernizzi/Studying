using System;
using System.Collections.Generic;
using System.Text;

public class Vertex
{
    public int id;
    public List<Edge> edges;
    public Vertex(int id)
    {
        this.id = id;
        edges = new List<Edge>();
    }
    public bool AddEdge(int adjacentVertex, int weight)
    {
        if (!FindEdge(adjacentVertex, weight, out Edge edge))
        {
            edges.Add(edge);
            return true;
        }
        return false;
    }//ok
    public bool RemoveEdge(int adjacentVertex, int weight) //ok
    {
        if (FindEdge(adjacentVertex, weight, out Edge edge))
        {
            edges.Remove(edge);
            return true;
        }
        return false;
    }
    bool FindEdge(int adjacentVertex, int weight, out Edge edge)
    {
        edge = edges.Find(e => e.adjacentVertex.id == adjacentVertex && e.weight == weight);
        if (edge != null)
            return true;
        return false;
    }
    public void RemoveAllEdges() => edges.Clear();//ok
    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append($"Node {id} with AdjacentVertices: ");
        edges.ForEach(e => str.Append($"{e.adjacentVertex.id}-W{e.weight}, "));
        str.Append("\n");
        return str.ToString();
    }//ok
}
