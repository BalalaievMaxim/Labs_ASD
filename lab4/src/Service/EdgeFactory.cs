namespace lab4;

public class EdgeFactory(VertexFactory nodeFactory, bool undirected = false)
{
    private readonly VertexFactory _nodeFactory = nodeFactory;
    private readonly List<Edge> _links = [];
    public List<Edge> Edges => _links;
    private readonly bool _undirected = undirected;

    public void CreateAll(int[,] matrix)
    {
        _links.Clear();

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == 1)
                {
                    bool hasInvertion = false;
                    if (matrix[j, i] == 1)
                    {
                        if (!_undirected)
                        {
                            matrix[j, i] = 2;
                            hasInvertion = true;
                        }

                    }
                    _links.Add(new Edge(_nodeFactory.Vertices[i], _nodeFactory.Vertices[j], hasInvertion));
                }
                else if (matrix[i, j] == 2)
                {
                    _links.Add(new Edge(_nodeFactory.Vertices[i], _nodeFactory.Vertices[j], false));
                }
            }
        }
    }
}

public class Edge
{
    public Vertex From { get; }
    public Vertex To { get; }
    public EdgeType Type { get; }
    public bool HasInvertion { get; }
    private readonly int _offset = 40;

    public Point PolygonalLinkVertex;
    public Point[] SelfLinkVertices = new Point[3];

    public Edge(Vertex from, Vertex to, bool hasInvertion)
    {
        From = from;
        To = to;
        HasInvertion = hasInvertion;

        if (hasInvertion)
            _offset = -_offset;

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
            Type = EdgeType.Normal;
            if (hasInvertion) PolygonalLinkVertex = new Point(
                (from.Point.X + to.Point.X) / 2 + _offset,
                (from.Point.Y + to.Point.Y) / 2 + _offset
            );
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

    public override string ToString() => $"{From.Id} -> {To.Id}, HasInvertion: {HasInvertion}";

}

public enum EdgeType
{
    Normal,
    SelfPointing,
    VisibilityObstructed
}
