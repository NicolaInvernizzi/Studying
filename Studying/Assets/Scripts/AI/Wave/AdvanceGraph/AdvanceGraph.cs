using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class AdvanceGraph : MonoBehaviour
{
    public List<Vertex> vertices;
    int[,] adjanceyMatrix;
    public AdvanceGraph() 
    {
        vertices = new List<Vertex>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.AddVertex(1);
            this.AddVertex(3);
            this.AddVertex(3);
            this.AddVertex(6);
            this.AddVertex(4);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.GraphStatus();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            this.PrintAdjanceyMatrix();
        }
    }
    public bool AddVertex(int vertex)
    {
        if (!vertices.Exists(v => v.id == vertex))
        {
            vertices.Add(new Vertex(vertex));
            return true;
        }
        return false;
    }//ok
    public bool AddEdge(int vertex, int adjacentVertex, int weight, bool outgoing, bool incoming)
    {
        if (ExistVertex(vertex, out Vertex v1) && ExistVertex(adjacentVertex, out Vertex v2))
        {
            if (outgoing)
                v1.AddEdge(adjacentVertex, weight);
            if (incoming)
                v2.AddEdge(adjacentVertex, weight);
            return true;
        }
        return false; 
    }//ok

    //public bool RemoveVertex(int vertex)
    //{
    //    if (ExistVertex(vertex, out Vertex v))
    //    {
    //        vertices.Remove(v);
    //        vertices.ForEach(i => i.adjacentVertices.Remove(i.adjacentVertices.Find(v => v.id == vertex)));
    //        return true;
    //    }
    //    return false;
    //}
    public bool RemoveEdge(int vertex, int adjacentVertex, int weight)
    {
        if (ExistVertex(vertex, out Vertex v1))
        {
            if(v1.RemoveEdge(weight, adjacentVertex))
                return true;
        }
        return false;
    }// ok
    public void Clear()
    {
        vertices.ForEach(v => v.RemoveAllEdges());
        vertices.Clear();
    }//ok
    public void GraphStatus()
    {
        StringBuilder str = new StringBuilder();
        vertices.ForEach(v => str.Append(v.ToString()));
        print(str);
    }
    bool ExistVertex(int vertex, out Vertex v)
    {
        v = vertices.Find(v => v.id == vertex);
        if (v != null)
            return true;
        return false;
    }
    //void CreateAdjanceyMatrix()
    //{
    //    adjanceyMatrix = new int[vertices.Count, vertices.Count];
    //    Sort(vertices);

    //    for (int i = 0; i < vertices.Count; i++)
    //    {
    //        if (vertices[i].adjacentVertices.Count != 0)
    //        {
    //            for (int j = 0; j < vertices[i].adjacentVertices.Count; j++)
    //            {
    //                adjanceyMatrix[i, vertices.IndexOf(vertices[i].adjacentVertices[j])] = 1;
    //            }
    //        }
    //    }
    //}
    void PrintAdjanceyMatrix()
    {
        //CreateAdjanceyMatrix();
        StringBuilder str = new StringBuilder();

        str.Append("   ");
        foreach (Vertex v in vertices)
            str.Append($"[{v.id}]");

        str.Append("\n");

        for (int raw = 0; raw < vertices.Count; raw++)
        {
            str.Append($"[{vertices.ElementAt(raw).id}]");
            for (int column = 0; column < vertices.Count; column++)
            {
                str.Append($" {adjanceyMatrix[raw, column]} ");
            }
            str.Append("\n");
        }
        print(str);
    }
    void Sort(List<Vertex> toSort)
    {
        Vertex min;
        Vertex swap;

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
