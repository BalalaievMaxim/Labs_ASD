using System.Linq;

namespace lab4;

public static class MatrixUtils
{
    public static void Print(int[,] matrix)
    {
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write($"{matrix[i, j]} ");
            }

            Console.WriteLine();
        }
    }

    public static int[,] ToUndirected(int[,] matrix)
    {
        int n = matrix.GetLength(0);
        for (var i = 0; i < n; i++)
        {
            for (var j = i + 1; j < n; j++)
            {
                matrix[j, i] = matrix[i, j];
            }
        }

        return matrix;
    }

    public static int[,] Multiply(int[,] matrix1, int[,] matrix2)
    {
        int n = matrix1.GetLength(0);
        int[,] result = new int[n, n];

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                result[i, j] = 0;
                for (int k = 0; k < n; k++)
                    result[i, j] += matrix1[i, k] * matrix2[k, j];
            }

        return result;
    }

    public static int[,] PerElementMultiply(int[,] matrix1, int[,] matrix2)
    {
        int n = matrix1.GetLength(0);
        int[,] result = new int[n, n];

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                result[i, j] = matrix1[i, j] * matrix2[i, j];

        return result;
    }

    public static (int, int)[] DirGraphVerticeDegrees(int[,] compatabilityMatrix)
    {
        int n = compatabilityMatrix.GetLength(0);
        (int, int)[] degrees = new (int, int)[n];

        for (int i = 0; i < n; i++)
        {
            (int, int) degree = (0, 0); // in, out
            for (int j = 0; j < n; j++)
            {
                degree.Item1 += compatabilityMatrix[j, i];
                degree.Item2 += compatabilityMatrix[i, j];
            }
            degrees[i] = degree;
        }
        return degrees;
    }

    public static int[] UndirGraphVerticeDegrees(int[,] compatabilityMatrix)
    {
        int n = compatabilityMatrix.GetLength(0);
        int[] degrees = new int[n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                degrees[i] += compatabilityMatrix[i, j];

                if (compatabilityMatrix[i, j] == 1 && i == j) degrees[i]++;
            }
        }

        return degrees;
    }

    public static void PrintVerticeDegrees(int[] undirDegrees)
    {
        System.Console.WriteLine("Степені вершин:");
        for (int i = 0; i < undirDegrees.Length; i++)
        {
            Console.WriteLine($"Вершина {i}: степінь = {undirDegrees[i]}");
        }
    }

    public static void PrintVerticeDegrees((int, int)[] dirDegrees)
    {
        System.Console.WriteLine("Степені вершин:");
        for (int i = 0; i < dirDegrees.Length; i++)
        {
            Console.WriteLine($"Вершина {i}: степінь = {dirDegrees[i].Item1 + dirDegrees[i].Item2} (від'ємна = {dirDegrees[i].Item2}, додатна = {dirDegrees[i].Item1})");
        }
    }

    public static void CheckForRegularity(int[] undirDegrees)
    {
        int degree = undirDegrees[0];

        if (undirDegrees.All(d => d == degree))
        {
            Console.WriteLine($"Граф регулярний, степінь однорідності: {degree}");
        }
        else
        {
            Console.WriteLine("Граф не регулярний");
        }
    }

    public static void CheckForRegularity((int, int)[] dirDegrees)
    {
        int degree = dirDegrees[0].Item1 + dirDegrees[0].Item2;

        if (dirDegrees.All(d => d.Item1 + d.Item2 == degree))
        {
            Console.WriteLine($"Граф регулярний, степінь однорідності: {degree}");
        }
        else
        {
            Console.WriteLine("Граф не регулярний");
        }
    }

    public static void CheckForHangingVertices(int[] undirDegrees)
    {
        bool flag = false;
        for (int i = 0; i < undirDegrees.Length; i++)
        {
            if (undirDegrees[i] == 1)
            {
                Console.WriteLine($"Вершина {i} - кінцева (висяча)");
                flag = true;
            }
        }

        if (!flag) Console.WriteLine("В графі немає кінцевих (висячих) вершин");
    }

    public static void CheckForHangingVertices((int, int)[] dirDegrees)
    {
        bool flag = false;
        for (int i = 0; i < dirDegrees.Length; i++)
        {
            if (dirDegrees[i].Item1 + dirDegrees[i].Item2 == 1)
            {
                Console.WriteLine($"Вершина {i} - кінцева (висяча)");
                flag = true;
            }
        }

        if (!flag) Console.WriteLine("В графі немає кінцевих (висячих) вершин");
    }

    public static void PrintPathsLength2(int[,] matrix)
    {
        System.Console.WriteLine("Всі шляхи довжиною 2:");

        int n = matrix.GetLength(0);
        int[,] matrixSquared = Multiply(matrix, matrix);

        for (int i = 0; i < n; i++)
        {
            bool first = true;
            for (int j = 0; j < n; j++)
            {
                if (matrixSquared[i, j] > 0)
                {
                    for (int k = 0; k < n; k++)
                    {
                        if (matrix[i, k] == 1 && matrix[k, j] == 1)
                        {
                            if (!first) Console.Write(", ");
                            first = false;
                            Console.Write($"{i} → {k} → {j}");
                        }
                    }
                }
            }
            Console.WriteLine("\n");
        }
    }

    public static void PrintPathsLength3(int[,] matrix)
    {
        System.Console.WriteLine("Всі шляхи довжиною 3:");

        int n = matrix.GetLength(0);
        int[,] matrixSquared = Multiply(matrix, matrix);
        int[,] matrixCubed = Multiply(matrixSquared, matrix);

        for (int i = 0; i < n; i++)
        {
            bool first = true;
            for (int j = 0; j < n; j++)
            {
                if (matrixCubed[i, j] > 0)
                {
                    for (int k = 0; k < n; k++)
                        for (int l = 0; l < n; l++)
                        {
                            if (matrix[i, k] == 1 && matrix[k, l] == 1 && matrix[l, j] == 1)
                            {
                                if (!first) Console.Write(", ");
                                first = false;
                                Console.Write($"{i} → {k} → {l} → {j}");
                            }
                        }
                }
            }
            Console.WriteLine("\n");
        }
    }

    public static int[,] GetReachabilityMatrix(int[,] adjMatrix)
    {
        int n = adjMatrix.GetLength(0);
        int[,] reach = new int[n, n];

        // Копія
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                reach[i, j] = adjMatrix[i, j];

        // Алгоритм Воршелла
        for (int k = 0; k < n; k++)
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (reach[i, k] == 1 && reach[k, j] == 1)
                        reach[i, j] = 1;

        return reach;
    }

    public static int[,] Transpone(int[,] matrix)
    {
        int n = matrix.GetLength(0);
        int[,] result = new int[n, n];

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                result[i, j] = matrix[j, i];

        return result;
    }

    public static int[,] GetStrongConnectivityMatrix(int[,] reachMatrix)
    {
        int[,] transpone = Transpone(reachMatrix);
        return PerElementMultiply(reachMatrix, transpone);
    }

    public static List<List<int>> GetStrongConnectedComponents(int[,] strongConnectivityMatrix)
    {
        int n = strongConnectivityMatrix.GetLength(0);
        bool[] visited = new bool[n];
        List<List<int>> components = [];

        for (int i = 0; i < n; i++)
        {
            if (!visited[i])
            {
                List<int> component = new List<int> { i };
                visited[i] = true;

                for (int j = i + 1; j < n; j++)
                {
                    if (!visited[j] && strongConnectivityMatrix[i, j] == 1)
                    {
                        component.Add(j);
                        visited[j] = true;
                    }
                }

                components.Add(component);
            }
        }

        return components;
    }

    public static void Print(List<List<int>> strongConnectedComponents)
    {
        System.Console.WriteLine("Компоненти сильної зв'язності:");
        for (int i = 0; i < strongConnectedComponents.Count; i++)
        {
            Console.WriteLine($"Компонента {i}: {string.Join(", ", strongConnectedComponents[i])}");
        }
    }

    public static (List<Vertex> condVertices, List<Edge> condEdges)
        ComputeCondensation(
            List<List<int>> strongConnectedComponents,
            List<Vertex> vertices,
            List<Edge> edges)
    {
        int n = vertices.Count;
        int[] compIndex = new int[n];
        for (int i = 0; i < strongConnectedComponents.Count; i++)
            foreach (int v in strongConnectedComponents[i])
                compIndex[v] = i;


        var vf = new VertexFactory(strongConnectedComponents.Count);
        vf.CreateAll();
        var condVertices = vf.Vertices;

        var condEdges = new List<Edge>();
        var seen = new HashSet<(int, int)>(); // уникаючи дублікатів
        foreach (var e in edges)
        {
            int cu = compIndex[e.From.Id];
            int cv = compIndex[e.To.Id];
            if (cu != cv && seen.Add((cu, cv)))
                condEdges.Add(new Edge(condVertices[cu], condVertices[cv], false));
        }

        return (condVertices, condEdges);
    }
}