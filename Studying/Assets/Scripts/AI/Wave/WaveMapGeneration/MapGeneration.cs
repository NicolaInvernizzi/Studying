using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MapGeneration : MonoBehaviour
{
    public static MapGeneration instance;
    public int height;
    public int lenght;
    public Element[] mapElements;
    public List<Rule> mapRules;
    public Text adjacencyList_Text;
    public Text verticesInfos_Text;
    Stack<List<NewVertex>> prevVertices = new Stack<List<NewVertex>>();
    [HideInInspector] public List<NewVertex> vertices = new List<NewVertex>();
    NewVertex currentVertex;

    private void Start()
    {
        instance = this;
        GraphGeneration(height, lenght);
        PrintVerticesInfos();
    }
    private void OnGUI()
    {
        if (GUILayout.Button("AdjacencyList"))
            PrintAdjacencyList();
        if (GUILayout.Button("UpdateMap"))
            UpdateMap();
    }
    public void RemoveVertex(int vertex)
    {
        vertices.Remove(vertices.Find(i => i.id == vertex));
        vertices.ForEach(i => i.edges.Remove(i.edges.Find(v => v.adjacentVertex.id == vertex)));
    }
    public void PrintAdjacencyList()
    {
        Sort(vertices);
        StringBuilder str = new StringBuilder();
        str.AppendLine("Adjacency List:");
        vertices.ForEach(v => str.Append(v.Print1(false)));
        adjacencyList_Text.text = str.ToString();
    }
    public void UpdateMap()
    {
        prevVertices.Push(vertices);
        currentVertex = vertices[Random.Range(0, vertices.Count)];
        currentVertex.SetRandomElement();
        vertices.ForEach(v => v.updated = false);
        PrintVerticesInfos();
    }
    public void GraphGeneration(int height, int lenght)
    {
        int[,] matrixMap = new int[height, lenght];
        int id = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < lenght; j++)
            {
                matrixMap[i, j] = id;

                NewVertex currentNewVertex = CreateNewVertex(id, mapElements);

                if (i == 0)
                    currentNewVertex.ModifyPossibleElements(mapRules.First(r => r.direction == Direction.Up).constraints);
                else if (i == height - 1)
                    currentNewVertex.ModifyPossibleElements(mapRules.First(r => r.direction == Direction.Down).constraints);

                if (j == 0)
                    currentNewVertex.ModifyPossibleElements(mapRules.First(r => r.direction == Direction.Left).constraints);
                else if (j == lenght - 1)
                    currentNewVertex.ModifyPossibleElements(mapRules.First(r => r.direction == Direction.Right).constraints);

                id++;
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
    public NewVertex CreateNewVertex(int newVertex, Element[] possibleElements)
    {
        NewVertex currentNewVertex = new NewVertex(newVertex, possibleElements);
        vertices.Add(currentNewVertex);
        return currentNewVertex;
    }
    public void CreateDoubleEdge(Direction id, int newVertex1, int newVertex2, int weight)
    {
        Direction inverseEdgeId;
        NewVertex v1 = vertices.Find(i => i.id == newVertex1);
        NewVertex v2 = vertices.Find(i => i.id == newVertex2);

        v1.AddEdge(id, v2.id, weight);

        switch (id)
        {
            case Direction.Right:
                inverseEdgeId = Direction.Left;
                break;
            default:
                inverseEdgeId = Direction.Up;
                break;
        }
        v2.AddEdge(inverseEdgeId, v1.id, weight);
    }
    void PrintVerticesInfos()
    {
        StringBuilder str = new StringBuilder();
        vertices.ForEach(v => str.Append(v.Print2()));
        verticesInfos_Text.text = str.ToString();
    }
    void Sort(List<NewVertex> toSort)
    {
        NewVertex min;
        NewVertex swap;

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
