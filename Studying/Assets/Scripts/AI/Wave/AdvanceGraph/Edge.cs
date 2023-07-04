using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public int weight;
    public Vertex adjacentVertex;
    public Edge(int id, int weight)
    {
        this.weight = weight;
        adjacentVertex = new Vertex(id);
    }
}
