namespace lab3;

public partial class DirectedForm : Form
{
    private readonly NodeFactory _nodeFactory;
    private readonly LinkFactory _linkFactory;

    private Graphics? _graphics;

    public DirectedForm(NodeFactory nodeFactory, LinkFactory linkFactory)
    {
        _nodeFactory = nodeFactory;
        _linkFactory = linkFactory;

        InitializeComponent();
    }

    private const int _textOffset = 10;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        _graphics = e.Graphics;

        foreach (var node in _nodeFactory.Nodes)
        {
            Draw(node);
        }
        foreach (var link in _linkFactory.Links)
        {
            Draw(link);
        }

    }

    private void Draw(Node node)
    {
        if (_graphics == null) return;

        _graphics.FillEllipse(
            new SolidBrush(Color.LightGray),
            node.Point.X - Node.Radius,
            node.Point.Y - Node.Radius,
            Node.Radius * 2,
            Node.Radius * 2
        );

        _graphics.DrawString(
            node.Id.ToString(),
            Font,
            Brushes.Black,
            node.Point.X - _textOffset,
            node.Point.Y - _textOffset
        );
    }

    private void Draw(Link link)
    {
        if (_graphics == null) return;

        switch (link.Type)
        {
            case LinkType.Normal:
                _graphics.DrawLine(Pens.Black, link.From.Point, link.To.Point);
                break;

            case LinkType.VisibilityObstructed:
                _graphics.DrawLine(Pens.Black, link.From.Point, link.PolygonalLinkVertice);
                _graphics.DrawLine(Pens.Black, link.PolygonalLinkVertice, link.To.Point);
                break;

        }
    }

}
