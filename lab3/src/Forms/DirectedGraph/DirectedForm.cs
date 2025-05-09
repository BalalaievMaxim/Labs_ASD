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

        foreach (var link in _linkFactory.Links)
        {
            Draw(link);
        }
        foreach (var node in _nodeFactory.Nodes)
        {
            Draw(node);
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
                {
                    var (start, end) = link.GetLineCoords();

                    if (!link.HasInvertion)
                    {
                        DrawArrow(start, end);
                    }
                    else
                    {
                        DrawLine(start, link.PolygonalLinkVertice);
                        DrawArrow(link.PolygonalLinkVertice, end);
                    }

                    break;
                }

                case LinkType.VisibilityObstructed:
                    {
                        var (start, end) = link.GetLineCoords();
                        DrawLine(start, link.PolygonalLinkVertice);
                        DrawArrow(link.PolygonalLinkVertice, end);
                        break;
                    }

                case LinkType.SelfPointing:
                    DrawLine(link.From.Point, link.SelfLinkVertices[0]);
                    DrawLine(link.SelfLinkVertices[0], link.SelfLinkVertices[1]);
                    DrawArrow(link.SelfLinkVertices[1], link.SelfLinkVertices[2]);

                    break;
        }

    }

    private void DrawArrow(PointF start, PointF end)
    {
        if (_graphics == null) return;

        Pen pen = Pens.Black;
        _graphics.DrawLine(pen, start, end);

        // Arrowhead
        const float arrowSize = 10f;
        double angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
        PointF arrow1 = new PointF(
            end.X - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
            end.Y - arrowSize * (float)Math.Sin(angle - Math.PI / 6));
        PointF arrow2 = new PointF(
            end.X - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
            end.Y - arrowSize * (float)Math.Sin(angle + Math.PI / 6));

        _graphics.DrawLine(pen, end, arrow1);
        _graphics.DrawLine(pen, end, arrow2);
    }

    private void DrawLine(PointF start, Point end)
    {
        if (_graphics == null) return;
        _graphics.DrawLine(Pens.Black, start, end);
    }
}

