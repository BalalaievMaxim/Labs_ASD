namespace lab3;

public partial class MainForm : Form
{
    private readonly NodeFactory _nodeFactory;
    private readonly LinkFactory _linkFactory;

    private Graphics? _graphics;

    public MainForm(NodeFactory nodeFactory, LinkFactory linkFactory)
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
            node.X - Node.Radius,
            node.Y - Node.Radius,
            Node.Radius * 2,
            Node.Radius * 2
        );

        _graphics.DrawString(
            node.Id.ToString(),
            Font,
            Brushes.Black,
            node.X - _textOffset,
            node.Y - _textOffset
        );
    }

    private void Draw(Link link)
    {
        if (_graphics == null) return;

        switch (link.Type)
        {
            case LinkType.Normal:
                _graphics.DrawLine(Pens.Black, link.From.X, link.From.Y, link.To.X, link.To.Y);
                break;

        }
    }

}
