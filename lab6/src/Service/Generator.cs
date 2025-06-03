namespace lab6;

public class Generator
{
    private readonly string _code;
    private uint _n;
    private readonly Random _random;
    public float K;

    public Generator(string code)
    {
        _code = code;
        Console.WriteLine($"Номер варіанту: {_code}");

        ParseFromCode();

        _random = new(int.Parse(_code));
    }

    public uint GetN(int position) => uint.Parse(_code[position - 1].ToString());

    private void ParseFromCode()
    {
        _n = GetN(3) + 10;
        Console.WriteLine($"Кількість вершин n = {_n}");

        Console.WriteLine($"Розміщення вершин - квадратом (прямокутником), бо n4 = {GetN(4)}");

        K = 1 - GetN(3) * 0.01f - GetN(4) * 0.005f - 0.05f;
        Console.WriteLine($"Коефіцієнт k = {K}");
    }

    public int[,] GenerateMatrix()
    {
        var matrix = new int[_n, _n];

        for (var i = 0; i < _n; i++)
        {
            for (var j = 0; j < _n; j++)
            {
                double value = _random.NextDouble() * 2;
                value *= K;
                value = value < 1 ? 0 : 1;

                matrix[i, j] = (int)value;
            }
        }

        return matrix;
    }

    public static void Await()
    {
        Console.Write($"\nНатисніть Enter, щоб продовжити...");
        Console.ReadLine();
    }

    private float[,] B()
    {
        var matrix = new float[_n, _n];

        for (var i = 0; i < _n; i++)
        {
            for (var j = 0; j < _n; j++)
            {
                matrix[i, j] = (float)_random.NextDouble() * 2;
            }
        }

        return matrix;
    }

    private int[,] C(float[,] B, int[,] undirMatrix)
    {
        var matrix = new int[_n, _n];

        for (int i = 0; i < _n; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                matrix[i, j] = (int)MathF.Ceiling(
                    B[i, j] * 100 * undirMatrix[i, j]
                );
            }
        }

        return matrix;
    }

    private int[,] D(int[,] C)
    {
        var matrix = new int[_n, _n];

        for (int i = 0; i < _n; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                matrix[i, j] = C[i, j] > 0 ? 1 : 0;
            }
        }

        return matrix;
    }

    private int[,] H(int[,] D)
    {
        var matrix = new int[_n, _n];

        for (int i = 0; i < _n; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                matrix[i, j] = D[i, j] == D[j, i] ? 1 : 0;
            }
        }

        return matrix;
    }

    private int[,] UpperTriangle()
    {
        var matrix = new int[_n, _n];

        for (int i = 0; i < _n; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                matrix[i, j] = i < j ? 1 : 0;
            }
        }

        return matrix;
    }

    public int[,] Weights(int[,] undirMatrix)
    {
        int[,] c = C(B(), undirMatrix);
        int[,] d = D(c);
        int[,] h = H(d);
        int[,] tri = UpperTriangle();

        var matrix = new int[_n, _n];

        for (int i = 0; i < _n; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                int elem = 0;

                if (i != j)
                {
                    elem = (d[i, j] + h[i, j] * tri[i, j]) * c[i, j];

                    if (elem == 0) elem = -1;
                }

                matrix[i, j] = elem;
            }
        }

        return MatrixUtils.ToSymmetrical(matrix);
    }
}