using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    public int node;
    public List<GraphNode> neighbors;
    public GraphNode(int value)
    {
        this.node = value;
        neighbors = new List<GraphNode>();
    }
    public bool AddNeighbor(GraphNode neighbor)
    {
        if (!neighbors.Contains(neighbor))
        {
            neighbors.Add(neighbor);
            return true;
        }
        return false;
    }
    public bool RemoveNeighbor(GraphNode neighbor) 
    {
        if (neighbors.Contains(neighbor))
        {
            neighbors.Remove(neighbor);
            return true;
        }
        return false;
    }
    public void RemoveAllNeighbors() => neighbors.Clear();
    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append($"Node {node} with neighbors: ");
        foreach (GraphNode neighbor in neighbors)
            str.Append($"{neighbor.node}, ");
        return str.ToString();
    }
}
