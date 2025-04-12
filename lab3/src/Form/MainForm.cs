namespace lab3;

public partial class MainForm : Form
{
    private readonly NodeFactory _nodeFactory;

    public MainForm(NodeFactory nodeFactory)
    {
        _nodeFactory = nodeFactory;

        InitializeComponent();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Brush brush = new SolidBrush(Color.Gray);
        foreach (var node in _nodeFactory.Nodes)
        {
            Draw(node, e.Graphics);
        }
    }

    private void Draw(Node node, Graphics graphics)
    {
        graphics.FillEllipse(
            new SolidBrush(Color.Gray),
            node.X - Node.Radius,
            node.Y - Node.Radius,
            Node.Radius * 2,
            Node.Radius * 2
        );

        graphics.DrawString(node.Id.ToString(), Font, Brushes.Black, node.X, node.Y);
    }
}
