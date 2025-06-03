namespace lab6;

public static class MatrixUtils
{
    public static event Action? OnRedrawNeeded;
    public static event Action<int, int>? OnEdgeAdded;
    public static event Action? OnPrimFinished;

    public static IEnumerable<Action> Prim(
        List<Vertex> vertices,
        int[,] adjacencyMatrix,
        int[,] weights
    )
    {
        Console.WriteLine("\nАлгоритм Пріма:");

        int n = vertices.Count;

        // чи в кістяку
        bool[] inMST = new bool[n];

        // мінімальна вага ребра для додавання
        int[] key = new int[n];

        // індекс "батьківської" вершини
        int[] parent = new int[n];

        // ініціалізація
        for (int i = 0; i < n; i++)
        {
            key[i] = int.MaxValue;
            parent[i] = -1;
        }

        for (int start = 0; start < n; start++)
        {
            if (inMST[start])
                continue;

            key[start] = 0;

            for (int count = 0; count < n; count++)
            {
                int u = -1;
                int minKey = int.MaxValue;
                for (int v = 0; v < n; v++)
                {
                    if (!inMST[v] && key[v] < minKey)
                    {
                        minKey = key[v];
                        u = v;
                    }
                }

                // якщо всі вершини вже в кістяку
                if (u == -1)
                    break;

                inMST[u] = true;

                if (parent[u] != -1)
                {
                    int pu = parent[u];
                    Console.WriteLine($"Додано ребро {vertices[pu].Id} -> {vertices[u].Id} (вага = {weights[pu, u]})");
                    OnEdgeAdded?.Invoke(pu, u);
                }

                for (int v = 0; v < n; v++)
                {
                    if (adjacencyMatrix[u, v] == 1 && !inMST[v])
                    {
                        int w = weights[u, v];
                        if (w >= 0 && w < key[v])
                        {
                            key[v] = w;
                            parent[v] = u;

                        }
                    }
                }

                vertices[u].State = VertexState.Closed;
                Console.WriteLine($"Вершина {vertices[u].Id} додана до мінімалього кістяка");
                yield return () => OnRedrawNeeded?.Invoke();
            }
        }

        OnPrimFinished?.Invoke();
    }

    public static int[,] ToSymmetrical(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[j, i] = matrix[i, j];
            }
        }

        return matrix;
    }

    public static int[,] Copy(int[,] matrix) => (int[,])matrix.Clone();
    public static void PrintMatrix(int[,] matrix) => Print(matrix, 1);
    public static void PrintWeights(int[,] matrix) => Print(matrix, 4);

    private static void Print(int[,] matrix, int order)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                int elem = matrix[i, j];

                if (elem == -1)
                {
                    Console.Write($"+inf ");
                }
                else
                {
                    Console.Write($"{new string(' ', order - elem.ToString().Length)}{elem} ");
                }

            }
            Console.WriteLine();
        }
    }
}