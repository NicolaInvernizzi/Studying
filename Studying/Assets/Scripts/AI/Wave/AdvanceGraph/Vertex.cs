using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Vertex
{
    public readonly int id;
    public List<Edge> edges { get; private set; }
    public Vertex(int id)
    {
        this.id = id;
        edges = new List<Edge>();
    }

    public bool AddEdge(int adjacentVertex, int weight)
    {
        if (!ExistEdge(adjacentVertex, weight, out Edge edge))
        {
            edges.Add(new Edge(adjacentVertex, weight));
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
    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append($"Vertex {id} linked to: ");
        edges.ForEach(e => str.Append($"id[{e.adjacentVertex.id}]-w[{e.weight}], "));
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
