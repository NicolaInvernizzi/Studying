public class Edge
{
    public int weight { get; private set; }
    public readonly Vertex adjacentVertex;
    public Edge(int id, int weight)
    {
        this.weight = weight;
        adjacentVertex = new Vertex(id);
    }
}
