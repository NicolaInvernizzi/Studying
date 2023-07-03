using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public int weight;
    public Vertex adjacentVertex;
    public Edge(Vertex adjacentVertex , int weight)
    {
        this.weight = weight;
        this.adjacentVertex = adjacentVertex;
    }
}
