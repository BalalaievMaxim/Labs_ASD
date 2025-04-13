namespace lab3;

public class Generator
{
    private readonly string _code;
    private uint _n;
    private readonly Random _random;
    private float _k;

    public Generator(string code)
    {
        _code = code;
        Console.WriteLine($"Номер варіанту: {_code}");

        ParseFromCode();

        _random = new(int.Parse(_code));
    }

    private uint GetN(int position) => uint.Parse(_code[position - 1].ToString());

    private void ParseFromCode()
    {
        _n = GetN(3) + 10;
        Console.WriteLine($"Кількість вершин n = {_n}");

        Console.WriteLine($"Розміщення вершин - квадратом (прямокутником), бо n4 = {GetN(4)}");

        _k = 1 - GetN(3) * 0.02f - GetN(4) * 0.005f - 0.25f;
        Console.WriteLine($"Коефіцієнт k = {_k}");
    }

    public int[,] GenerateMatrix()
    {
        var matrix = new int[_n, _n];

        for (var i = 0; i < _n; i++)
        {
            for (var j = 0; j < _n; j++)
            {
                double value = _random.NextDouble() * 2;
                value *= _k;
                value = value < 1 ? 0 : 1;

                matrix[i, j] = (int)value;
            }
        }

        return matrix;
    }

    public static void Await()
    {
        Console.Write($"Натисніть Enter, щоб продовжити...");
        Console.ReadLine();
    }

    public static void PrintMatrix(int[,] matrix)
    {
        Console.WriteLine("Матриця суміжності:");
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write($"{matrix[i, j]} ");
            }

            Console.WriteLine();
        }
    }
}