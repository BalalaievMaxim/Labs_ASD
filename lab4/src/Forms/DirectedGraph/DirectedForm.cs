namespace lab4;

public partial class DirectedForm : Form
{
    private readonly List<Vertex> _vertices;
    private readonly List<Edge> _edges;

    private Graphics? _graphics;

    public DirectedForm(List<Vertex> vertices, List<Edge> edges)
    {
        _vertices = vertices;
        _edges = edges;

        InitializeComponent();
    }

    private const int _textOffset = 10;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        _graphics = e.Graphics;

        foreach (var edge in _edges)
        {
            Draw(edge);
        }
        foreach (var vertex in _vertices)
        {
            Draw(vertex);
        }
    }

    private void Draw(Vertex vertex)
    {
        if (_graphics == null) return;

        _graphics.FillEllipse(
            new SolidBrush(Color.LightGray),
            vertex.Point.X - Vertex.Radius,
            vertex.Point.Y - Vertex.Radius,
            Vertex.Radius * 2,
            Vertex.Radius * 2
        );

        _graphics.DrawString(
            vertex.Id.ToString(),
            Font,
            Brushes.Black,
            vertex.Point.X - _textOffset,
            vertex.Point.Y - _textOffset
        );
    }

    private void Draw(Edge edge)
    {
        if (_graphics == null) return;

        switch (edge.Type)
        {
            case EdgeType.Normal:
                {
                    var (start, end) = edge.GetLineCoords();

                    if (!edge.HasInvertion)
                    {
                        DrawArrow(start, end);
                    }
                    else
                    {
                        DrawLine(start, edge.PolygonalLinkVertex);
                        DrawArrow(edge.PolygonalLinkVertex, end);
                    }

                    break;
                }

            case EdgeType.VisibilityObstructed:
                {
                    var (start, end) = edge.GetLineCoords();
                    DrawLine(start, edge.PolygonalLinkVertex);
                    DrawArrow(edge.PolygonalLinkVertex, end);
                    break;
                }

            case EdgeType.SelfPointing:
                DrawLine(edge.From.Point, edge.SelfLinkVertices[0]);
                DrawLine(edge.SelfLinkVertices[0], edge.SelfLinkVertices[1]);
                DrawArrow(edge.SelfLinkVertices[1], edge.SelfLinkVertices[2]);

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

