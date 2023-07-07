using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AdvanceGraph : MonoBehaviour
{
    public int debugVertex;
    public int height;
    public int lenght;
    public Text adjacencyList_Text;
    public Text adjacencyMatrix_Text;
    public Text verticesInfos_Text;
    public static List<Vertex> vertices { get; private set; }
    public int[,] adjanceyMatrix { get; private set; }
    public MapElement[] mapElements;
    public List<MapRule> mapRoles;
    public int[] elementsIds;
    public AdvanceGraph() 
    {
        vertices = new List<Vertex>();
    }

    private void Start()
    {
        elementsIds = GetElementsIds();
    }
    private void OnGUI()
    {
        if (GUILayout.Button("AdjacencyList"))
            PrintAdjacencyList();
        if (GUILayout.Button("MapGeneration"))
            MapGeneratio();
        if (GUILayout.Button("DebugVertices"))
            PrintVerticesInfos();
    }
    public MapElement GetMapElement(int id)
    {
        return mapElements.First(e => e.id == id);
    }
    public void MapGeneratio()
    {
        ClearGraph();
        GraphGeneration(height, lenght);

        Vertex currentVertex = vertices[Random.Range(0, vertices.Count)];
        currentVertex.SetRandomElement();

        print($"{currentVertex.id} {currentVertex.mapElement}");

        MapElement mapElement = GetMapElement(currentVertex.mapElement);
        currentVertex.UpdateAdjacent(mapElement);
    }
    public void DebugVertex()
    {
        Vertex toDebug = vertices.First(v => v.id == debugVertex);

        print($"{toDebug.id} PossibleElements:");
        foreach(int i in toDebug.possibleElements)
            print(i);
        print($"{toDebug.id} element: {toDebug.mapElement}");
    }
    public MapElement FindElement(int element)
    {
        MapElement mapElement;
        try
        {
            mapElement = mapElements.First(e => e.id == element);
        }
        catch
        {
            mapElement = null;
        }
        return mapElement;
    }
    public int[] GetElementsIds()
    {
        int[] elements = new int[mapElements.Length];
        for(int i = 0; i < elements.Length; i++)
            elements[i] = mapElements[i].id;
        return elements;
    }
    public void GraphGeneration(int height, int lenght)
    {
        int[,] matrixMap = new int[height, lenght];
        int id = 0;
        int[] possibleElements = elementsIds;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < lenght; j++)
            {
                matrixMap[i, j] = id;

                Vertex currentVertex = CreateVertex(id, possibleElements);

                if (i == 0)
                    currentVertex.ModifyPossibleElements(mapRoles.First(r => r.direction == Direction.Up).constraints);
                else if (i == height - 1)
                    currentVertex.ModifyPossibleElements(mapRoles.First(r => r.direction == Direction.Down).constraints);

                if (j == 0)
                    currentVertex.ModifyPossibleElements(mapRoles.First(r => r.direction == Direction.Left).constraints);
                else if (j == lenght - 1)
                    currentVertex.ModifyPossibleElements(mapRoles.First(r => r.direction == Direction.Right).constraints);

                id++;

                if (possibleElements != elementsIds)
                    possibleElements = elementsIds;
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < lenght - 1; j++)
                CreateDoubleEdge(Direction.Right, matrixMap[i, j], matrixMap[i, j + 1], 1);
        }

        for (int j = 0; j < lenght; j++)
        {
            for (int i = 0; i < height - 1; i++)
                CreateDoubleEdge(Direction.Down, matrixMap[i, j], matrixMap[i + 1, j], 1);
        }
    }
    public Vertex CreateVertex(int vertex, int[] possibleElements)
    {
        if (!ExistVertex(vertex, out Vertex v))
        {
            Vertex currentVertex = new Vertex(vertex, possibleElements);
            vertices.Add(currentVertex);
            return currentVertex;
        }
        Debug.LogWarning($"Can't Create Vertex {vertex}: there's already one");
        return null;
    }
    public bool CreateMonoEdge(Direction edgeId, int fromVertex, int toVertex, int weight)
    {
        if (ExistVertex(fromVertex, out Vertex v1) && ExistVertex(toVertex, out Vertex v2))
        {
            v1.AddEdge(edgeId, v2.id, weight);
            return true;
        }
        Debug.LogWarning($"Can't Create Mono Edge [{fromVertex} -> {toVertex} (w {weight})]:" +
            $" Vertex {(v1 == null ? fromVertex.ToString() : toVertex.ToString())} doesn't exist");
        return false;
    }
    public bool CreateDoubleEdge(Direction id,int vertex1, int vertex2, int weight)
    {
        Direction inverseEdgeId;
        if (ExistVertex(vertex1, out Vertex v1) && ExistVertex(vertex2, out Vertex v2))
        {
            v1.AddEdge(id, v2.id, weight);

            switch(id)
            {
                case Direction.Right:
                    inverseEdgeId = Direction.Left;
                    break;
                default:
                    inverseEdgeId = Direction.Up;
                    break;
            }
            v2.AddEdge(inverseEdgeId, v1.id, weight);
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
    public void PrintAdjacencyList()
    {
        Sort(vertices);
        StringBuilder str = new StringBuilder();
        str.AppendLine("Adjacency List:");
        vertices.ForEach(v => str.Append(v.Print1(false)));
        adjacencyList_Text.text = str.ToString();
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
        adjacencyMatrix_Text.text = str.ToString();
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
    void PrintVerticesInfos()
    {
        StringBuilder str = new StringBuilder();
        vertices.ForEach(v => str.Append(v.Print2()));
        verticesInfos_Text.text = str.ToString(); 
    }
}
public enum Direction
{
    None,
    Up, 
    Down, 
    Left, 
    Right
}
