namespace lab3;

public class NodeFactory(int n)
{
    private readonly int _n = n;
    private readonly List<Node> _nodes = [];
    public List<Node> Nodes => _nodes;

    private const int _startX = 100, _startY = 100;
    private const int _gap = 100;
    private enum Direction
    {
        Right = 4,
        Down = 6,
        Left = 9,
        Up = 1
    }
    private Direction _currentDirection = Direction.Right;
    private static readonly Direction[] _directions = [
        Direction.Right,
        Direction.Down,
        Direction.Left,
        Direction.Up
    ];
    private Direction GetNextDirection()
    {
        int index = Array.IndexOf(_directions, _currentDirection);
        if (index < 0 || index + 1 >= _directions.Length)
            return _directions[0];

        return _directions[index + 1];
    }

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
                    _currentDirection = GetNextDirection();

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
    public int Id { get; private set; } = id;
    public int X { get; private set; } = x;
    public int Y { get; private set; } = y;
    public const int Radius = 25;

    public override string ToString() => $"{Id}: ({X}, {Y})";
}