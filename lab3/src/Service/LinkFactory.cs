namespace lab3;

public class LinkFactory(NodeFactory nodeFactory, bool undirected = false)
{
    private readonly NodeFactory _nodeFactory = nodeFactory;
    private readonly List<Link> _links = [];
    public List<Link> Links => _links;
    private readonly bool _undirected = undirected;

    public void CreateAll(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == 1)
                {
                    if (_undirected && matrix[j, i] == 1) matrix[j, i] = 0;
                    _links.Add(new Link(_nodeFactory.Nodes[i], _nodeFactory.Nodes[j]));
                }
            }
        }
    }

    public static int RandomizeOffset() => new Random().Next(40, 60);
}

public class Link
{
    public Node From { get; }
    public Node To { get; }
    public LinkType Type { get; }

    public Point PolygonalLinkVertice;
    public Point[] SelfLinkVertices = new Point[3];

    public Link(Node from, Node to)
    {
        From = from;
        To = to;

        if (from == to)
        {
            Type = LinkType.SelfPointing;

            switch (from.Outer)
            {
                case Direction.Up:
                    SelfLinkVertices = [
                        new Point(From.Point.X - Node.Radius, from.Point.Y),
                        new Point(from.Point.X - Node.Radius * 2, from.Point.Y - Node.Radius * 2),
                        new Point(from.Point.X, from.Point.Y - Node.Radius),
                    ];
                    break;

                case Direction.Right:
                    SelfLinkVertices = [
                        new Point(From.Point.X, from.Point.Y - Node.Radius),
                        new Point(from.Point.X + Node.Radius * 2, from.Point.Y - Node.Radius * 2),
                        new Point(from.Point.X + Node.Radius, from.Point.Y),
                    ];
                    break;

                case Direction.Down:
                    SelfLinkVertices = [
                        new Point(From.Point.X + Node.Radius, from.Point.Y),
                        new Point(from.Point.X + Node.Radius * 2, from.Point.Y + Node.Radius * 2),
                        new Point(from.Point.X, from.Point.Y + Node.Radius),
                    ];
                    break;

                case Direction.Left:
                    SelfLinkVertices = [
                        new Point(From.Point.X, from.Point.Y + Node.Radius),
                        new Point(from.Point.X - Node.Radius * 2, from.Point.Y + Node.Radius * 2),
                        new Point(from.Point.X - Node.Radius, from.Point.Y),
                    ];
                    break;
            }
        }
        else if (
            from.Point.X == to.Point.X &&
            MathF.Abs(from.Point.Y - to.Point.Y) != NodeFactory.Gap
        )
        {
            Type = LinkType.VisibilityObstructed;
            PolygonalLinkVertice = new(
                from.Point.X + (
                    from.Outer == Direction.Right ? -LinkFactory.RandomizeOffset() : LinkFactory.RandomizeOffset()
                ),
                (from.Point.Y + to.Point.Y) / 2
            );
        }

        else if (
            from.Point.Y == to.Point.Y &&
            MathF.Abs(from.Point.X - to.Point.X) != NodeFactory.Gap
        )
        {
            Type = LinkType.VisibilityObstructed;
            PolygonalLinkVertice = new(
                (from.Point.X + to.Point.X) / 2,
                from.Point.Y + (
                    from.Outer == Direction.Up ? -LinkFactory.RandomizeOffset() : +LinkFactory.RandomizeOffset()
                )
            );
        }

        else Type = LinkType.Normal;

    }
    public static bool IsRectangleSide(Link link) => link.From.Outer == link.To.Outer;
    public (PointF, PointF) GetLineCoords()
    {
        float dx = To.Point.X - From.Point.X;
        float dy = To.Point.Y - From.Point.Y;
        float dist = (float)Math.Sqrt(dx * dx + dy * dy);

        float ux = dx / dist;
        float uy = dy / dist;

        PointF start = new(From.Point.X + ux * Node.Radius, From.Point.Y + uy * Node.Radius);
        PointF end = new(To.Point.X - ux * Node.Radius, To.Point.Y - uy * Node.Radius);

        return (start, end);
    }

    public override string ToString() => $"{From.Id} -> {To.Id}";

}

public enum LinkType
{
    Normal,
    SelfPointing,
    VisibilityObstructed
}


public class DirectedLink(Node from, Node to) : Link(from, to)
{

}

