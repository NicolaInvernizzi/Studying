using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MapGeneration : MonoBehaviour
{
    public Transform mapPosition;
    public float generationSpeed;
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
    bool stopLoop;
    int generationTry;
    Coroutine mainLoop;
    Coroutine updateLoop;
    Coroutine waveLoop;

    private void Start()
    {
        instance = this;
        GraphGeneration(height, lenght);
        PrintVerticesInfos();
    }
    private void OnGUI()
    {
        if (GUILayout.Button("UpdateMap"))
        {
            if (mainLoop == null)
                mainLoop = StartCoroutine(MainLoop());
        }
    }
    void ResetGraph()
    {
        vertices.Clear();
        currentVertex = null;
        GraphGeneration(height, lenght);
        PrintVerticesInfos();
    }
    void CreateMap()
    {
        vertices.ForEach(v => v.InstantiatePrefab());
    }
    IEnumerator MainLoop()
    {
        stopLoop = false;
        generationTry = 1;
        do
        {
            yield return new WaitForSeconds(1);
            updateLoop = StartCoroutine(UpdateMap());
        } while (!stopLoop);
        Debug.Log($"Map Created in {generationTry} {(generationTry == 1?"attempt":"attempts")}");
        mainLoop = null;
    }
    IEnumerator UpdateMap()
    {
        while (vertices.Exists(v => v.currentElement == null))
        {
            if (waveLoop == null)
            {
                LowestEntropy();
                if (currentVertex.WaveGeneration())
                {
                    waveLoop = StartCoroutine(Wave());
                }
                else
                {
                    ResetGraph();
                    generationTry++;
                    Debug.LogWarning("Error during generation");
                    foreach (Transform child in mapPosition)
                        Destroy(child.gameObject);
                    yield break;
                }
            }
            yield return new WaitForSeconds(generationSpeed);
        }
        stopLoop = true;
        //CreateMap();
        updateLoop = null;
    }
    IEnumerator Wave()
    {
        while (vertices.Exists(v => v.inWave == true))
        {
            List<NewVertex> waveVertices = vertices.Where(v => v.inWave == true).ToList();

            foreach (NewVertex v in waveVertices)
            {
                if (v.inWave)
                    v.UpdateAdjacent();
            }
            vertices.ForEach(v => v.newPossibles.Clear());
            yield return null;
        }
        vertices.ForEach(v => v.updated = false);
        PrintVerticesInfos();
        waveLoop = null;
    }
    [ContextMenu("Test")]
    public void Test()
    {
        Debug.Log(updateLoop == null);
        Debug.Log(mainLoop == null);
        Debug.Log(waveLoop == null);
    }
    void LowestEntropy()
    {
        int lowest = mapElements.Count;

        foreach(NewVertex v in vertices)
        {
            if (v.currentElement == null && v.possibleElements.Count < lowest)
                lowest = v.possibleElements.Count;
        }
        List<NewVertex> lowestV = vertices.FindAll(v => v.possibleElements.Count == lowest && v.currentElement == null);
        NewVertex randomVertex = lowestV[Random.Range(0, lowestV.Count)];
        currentVertex = randomVertex;
    }
    void GraphGeneration(int height, int lenght)
    {
        int[,] matrixMap = new int[height, lenght];
        int id = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < lenght; j++)
            {
                matrixMap[i, j] = id;

                vertices.Add(new NewVertex(id, mapElements, elementSize * j, elementSize * -i));
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
        ApplyMapRules();
    }
    void ApplyMapRules()
    {
        foreach(Rule r in mapRules)
        {
            foreach(NewVertex v in vertices)
            {
                if (!v.edges.Exists(e => e.id == r.direction))
                    v.SetUpMapRules(r.constraints);
            }
        }
    }
    void CreateDoubleEdge(Direction id, int newVertex1, int newVertex2, int weight)
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
    void PrintAdjacencyList()
    {
        Sort(vertices);
        StringBuilder str = new StringBuilder();
        str.AppendLine("Adjacency List:");
        vertices.ForEach(v => str.Append(v.Print1(false)));
        adjacencyList_Text.text = str.ToString();
    }
    public void RemoveVertex(NewVertex vertex)
    {
        vertices.Remove(vertices.Find(i => i == vertex));
        vertices.ForEach(i => i.edges.Remove(i.edges.Find(v => v.adjacentVertex == vertex)));
    }
}
