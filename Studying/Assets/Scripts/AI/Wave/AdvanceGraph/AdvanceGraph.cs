using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AdvanceGraph : MonoBehaviour
{
    public List<Vertex> vertices { get; private set; }
    public int[,] adjanceyMatrix { get; private set; }
    public AdvanceGraph() 
    {
        vertices = new List<Vertex>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.CreateVertex(3);
            this.CreateVertex(1);
            this.CreateVertex(6);
            this.CreateVertex(4);
            this.CreateMonoEdge(1, 4, 5);
            this.CreateMonoEdge(1, 4, 2);
            this.CreateMonoEdge(1, 4, 1);
            this.CreateMonoEdge(1, 3, 1);
            this.CreateMonoEdge(1, 3, 4);
            this.CreateDoubleEdge(6, 4, 2);
            this.CreateMonoEdge(6, 4, 2);
            this.CreateMonoEdge(6, 4, 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            this.AdjacencyList();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            this.PrintAdjacencyMatrix();
        }
    }

    public bool CreateVertex(int vertex)
    {
        if (!ExistVertex(vertex, out Vertex v))
        {
            vertices.Add(new Vertex(vertex));
            return true;
        }
        Debug.LogWarning($"Can't Create Vertex {vertex}: there's already one");
        return false;
    }
    public bool CreateMonoEdge(int fromVertex, int toVertex, int weight)
    {
        if (ExistVertex(fromVertex, out Vertex v1) && ExistVertex(toVertex, out Vertex v2))
        {
            v1.AddEdge(v2.id, weight);
            return true;
        }
        Debug.LogWarning($"Can't Create Mono Edge [{fromVertex} -> {toVertex} (w {weight})]:" +
            $" Vertex {(v1 == null ? fromVertex.ToString() : toVertex.ToString())} doesn't exist");
        return false;
    }
    public bool CreateDoubleEdge(int vertex1, int vertex2, int weight)
    {
        if (ExistVertex(vertex1, out Vertex v1) && ExistVertex(vertex2, out Vertex v2))
        {
            v1.AddEdge(v2.id, weight);
            v2.AddEdge(v1.id, weight);
            return true;
        }
        Debug.LogWarning($"Can't Create Double Edge [{vertex1} <-> {vertex2} (w {weight})]: " +
            $"Vertex {(v1 == null? vertex1.ToString() : vertex2.ToString())} doesn't exist");
        return false;
    }
    public bool RemoveVertex(int vertex)
    {
        if (ExistVertex(vertex, out Vertex v))
        {
            vertices.Remove(v);
            vertices.ForEach(i => i.edges.Remove(i.edges.Find(v => v.adjacentVertex.id == vertex)));
            return true;
        }
        Debug.LogWarning($"Can't remove Vertex {vertex}: it does not exist");
        return false;
    }
    public bool RemoveEdge(int vertex, int adjacentVertex, int weight)
    {
        if (ExistVertex(vertex, out Vertex v1))
        {
            if (v1.RemoveEdge(weight, adjacentVertex))
                return true;
        }
        Debug.LogWarning($"Can't remove Edge [{vertex} -> {adjacentVertex} (w {weight})]: it does not exist");
        return false;
    }
    public void ClearGraph()
    {
        vertices.ForEach(v => v.RemoveAllEdges());
        vertices.Clear();
        Debug.Log("Graph cleared");
    }
    public void AdjacencyList()
    {
        Sort(vertices);
        StringBuilder str = new StringBuilder();
        str.AppendLine("Adjacency List:");
        vertices.ForEach(v => str.Append(v.ToString()));
        print(str);
    }

    bool ExistVertex(int vertex, out Vertex v)
    {
        v = vertices.Find(i => i.id == vertex);
        if (v == null)
            return false;
        return true;
    }
    void CreateAdjacencyMatrix()
    {
        adjanceyMatrix = new int[vertices.Count, vertices.Count];
        Sort(vertices);

        for (int i = 0; i < vertices.Count; i++)
        {
            if (vertices[i].edges.Count > 0)
            {
                for (int j = 0; j < vertices[i].edges.Count; j++)
                    adjanceyMatrix[i, vertices.FindIndex(v => v.id == vertices[i].edges[j].adjacentVertex.id)] 
                        = vertices[i].CountEdges(vertices[i].edges[j].adjacentVertex.id);
            }
        }
    }
    void PrintAdjacencyMatrix()
    {
        CreateAdjacencyMatrix();
        StringBuilder str = new StringBuilder();

        str.Append("Adjacency Matrix (number of edge per vertex):\n");
        str.Append("   ");
        vertices.ForEach(v => str.Append($"[{v.id}]"));
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
