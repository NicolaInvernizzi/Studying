using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        Debug.Log($"x:{xPosition} y:{zPosition}");
        MonoBehaviour.Instantiate(currentElement.prefab, new Vector3(xPosition, 0, zPosition), currentElement.prefab.transform.rotation);
    }
    //public void RemovePossibleElement(Element element)
    //{
    //    possibleElements = possibleElements.Where(e => e != element).ToArray();
    //}
    //public void SetUpMapRules(Element[] toIntersect)
    //{
    //    possibleElements = possibleElements.Intersect(toIntersect).ToArray();
    //    prevPossibleElements = possibleElements.Length;
    //}
    public void WaveGeneration()
    {
        Debug.Log(possibleElements.Count);
        currentElement = possibleElements[Random.Range(0, possibleElements.Count)];
        possibleElements = possibleElements.Where(e => e == currentElement).ToList();
        Debug.Log($"V{id}, E:{currentElement.name}");
        /*
        MapGeneration.instance.currentElement = currentElement;
        */
        inWave = true;
    }
    public void UpdateAdjacent()
    {
        updated = true;
        foreach (Element element in possibleElements)
        {
            foreach (Edge edge in edges)
            {
                Debug.Log($"V{id} E{element.prefab.name}{edge.id}");
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
        /*
        if (MapGeneration.instance.stopUpdating || currentElement != null || updated)
            return;

        if (possibleElements.Length == 0)
        {
            MapGeneration.instance.stopUpdating = true;
            return;
        }
        */

        if (!updated || currentElement == null)
        {
            foreach(Element e in toIntersect)
            {
                if (possibleElements.Exists(p => p == e))
                    newPossibles.Add(e);
            }
        }
    }
    void ContinueWave()
    {
        if (!updated)
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
        str.Append($"id [{this.id}] - inW[{inWave}] - Element [{txt}] - Possibles ");

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
