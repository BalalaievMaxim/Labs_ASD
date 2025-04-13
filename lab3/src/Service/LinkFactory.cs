using System.Windows.Forms.VisualStyles;

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
                    bool hasInvertion = false;
                    if (matrix[j, i] == 1)
                    {
                        if (!_undirected)
                        {
                            matrix[j, i] = 2;
                            hasInvertion = true;
                        }

                    }
                    _links.Add(new Link(_nodeFactory.Nodes[i], _nodeFactory.Nodes[j], hasInvertion));
                }
                else if (matrix[i, j] == 2)
                {
                    _links.Add(new Link(_nodeFactory.Nodes[i], _nodeFactory.Nodes[j], false));
                }
            }
        }
    }

    public static int RandomizeOffset() => new Random().Next(40, 80);
}

public class Link
{
    public Node From { get; }
    public Node To { get; }
    public LinkType Type { get; }
    public bool HasInvertion { get; }
    private readonly int _offset = 40;

    public Point PolygonalLinkVertice;
    public Point[] SelfLinkVertices = new Point[3];

    public Link(Node from, Node to, bool hasInvertion)
    {
        From = from;
        To = to;
        HasInvertion = hasInvertion;

        if (hasInvertion)
            _offset = -_offset;

        if (from == to)
        {
            Type = LinkType.SelfPointing;
            SelfLinkVertices = [
                new Point(from.Point.X, from.Point.Y - Node.Radius * 2),
                new Point(from.Point.X - Node.Radius * 2, from.Point.Y),
                new Point(from.Point.X - Node.Radius, from.Point.Y),
            ];
        }
        else if (
            from.Point.X == to.Point.X &&
            MathF.Abs(from.Point.Y - to.Point.Y) != NodeFactory.Gap
        )
        {
            Type = LinkType.VisibilityObstructed;
            PolygonalLinkVertice = new(
                from.Point.X + (
                    from.Outer == Direction.Right ? +_offset : -_offset
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
                    from.Outer == Direction.Down ? +_offset : -_offset
                )
            );
        }

        else
        {
            Type = LinkType.Normal;
            if (hasInvertion) PolygonalLinkVertice = new Point(
                (from.Point.X + to.Point.X) / 2 + _offset,
                (from.Point.Y + to.Point.Y) / 2 + _offset
            );
        }

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

    public override string ToString() => $"{From.Id} -> {To.Id}, HasInvertion: {HasInvertion}";

}

public enum LinkType
{
    Normal,
    SelfPointing,
    VisibilityObstructed
}
