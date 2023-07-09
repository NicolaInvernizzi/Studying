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
    public bool updated;
    public NewVertex(int id, Element[] possibleElements)
    {
        this.id = id;
        this.possibleElements = possibleElements;
        edges = new List<Edge>();
    }

    public void RemovePossibleElement(Element element)
    {
        possibleElements = possibleElements.Where(e => e != element).ToArray();
    }
    public void ModifyPossibleElements(Element[] toIntersect)
    {
        if (currentElement != null && !updated)
            return;

        possibleElements = possibleElements.Intersect(toIntersect).ToArray();
        UpdateAdjacent();
        updated = true;
    }
    public void SetRandomElement()
    {
        currentElement = possibleElements[Random.Range(0, possibleElements.Length)];
        Debug.Log($"V{id}, E:{currentElement.name}");
        RemovePossibleElement(currentElement);
        UpdateAdjacent();
    }
    void UpdateAdjacent()
    {
        foreach (Edge edge in edges)
            edge.adjacentVertex.ModifyPossibleElements(currentElement.rules.First(r => r.direction == edge.id).constraints);
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
        str.Append($" id [{this.id}] - Element [{txt}] - Possibles ");

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
