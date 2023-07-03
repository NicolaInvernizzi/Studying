using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AdvanceGraph : MonoBehaviour
{
    public List<GraphNode> nodes;
    int[,] adjanceyMatrix;
    public AdvanceGraph() 
    {
        nodes = new List<GraphNode>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.AddNode(1);
            this.AddNode(3);
            this.AddNode(3);
            this.AddNode(6);
            this.AddNode(4);
            this.AddEdge(4, 6);
            this.AddEdge(1, 8);
            this.AddEdge(3, 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.GraphStatus();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            this.PrintAdjanceyMatrix();
        }
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    this.RemoveEdge(3, 1);
        //}
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    this.RemoveNode(8);
        //    this.RemoveNode(1);
        //}
    }
    public bool AddNode(int node)
    {
        if (!nodes.Exists(n => n.id == node))
        {
            nodes.Add(new GraphNode(node));
            return true;
        }
        return false;
    }
    public bool AddEdge(int node, int linkedNode)
    {
        if (ExistNode(node, out GraphNode g1) && ExistNode(linkedNode, out GraphNode g2))
        {
            g1.AddLinkedNode(g2);
            return true;
        }
        return false;
    }
    public bool RemoveNode(int node)
    {
        if (ExistNode(node, out GraphNode g))
        { 
            nodes.Remove(g);
            nodes.ForEach(i => i.linkedNodes.Remove(i.linkedNodes.Find(n => n.id == node)));
            return true;
        }
        return false;
    }
    public bool RemoveEdge(int node, int linkedNode)
    {
        if (ExistNode(node, out GraphNode g1) && ExistNode(linkedNode, out GraphNode g2))
        {
            g1.RemoveLinkedNode(g2);
            return true;
        }
        return false;
    }
    public void Clear()
    {
        nodes.ForEach(n => n.RemoveAllLinkedNode());
        nodes.Clear();
    }
    public void GraphStatus()
    {
        StringBuilder str = new StringBuilder();
        nodes.ForEach(n => str.Append(n.ToString()));
        print(str);
    }
    bool ExistNode(int node, out GraphNode graphNode)
    {
        graphNode = null;
        if (nodes.Exists(n => n.id == node))
        {
            graphNode = nodes.Find(n => n.id == node);
            return true;
        }
        return false;
    }
    void CreateAdjanceyMatrix()
    {
        adjanceyMatrix = new int[nodes.Count, nodes.Count];
        Sort(nodes);

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].linkedNodes.Count != 0)
            {
                for (int j = 0; j < nodes[i].linkedNodes.Count; j++)
                {
                    adjanceyMatrix[i, nodes.IndexOf(nodes[i].linkedNodes[j])] = 1;
                }
            }
        }
    }
    void PrintAdjanceyMatrix()
    {
        CreateAdjanceyMatrix();
        StringBuilder str = new StringBuilder();

        str.Append("   ");
        foreach (GraphNode node in nodes)
            str.Append($"[{node.id}]");

        str.Append("\n");

        for (int raw = 0; raw < nodes.Count; raw++)
        {
            str.Append($"[{nodes.ElementAt(raw).id}]");
            for (int column = 0; column < nodes.Count; column++)
            {
                str.Append($" {adjanceyMatrix[raw, column]} ");
            }
            str.Append("\n");
        }
        print(str);
    }
    void Sort(List<GraphNode> toSort)
    {
        GraphNode min;
        GraphNode swap;

        for (int j = 0; j < toSort.Count; j++)
        {
            min = toSort[j];
            for (int i = j; i < toSort.Count; i++)
            {
                if (toSort[i].id < min.id)
                    min = toSort[i];
            }
            swap = toSort[j];
            toSort[toSort.IndexOf(min)] = swap;
            toSort[j] = min;
        }
    }
}
