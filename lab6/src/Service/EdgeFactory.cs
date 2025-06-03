namespace lab6;

public class EdgeFactory(VertexFactory vertexFactory, int[,] weights)
{
    private readonly VertexFactory _nodeFactory = vertexFactory;
    private readonly List<Edge> _edges = [];
    public List<Edge> Edges => _edges;
    private int[,] _weights = weights;

    public void CreateAll(int[,] matrix)
    {
        _edges.Clear();
        var copy = (int[,])matrix.Clone();

        for (int i = 0; i < copy.GetLength(0); i++)
        {
            for (int j = 0; j < copy.GetLength(1); j++)
            {
                if (copy[i, j] == 1)
                {
                    if (copy[j, i] == 1)
                    {
                        copy[j, i] = 2;
                    }
                    _edges.Add(new Edge(
                        _nodeFactory.Vertices[i], _nodeFactory.Vertices[j], _weights[i, j]
                    ));
                }
                // else if (matrix[i, j] == 2)
                // {
                //     _edges.Add(new Edge(
                //         _nodeFactory.Vertices[i], _nodeFactory.Vertices[j],
                //         false, _weights[i, j]
                //     ));
                // }
            }
        }
    }

    public void Reset()
    {
        foreach (var edge in _edges)
            edge.InMST = false;
    }
}

public class Edge
{
    public Vertex From { get; }
    public Vertex To { get; }
    public EdgeType Type { get; }
    private readonly int _offset = 40;
    public bool InMST;
    public int Weight { get; }


    public Point PolygonalLinkVertex;
    public Point[] SelfLinkVertices = new Point[3];

    public Edge(Vertex from, Vertex to, int weight)
    {
        From = from;
        To = to;
        Weight = weight;


        if (from == to)
        {
            Type = EdgeType.SelfPointing;
            SelfLinkVertices = [
                new Point(from.Point.X, from.Point.Y - Vertex.Radius * 2),
                new Point(from.Point.X - Vertex.Radius * 2, from.Point.Y),
                new Point(from.Point.X - Vertex.Radius, from.Point.Y),
            ];
        }
        else if (
            from.Point.X == to.Point.X &&
            MathF.Abs(from.Point.Y - to.Point.Y) != VertexFactory.Gap
        )
        {
            Type = EdgeType.VisibilityObstructed;
            PolygonalLinkVertex = new(
                from.Point.X + (
                    from.Outer == Direction.Right ? +_offset : -_offset
                ),
                (from.Point.Y + to.Point.Y) / 2
            );
        }

        else if (
            from.Point.Y == to.Point.Y &&
            MathF.Abs(from.Point.X - to.Point.X) != VertexFactory.Gap
        )
        {
            Type = EdgeType.VisibilityObstructed;
            PolygonalLinkVertex = new(
                (from.Point.X + to.Point.X) / 2,
                from.Point.Y + (
                    from.Outer == Direction.Down ? +_offset : -_offset
                )
            );
        }

        else
        {
            // Type = EdgeType.Normal;
            // if (hasInvertion) PolygonalLinkVertex = new Point(
            //     (from.Point.X + to.Point.X) / 2 + _offset,
            //     (from.Point.Y + to.Point.Y) / 2 + _offset
            // );
        }

    }
    public static bool IsRectangleSide(Edge link) => link.From.Outer == link.To.Outer;
    public (PointF, PointF) GetLineCoords()
    {
        float dx = To.Point.X - From.Point.X;
        float dy = To.Point.Y - From.Point.Y;
        float dist = (float)Math.Sqrt(dx * dx + dy * dy);

        float ux = dx / dist;
        float uy = dy / dist;

        PointF start = new(From.Point.X + ux * Vertex.Radius, From.Point.Y + uy * Vertex.Radius);
        PointF end = new(To.Point.X - ux * Vertex.Radius, To.Point.Y - uy * Vertex.Radius);

        return (start, end);
    }


    public override string ToString() => $"{From.Id} -> {To.Id}";

}

public enum EdgeType
{
    Normal,
    SelfPointing,
    VisibilityObstructed
}
