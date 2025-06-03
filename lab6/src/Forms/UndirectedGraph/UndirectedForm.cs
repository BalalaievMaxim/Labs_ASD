namespace lab6;

public partial class UndirectedForm : Form
{
    private readonly List<Vertex> _vertices;
    private readonly List<Edge> _edges;
    private readonly IEnumerator<Action>? _steps;

    private int _path;

    public UndirectedForm(
        List<Vertex> vertices,
        List<Edge> edges,
        int[,] undirMatrix,
        int[,] weights
    )
    {
        _vertices = vertices;
        _edges = edges;


        MatrixUtils.OnRedrawNeeded += Invalidate;

        MatrixUtils.OnEdgeAdded += (v1, v2) =>
        {
            Edge edge;
            try
            {
                edge = _edges.First(e => e.From.Id == v1 && e.To.Id == v2);

            }
            catch
            {
                edge = _edges.First(e => e.From.Id == v2 && e.To.Id == v1);
            }
            _path += edge.Weight;
            edge.InMST = true;
        };

        InitializeComponent();

        _steps = MatrixUtils.Prim(vertices, undirMatrix, weights).GetEnumerator();
        MatrixUtils.OnPrimFinished += () =>
            {
                Console.WriteLine("\nАлгоритм Пріма завершив роботу.");
                Console.WriteLine("Сума ваг ребер знайденого мінімального кістяка: " + _path);
            };
    }

    private const int _textOffset = 10;

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        foreach (var edge in _edges)
            DrawEdge(e.Graphics, edge);

        foreach (var edge in _edges)
            DrawWeight(e.Graphics, edge);

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
        var color = edge.InMST ? Color.Red : Color.Black;

        switch (edge.Type)
        {
            case EdgeType.Normal:
                {
                    var (start, end) = edge.GetLineCoords();
                    DrawLine(g, start, end, color);

                    break;
                }

            case EdgeType.VisibilityObstructed:
                {
                    var (start, end) = edge.GetLineCoords();
                    DrawLine(g, start, edge.PolygonalLinkVertex, color);
                    DrawLine(g, edge.PolygonalLinkVertex, end, color);
                    break;
                }

            case EdgeType.SelfPointing:
                DrawLine(g, edge.From.Point, edge.SelfLinkVertices[0], color);
                DrawLine(g, edge.SelfLinkVertices[0], edge.SelfLinkVertices[1], color);
                DrawLine(g, edge.SelfLinkVertices[1], edge.SelfLinkVertices[2], color);

                break;
        }

    }

    private void DrawWeight(Graphics g, Edge edge)
    {
        string weight = edge.Weight.ToString();
        SizeF size = g.MeasureString(weight, Font);
        PointF pos = new(0, 0);

        switch (edge.Type)
        {
            case EdgeType.Normal:
                var (start, end) = edge.GetLineCoords();
                pos = new(
                    ((start.X + end.X) / 2) - (size.Width / 2),
                    ((start.Y + end.Y) / 2) - (size.Height / 2)
                );

                break;

            case EdgeType.SelfPointing:
                return;

            case EdgeType.VisibilityObstructed:
                pos = new(
                    edge.PolygonalLinkVertex.X - (size.Width / 2),
                    edge.PolygonalLinkVertex.Y - (size.Height / 2)
                );

                break;
        }


        g.DrawRectangle(
            edge.InMST ? Pens.Red : Pens.Black,
            pos.X - 1,
            pos.Y - 1,
            size.Width + 1,
            size.Height + 2
        );

        g.FillRectangle(
            Brushes.White,
            pos.X,
            pos.Y,
            size.Width,
            size.Height
        );

        g.DrawString(
            weight,
            Font,
            edge.InMST ? Brushes.Red : Brushes.Black,
            pos.X, pos.Y
        );
    }

    private static void DrawLine(Graphics g, PointF start, PointF end, Color color)
    {
        using Pen pen = new(color);
        g.DrawLine(pen, start, end);
    }
}