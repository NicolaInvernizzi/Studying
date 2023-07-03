using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GraphNode
{
    public int id;
    public List<GraphNode> linkedNodes;
    public GraphNode(int id)
    {
        this.id = id;
        linkedNodes = new List<GraphNode>();
    }
    public bool AddLinkedNode(GraphNode neighbor)
    {
        if (!linkedNodes.Contains(neighbor))
        {
            linkedNodes.Add(neighbor);
            return true;
        }
        return false;
    }
    public bool RemoveLinkedNode(GraphNode neighbor) 
    {
        if (linkedNodes.Contains(neighbor))
        {
            linkedNodes.Remove(neighbor);
            return true;
        }
        return false;
    }
    public void RemoveAllLinkedNode() => linkedNodes.Clear();
    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append($"Node {id} with LinkedNodes: ");
        foreach (GraphNode neighbor in linkedNodes)
            str.Append($"{neighbor.id}, ");
        str.Append("\n");
        return str.ToString();
    }
}
