namespace lab3;

public class NodeFactory(int n)
{
    private readonly int _n = n;
    private readonly List<Node> _nodes = [];
    public List<Node> Nodes => _nodes;

    private const int _startX = 100, _startY = 100;
    private const int _gap = 100;

    private Direction _currentDirection = Direction.Right;
    private Direction GetNext() => _currentDirection switch
    {
        Direction.Right => Direction.Down,
        Direction.Down => Direction.Left,
        Direction.Left => Direction.Up,
        Direction.Up => Direction.Right,
        _ => Direction.Right,
    };
    
    public static Direction GetOpposite(Direction direction) => direction switch
    {
        Direction.Right => Direction.Left,
        Direction.Left => Direction.Right,
        Direction.Up => Direction.Down,
        Direction.Down => Direction.Up,
        _ => Direction.Right
    };

    public void CreateAll()
    {
        for (int i = 0; i < _n; i++)
        {
            int x, y;

            if (_nodes.Count == 0)
            {
                x = _startX;
                y = _startY;
            }


            else
            {
                x = _nodes[^1].X;
                y = _nodes[^1].Y;

                if (i == (int)_currentDirection)
                    _currentDirection = GetNext();

                switch (_currentDirection)
                {
                    case Direction.Right:
                        x += _gap;
                        break;
                    case Direction.Down:
                        y += _gap;
                        break;
                    case Direction.Left:
                        x -= _gap;
                        break;
                    case Direction.Up:
                        y -= _gap;
                        break;
                }
            }

            _nodes.Add(new Node(i, x, y));
        }
    }
}

public class Node(int id, int x, int y)
{
    public int Id { get; } = id;
    public int X { get; } = x;
    public int Y { get; } = y;
    public const int Radius = 25;

    public override string ToString() => $"{Id}: ({X}, {Y})";
}

public enum Direction
{
    Right = 4,
    Down = 6,
    Left = 9,
    Up = 1
}