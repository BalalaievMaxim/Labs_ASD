namespace lab3;

public class LinkFactory(NodeFactory nodeFactory)
{
    private readonly NodeFactory _nodeFactory = nodeFactory;
    private readonly List<Link> _links = [];
    public List<Link> Links => _links;

    public const int DefaultLinkOffset = 50;

    public void CreateAll(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == 1)
                {
                    _links.Add(new Link(_nodeFactory.Nodes[i], _nodeFactory.Nodes[j]));
                }
            }
        }
    }
}

public class Link
{
    public Node From { get; }
    public Node To { get; }
    public LinkType Type { get; }

    public Point PolygonalLinkVertice;

    public Link(Node from, Node to)
    {
        From = from;
        To = to;

        if (from == to) Type = LinkType.SelfPointing;
        else if (
            from.Point.X == to.Point.X &&
            IsRectangleSide(this) &&
            MathF.Abs(from.Point.Y - to.Point.Y) != NodeFactory.Gap
        )
        {
            Type = LinkType.VisibilityObstructed;
            PolygonalLinkVertice = new(
                from.Point.X + (
                    from.Outer == Direction.Right ? -LinkFactory.DefaultLinkOffset : LinkFactory.DefaultLinkOffset
                ),
                (from.Point.Y + to.Point.Y) / 2
            );
        }
        else if (
            from.Point.Y == to.Point.Y &&
            IsRectangleSide(this) &&
            MathF.Abs(from.Point.X - to.Point.X) != NodeFactory.Gap
        )
        {
            Type = LinkType.VisibilityObstructed;
            PolygonalLinkVertice = new(
                (from.Point.X + to.Point.X) / 2,
                from.Point.Y + (
                    from.Outer == Direction.Up ? -LinkFactory.DefaultLinkOffset : +LinkFactory.DefaultLinkOffset
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

