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
    public NewVertex(NewVertex n)
    {
        this.id = n.id;
        this.possibleElements = new List<Element>(n.possibleElements);
        edges = new List<Edge>(n.edges);
        this.xPosition = n.xPosition;
        this.zPosition = n.zPosition;
    }

    public void InstantiatePrefab()
    {
        MonoBehaviour.Instantiate(currentElement.prefab, new Vector3(xPosition, 0, zPosition), currentElement.prefab.transform.rotation);
    }
    public bool RemovePossibleElement(Element element)
    {
        currentElement = null;
        StringBuilder s = new StringBuilder();
        s.Append($"M = {id}");
        s.Append("PWas:");
        possibleElements.ForEach(e => s.Append(e.prefab.name));
        possibleElements.Remove(element);
        s.Append("NowIs:");
        if (possibleElements.Count != 0)
            possibleElements.ForEach(e => s.Append(e.prefab.name));
        else
            s.Append("Null");
        Debug.Log(s);
        return possibleElements.Count == 0;
    }
    public void SetUpMapRules(Element[] toIntersect)
    {
        possibleElements = possibleElements.Intersect(toIntersect).ToList();
    }
    public void WaveGeneration()
    {
        if (MapGeneration.instance.auto)
            currentElement = possibleElements[Random.Range(0, possibleElements.Count)];
        else
            currentElement = MapGeneration.instance.te;
        MapGeneration.instance.Push(); // save in stack

        // START WAVE
        possibleElements = possibleElements.Where(e => e == currentElement).ToList();
        Debug.Log($"V{id}, E:{currentElement.name}");
        inWave = true;
    }
    public void UpdateAdjacent()
    {
        updated = true;
        foreach (Element element in possibleElements)
        {
            foreach (Edge edge in edges)
            {
                if (MapGeneration.instance.stop)
                    return;
                else
                {
                    Debug.Log($"V{id} E{element.prefab.name} {edge.id}");
                    edge.adjacentVertex.ModifyPossibleElements(
                        element.rules.First(r => r.direction == edge.id).constraints);
                }
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
            StringBuilder str = new StringBuilder();
            str.Append($"\nV{id}Intersec:");
            foreach (Element e in toIntersect)
            {
                str.Append(e.prefab.name);
            }
            Debug.Log(str);
            if (newPossibles.Count == 0)
            {
                Debug.Log($"V{id} empty state");
                MapGeneration.instance.stop = true;
            }
        }
    }
    void ContinueWave()
    {
        if (!updated && currentElement == null)
        {
            if (newPossibles.Count < possibleElements.Count)
            {
                StringBuilder str = new StringBuilder();
                str.Append($"V{id}:");
                str.Append("\nOld:");
                foreach (Element e in possibleElements)
                {
                    str.Append(e.prefab.name);
                }
                str.Append("\nNew:");
                foreach (Element e in newPossibles)
                {
                    str.Append(e.prefab.name);
                }
                Debug.Log(str);
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
