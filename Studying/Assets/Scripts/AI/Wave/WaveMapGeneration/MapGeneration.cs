using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapGeneration : MonoBehaviour
{
    public static MapGeneration instance;
    public int height;
    public int lenght;
    public float elementSize;
    public List<Element> mapElements;
    public List<Rule> mapRules;
    public Text adjacencyList_Text;
    public Text verticesInfos_Text;
    [HideInInspector] public List<NewVertex> vertices = new List<NewVertex>();
    NewVertex currentVertex;
    [HideInInspector] public bool stopUpdating;
    [HideInInspector] public Element currentElement;
    public int testing = 0;

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
        if (GUILayout.Button("PrintVerticesInfos"))
            PrintVerticesInfos();
        if (GUILayout.Button("CreateMap"))
            CreateMap();
        if (GUILayout.Button("NewVertex"))
            NNN();
        if (GUILayout.Button("Wave"))
            WWW();
    }
    public void CreateMap()
    {
        vertices.ForEach(v => v.InstantiatePrefab());
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
    public void NNN()
    {
        LowestEntropy();
        currentVertex.WaveGeneration();
    }
    public void WWW()
    {
        List<NewVertex> waveVertices = vertices.Where(v => v.inWave == true).ToList();

        foreach (NewVertex v in waveVertices)
        {
            if (v.inWave)
                v.UpdateAdjacent();
        }
        vertices.ForEach(v => v.newPossibles.Clear());

        if (!vertices.Exists(v => v.inWave == true))
        {
            vertices.ForEach(v => v.updated = false);
            foreach (NewVertex v in vertices)
            {
                if (v.currentElement == null && v.possibleElements.Count == 1)
                {
                    Debug.Log($"V{v.id} {v.possibleElements[0]}");
                    v.currentElement = v.possibleElements[0];
                }
            }
            Debug.LogWarning("EndWave");
        }
        PrintVerticesInfos();
    }
    public void UpdateMap()
    {
        while (vertices.Exists(v => v.currentElement == null))
        {
            LowestEntropy();
            currentVertex.WaveGeneration();

            while (vertices.Exists(v => v.inWave == true))
            {
                List<NewVertex> waveVertices = vertices.Where(v => v.inWave == true).ToList();

                foreach (NewVertex v in waveVertices)
                {
                    if (v.inWave)
                        v.UpdateAdjacent();
                }
                vertices.ForEach(v => v.newPossibles.Clear());
            }
            vertices.ForEach(v => v.updated = false);
            PrintVerticesInfos();
        }
    }
    void LowestEntropy()
    {
        int lowest = mapElements.Count;

        foreach(NewVertex v in vertices)
        {
            if (v.currentElement == null && v.possibleElements.Count < lowest)
                lowest = v.possibleElements.Count;
        }
        List<NewVertex> lowestV = vertices.FindAll(v => v.possibleElements.Count == lowest);
        NewVertex randomVertex = lowestV[Random.Range(0, lowestV.Count)];
        currentVertex = randomVertex;
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

                CreateNewVertex(id, mapElements, elementSize * j - 50, elementSize * -i);
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
    public void CreateNewVertex(int newVertex, List<Element> possibleElements, float xPosition, float zPosition)
    {
        vertices.Add(new NewVertex(newVertex, possibleElements, xPosition, zPosition));
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
