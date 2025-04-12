namespace lab3;

public class LinkFactory(NodeFactory nodeFactory)
{
    private readonly NodeFactory _nodeFactory = nodeFactory;
    private readonly List<Link> _links = [];
    public List<Link> Links => _links;

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

    public Link(Node from, Node to)
    {
        From = from;
        To = to;

        if (from == to) Type = LinkType.SelfPointing;
        else if (from.X == to.X || from.Y == to.Y) Type = LinkType.VisibilityObstructed;
        else Type = LinkType.Normal;

    }

    public override string ToString() => $"{From.Id} -> {To.Id}";

}

public class DirectedLink(Node from, Node to) : Link(from, to)
{
    
}

public enum LinkType
{
    Normal,
    SelfPointing,
    VisibilityObstructed
}
