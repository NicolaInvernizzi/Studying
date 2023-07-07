using System.Linq;

public class Edge
{
    public Direction id;
    public int weight;
    public NewVertex adjacentVertex;
    public Edge(Direction edgeId, int adjacentVertex, int weight)
    {
        id = edgeId;
        this.adjacentVertex = MapGeneration.instance.vertices.First(v => v.id == adjacentVertex);
        this.weight = weight;
    }
}
