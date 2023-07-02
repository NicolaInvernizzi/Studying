using System.Collections.Generic;
using UnityEngine;

public class AdvanceGraph : MonoBehaviour
{
    public List<GraphNode> nodes;
    public AdvanceGraph() 
    {
        nodes = new List<GraphNode>();
    }
    public bool AddNode(int node)
    {
        if (!nodes.Exists(n => n.node == node))
        {
            nodes.Add(new GraphNode(node));
            return true;
        }
        return false;
    }
    public bool AddEdge(int node, int neighbor)
    {
        if (ExistGraphNode(node, out GraphNode g1) && ExistGraphNode(neighbor, out GraphNode g2))
        {
            g1.AddNeighbor(g2);
            return true;
        }
        return false;
    }
    public bool RemoveNode(int node)
    {
        if (ExistGraphNode(node, out GraphNode g))
        { 
            nodes.Remove(g);
            nodes.ForEach(i => i.neighbors.Remove(i.neighbors.Find(n => n.node == node)));
            return true;
        }
        return false;
    }
    public bool RemoveEdge(int node, int neighbor)
    {
        if (ExistGraphNode(node, out GraphNode g1) && ExistGraphNode(neighbor, out GraphNode g2))
        {
            g1.RemoveNeighbor(g2);
            return true;
        }
        return false;
    }
    public void Clear()
    {
        nodes.ForEach(n => n.RemoveAllNeighbors());
        nodes.Clear();
    }
    public override string ToString()
    {
        return "";
    }
    bool ExistGraphNode(int node, out GraphNode graphNode)
    {
        graphNode = null;
        if (nodes.Exists(n => n.node == node))
        {
            graphNode = nodes.Find(n => n.node == node);
            return true;
        }
        return false;
    }
}
