namespace lab6;

public class VertexFactory(int n)
{
    private readonly int _n = n;
    private readonly List<Vertex> _nodes = [];
    public List<Vertex> Vertices => _nodes;

    private const int _startX = 100, _startY = 100;
    public const int Gap = 200;

    private Direction _currentDirection = Direction.Right;
    private Direction GetNext() => _currentDirection switch
    {
        Direction.Right => Direction.Down,
        Direction.Down => Direction.Left,
        Direction.Left => Direction.Up,
        Direction.Up => Direction.Right,
        _ => Direction.Right,
    };

    public static Direction GetPrevious(Direction direction) => direction switch
    {
        Direction.Right => Direction.Up,
        Direction.Left => Direction.Down,
        Direction.Up => Direction.Left,
        Direction.Down => Direction.Right,
        _ => Direction.Right
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
            Direction outer;

            if (_nodes.Count == 0)
            {
                x = _startX;
                y = _startY;
                outer = Direction.Up;
            }


            else
            {
                x = _nodes[^1].Point.X;
                y = _nodes[^1].Point.Y;

                if (i == (int)_currentDirection)
                    _currentDirection = GetNext();

                switch (_currentDirection)
                {
                    case Direction.Right:
                        x += Gap;
                        break;
                    case Direction.Down:
                        y += Gap;
                        break;
                    case Direction.Left:
                        x -= Gap;
                        break;
                    case Direction.Up:
                        y -= Gap;
                        break;
                }

                outer = GetPrevious(_currentDirection);
            }

            _nodes.Add(new Vertex(i, x, y, outer));
        }
    }

    public void Reset()
    {
        foreach (var node in _nodes)
            node.State = VertexState.New;
    }
}

public class Vertex(int id, int x, int y, Direction outer)
{
    public int Id { get; } = id;
    public Point Point { get; } = new Point(x, y);
    public Direction Outer { get; } = outer;
    public const int Radius = 25;
    public VertexState State;

    public override string ToString() => $"{Id}: ({Point.X}, {Point.Y})";
}

public enum Direction
{
    Right = 4,
    Down = 6,
    Left = 9,
    Up = 1
}

public enum VertexState
{
    New,
    Visited,
    Active,
    Closed
}