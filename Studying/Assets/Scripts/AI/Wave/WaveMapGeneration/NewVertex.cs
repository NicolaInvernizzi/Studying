using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NewVertex
{
    public int id;
    public List<Edge> edges;
    public Element currentElement;
    public Element[] possibleElements;
    public List<Element> triedElements;
    public float xPosition;
    public float zPosition;
    public bool updated;
    public NewVertex(int id, Element[] possibleElements, float xPosition, float zPosition)
    {
        this.id = id;
        this.possibleElements = possibleElements;
        edges = new List<Edge>();
        this.xPosition = xPosition;
        this.zPosition = zPosition;
    }

    public void InstantiatePrefab()
    {
        Debug.Log($"x:{xPosition} y:{zPosition}");
        MonoBehaviour.Instantiate(currentElement.prefab, new Vector3(xPosition, 0, zPosition), Quaternion.identity);
    }
    public void RemovePossibleElement(Element element)
    {
        possibleElements = possibleElements.Where(e => e != element).ToArray();
    }
    public void SetUpMapRules(Element[] toIntersect)
    {
        possibleElements = possibleElements.Intersect(toIntersect).ToArray();
    }
    public void SetRandomElement()
    {
        currentElement = possibleElements[Random.Range(0, possibleElements.Length)];
        possibleElements = possibleElements.Where(e => e == currentElement).ToArray();

        Debug.Log($"V{id}, E:{currentElement.name}");
        MapGeneration.instance.currentElement = currentElement;

        UpdateAdjacent();
    }
    void UpdateAdjacent()
    {
        foreach(Element element in possibleElements)
        {
            foreach (Edge edge in edges)
                edge.adjacentVertex.ModifyPossibleElements(element.rules.First(r => r.direction == edge.id).constraints);
        }
    }
    void ModifyPossibleElements(Element[] toIntersect)
    {
        if (MapGeneration.instance.stopUpdating || currentElement != null || updated)
            return;

        if (possibleElements.Length == 0)
        {
            MapGeneration.instance.stopUpdating = true;
            return;
        }

        // UPDATE this vertex infos
        int prevPossibleElements = possibleElements.Length;
        possibleElements = possibleElements.Intersect(toIntersect).ToArray();
        updated = true;

        if (prevPossibleElements != possibleElements.Length)
            UpdateAdjacent();
    }
    public void AddEdge(Direction id, int adjacentNewVertex, int weight)
    {
        edges.Add(new Edge(id, adjacentNewVertex, weight));
    }
    public string Print1(bool printWeight)
    {
        StringBuilder str = new StringBuilder();
        str.Append($"V[{id}]: ");

        foreach (Edge edge in edges)
        {
            str.Append($" [{edge.id} - {edge.adjacentVertex.id}]");
            if (printWeight)
                str.Append($"[w{edge.weight}]");
        }
        str.Append("\n");
        return str.ToString();
    }
    public string Print2()
    {
        StringBuilder str = new StringBuilder();

        string txt = currentElement == null ? "X" : currentElement.name;
        str.Append($"id [{this.id}] - Element [{txt}] - Possibles ");

        if (possibleElements != null)
        {
            foreach (Element i in this.possibleElements)
                str.Append($"[{i.name}]");
        }
        else
            str.Append("null");

        str.Append("\n");
        return str.ToString();
    }
}
