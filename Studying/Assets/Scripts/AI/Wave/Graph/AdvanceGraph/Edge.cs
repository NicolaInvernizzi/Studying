using System.Linq;

public class Edge
{
    public Direction id;
    public int weight { get; private set; }
    public Vertex adjacentVertex;
    public Edge(Direction edgeId, int adjacentVertex, int weight)
    {
        id = edgeId;
        this.adjacentVertex = AdvanceGraph.vertices.First(v => v.id == adjacentVertex);
        this.weight = weight;
    }
}
