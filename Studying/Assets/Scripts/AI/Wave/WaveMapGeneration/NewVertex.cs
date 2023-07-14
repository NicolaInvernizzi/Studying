using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

public class NewVertex
{
    public int id;
    public List<Edge> edges;
    public Element currentElement;
    public List<Element> possibleElements;
    public List<Element> triedElements;
    public float xPosition;
    public float zPosition;
    public bool updated;
    public bool inWave = false;
    public List<Element> newPossibles = new List<Element>();
    public NewVertex(int id, List<Element> possibleElements, float xPosition, float zPosition)
    {
        this.id = id;
        this.possibleElements = possibleElements;
        edges = new List<Edge>();
        this.xPosition = xPosition;
        this.zPosition = zPosition;
    }
    public void InstantiatePrefab()
    {
        MonoBehaviour.Instantiate(
            currentElement.prefab,
            MapGeneration.instance.mapPosition.position + new Vector3(xPosition, 0, zPosition), 
            currentElement.prefab.transform.rotation, 
            MapGeneration.instance.mapPosition);
    }
    public void SetUpMapRules(Element[] toIntersect)
    {
        possibleElements = possibleElements.Intersect(toIntersect).ToList();
    }
    public bool WaveGeneration()
    {
        try
        {
            currentElement = possibleElements[Random.Range(0, possibleElements.Count)];
        }
        catch
        {
            return false;
        }
        InstantiatePrefab();
        possibleElements = possibleElements.Where(e => e == currentElement).ToList();
        inWave = true;
        return true;
    }
    public void UpdateAdjacent()
    {
        updated = true;
        foreach (Element element in possibleElements)
        {
            foreach (Edge edge in edges)
            {
                edge.adjacentVertex.ModifyPossibleElements(
                    element.rules.First(r => r.direction == edge.id).constraints);
            }
        }

        inWave = false;

        foreach (Edge edge in edges)
            edge.adjacentVertex.ContinueWave();
    }
    void ModifyPossibleElements(Element[] toIntersect)
    {
        if (!updated && currentElement == null)
        {
            foreach(Element e in toIntersect)
            {
                if (possibleElements.Exists(p => p == e) && !newPossibles.Exists(n => n == e))
                    newPossibles.Add(e);
            }
        }
    }
    void ContinueWave()
    {
        if (!updated && currentElement == null)
        {
            if (newPossibles.Count < possibleElements.Count)
            {
                possibleElements = new List<Element>(newPossibles);
                inWave = true;
            }
        }
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
        str.Append($"V[{this.id}] - U[{updated}]- W[{inWave}] - E[{txt}] - P");

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
