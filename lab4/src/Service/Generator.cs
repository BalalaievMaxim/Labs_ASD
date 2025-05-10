namespace lab4;

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

        K = 1 - GetN(3) * 0.01f - GetN(4) * 0.001f - 0.3f;
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
}