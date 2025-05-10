namespace lab4;

public partial class UndirectedForm : Form
{
    private readonly List<Vertex> _vertices;
    private readonly List<Edge> _edges;

    private Graphics? _graphics;

    public UndirectedForm(List<Vertex> vertices, List<Edge> edges)
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

    private void Draw(Vertex vertice)
    {
        if (_graphics == null) return;

        _graphics.FillEllipse(
            new SolidBrush(Color.LightGray),
            vertice.Point.X - Vertex.Radius,
            vertice.Point.Y - Vertex.Radius,
            Vertex.Radius * 2,
            Vertex.Radius * 2
        );

        _graphics.DrawString(
            vertice.Id.ToString(),
            Font,
            Brushes.Black,
            vertice.Point.X - _textOffset,
            vertice.Point.Y - _textOffset
        );
    }

    private void Draw(Edge edge)
    {
        if (_graphics == null) return;

        switch (edge.Type)
        {
            case EdgeType.Normal:
                {
                    DrawLine(edge.From.Point, edge.To.Point);
                    break;
                }

            case EdgeType.VisibilityObstructed:
                {
                    DrawLine(edge.From.Point, edge.PolygonalLinkVertex);
                    DrawLine(edge.PolygonalLinkVertex, edge.To.Point);
                    break;
                }

            case EdgeType.SelfPointing:
                DrawLine(edge.From.Point, edge.SelfLinkVertices[0]);
                DrawLine(edge.SelfLinkVertices[0], edge.SelfLinkVertices[1]);
                DrawLine(edge.SelfLinkVertices[1], edge.From.Point);

                break;
        }

    }

    private void DrawLine(Point start, Point end)
    {
        if (_graphics == null) return;
        _graphics.DrawLine(Pens.Black, start, end);
    }
}

