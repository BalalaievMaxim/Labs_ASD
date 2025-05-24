namespace lab5;

public partial class DirectedForm : Form
{
    private readonly List<Vertex> _vertices;
    private readonly List<Edge> _edges;
    private readonly IEnumerator<Action>? _steps;



    public DirectedForm(
        List<Vertex> vertices,
        List<Edge> edges,
        int[,] matrix,
        GraphTraversalStrategy strategy
    )
    {
        _vertices = vertices;
        _edges = edges;

        GraphSearchUtils.OnRedrawNeeded += Invalidate;

        GraphSearchUtils.OnEdgeVisited += (v1, v2) =>
        {
            var edge = _edges.First(e => e.From.Id == v1 && e.To.Id == v2);
            edge.Visited = true;
            Invalidate();
        };

        InitializeComponent();

        if (strategy == GraphTraversalStrategy.BFS)
        {
            GraphSearchUtils.OnBFSFinished += Generator.Print;
            Text = "Пошук вшир (BFS)";
            _steps = GraphSearchUtils.BFS(_vertices, matrix, 0).GetEnumerator();
        }
        else
        {
            GraphSearchUtils.OnDFSFinished += Generator.Print;
            Text = "Пошук вглиб (DFS)";
            _steps = GraphSearchUtils.DFS(_vertices, matrix, 0).GetEnumerator();
        }

    }

    private const int _textOffset = 10;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        foreach (var edge in _edges)
            DrawEdge(e.Graphics, edge);

        for (int i = 0; i < _vertices.Count; i++)
        {
            var vertex = _vertices[i];
            DrawVertex(e.Graphics, vertex);
        }

    }

    private void DrawVertex(Graphics g, Vertex vertex)
    {
        var color = vertex.State switch
        {
            VertexState.Visited => Color.Gold,
            VertexState.Active => Color.LightCoral,
            VertexState.Closed => Color.LightGreen,
            _ => Color.LightGray,
        };

        g.FillEllipse(new SolidBrush(color),
            vertex.Point.X - Vertex.Radius,
            vertex.Point.Y - Vertex.Radius,
            Vertex.Radius * 2,
            Vertex.Radius * 2);

        g.DrawString(
            vertex.Id.ToString(),
            Font,
            Brushes.Black,
            vertex.Point.X - _textOffset,
            vertex.Point.Y - _textOffset);
    }

    private static void DrawEdge(Graphics g, Edge edge)
    {
        var color = edge.Visited ? Color.Red : Color.Black;

        switch (edge.Type)
        {
            case EdgeType.Normal:
                {
                    var (start, end) = edge.GetLineCoords();

                    if (!edge.HasInvertion)
                    {
                        DrawArrow(g, start, end, color);
                    }
                    else
                    {
                        DrawLine(g, start, edge.PolygonalLinkVertex, color);
                        DrawArrow(g, edge.PolygonalLinkVertex, end, color);
                    }

                    break;
                }

            case EdgeType.VisibilityObstructed:
                {
                    var (start, end) = edge.GetLineCoords();
                    DrawLine(g, start, edge.PolygonalLinkVertex, color);
                    DrawArrow(g, edge.PolygonalLinkVertex, end, color);
                    break;
                }

            case EdgeType.SelfPointing:
                DrawLine(g, edge.From.Point, edge.SelfLinkVertices[0], color);
                DrawLine(g, edge.SelfLinkVertices[0], edge.SelfLinkVertices[1], color);
                DrawArrow(g, edge.SelfLinkVertices[1], edge.SelfLinkVertices[2], color);

                break;
        }

    }

    private static void DrawArrow(Graphics g, PointF start, PointF end, Color color)
    {
        Pen pen = new(color);
        g.DrawLine(pen, start, end);

        // Arrowhead
        const float arrowSize = 10f;
        double angle = Math.Atan2(end.Y - start.Y, end.X - start.X);
        PointF arrow1 = new PointF(
            end.X - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
            end.Y - arrowSize * (float)Math.Sin(angle - Math.PI / 6));
        PointF arrow2 = new PointF(
            end.X - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
            end.Y - arrowSize * (float)Math.Sin(angle + Math.PI / 6));

        g.DrawLine(pen, end, arrow1);
        g.DrawLine(pen, end, arrow2);
    }

    private static void DrawLine(Graphics g, PointF start, PointF end, Color color)
    {
        using Pen pen = new(color);
        g.DrawLine(pen, start, end);
    }
}

public enum GraphTraversalStrategy
{
    BFS,
    DFS
}