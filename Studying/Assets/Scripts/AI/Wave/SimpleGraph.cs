using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SimpleGraph : MonoBehaviour
{
    /*
     * Testing
     *      SimpleGraph graph = new SimpleGraph(8);
            graph.AddEdge(0, 2);
            graph.AddEdge(1, 3);
            graph.AddEdge(1, 4);
            graph.AddEdge(2, 5);
            graph.AddEdge(3, 5);
            graph.AddEdge(4, 6);
            graph.AddEdge(6, 7);

            graph.PrintAdjanceyList();
            graph.CreateAdjanceyMatrix();
    */
    int totalVertices = 0;
    LinkedList<int>[] adjanceyArray;
    int?[,] adjanceyMatrix;
    public SimpleGraph(int n)
    {
        totalVertices = n;
        adjanceyArray = new LinkedList<int>[n];
        adjanceyMatrix = new int?[totalVertices, totalVertices];
        for (int i = 0; i < totalVertices; i++)
            adjanceyArray[i] = new LinkedList<int>();
    }

    public void AddEdge(int vertex, int vertexToAdd)
    {
        adjanceyArray[vertex].AddLast(vertexToAdd);
    }
    public void PrintAdjanceyList()
    {
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < totalVertices; i++)
        {
            str.Append($"Node Value: {i} with Neighbors -> ");
            foreach (int val in adjanceyArray[i])
                str.Append($"{val.ToString()}, ");
            str.Append("\n");
        }
        Console.WriteLine(str);
    }
    public void CreateAdjanceyMatrix() // squared matrix
    {
        for (int parentVertex = 0; parentVertex < totalVertices; parentVertex++)
        {
            LinkedList<int> parentNode = adjanceyArray[parentVertex];

            for (int childVertex = 0; childVertex < totalVertices; childVertex++)
            {
                if (parentVertex != childVertex && parentNode.Find(childVertex) != null)
                    adjanceyMatrix[parentVertex, childVertex] = 1;
            }
        }
        PrintAdjanceyMatrix();
    }
    void PrintAdjanceyMatrix()
    {
        StringBuilder str = new StringBuilder();

        str.Append("   ");
        for (int i = 0; i < totalVertices; i++)
            str.Append($" {i} ");

        str.Append("\n");

        for (int raw = 0; raw < totalVertices; raw++)
        {
            str.Append($" {raw} ");
            for (int column = 0; column < totalVertices; column++)
            {
                string text = "";
                if (raw == column)
                    text = " x ";
                else if (adjanceyMatrix[raw, column] == null)
                    text = " . ";
                else
                    text = $" {adjanceyMatrix[raw, column]} ";
                str.Append(text);
            }
            str.Append("\n");
        }
        Console.Write(str);
    }
}
