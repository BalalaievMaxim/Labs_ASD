namespace lab5;

public static class GraphSearchUtils
{
    public static event Action<Vertex>? OnVertexVisited;
    public static event Action<int, int>? OnEdgeVisited;
    public static event Action<int[,]>? OnBFSFinished;
    public static event Action<int[,]>? OnDFSFinished;

    public static IEnumerable<Action> BFS(List<Vertex> vertices, int[,] adjacencyMatrix, int startId)
    {
        System.Console.WriteLine("Обхід в ширину (BFS):");

        int n = vertices.Count;
        bool[] visited = new bool[n];
        int[] parent = new int[n];
        for (int i = 0; i < n; i++)
            parent[i] = -1;

        int[,] treeTraversalMatrix = new int[n, n];
        Queue<int> queue = new();
        visited[startId] = true;
        queue.Enqueue(startId);

        int j = 0;
        while (queue.Count > 0)
        {
            int u = queue.Dequeue();
            yield return () => OnVertexVisited?.Invoke(vertices[u]);
            Console.WriteLine($"Вершина {vertices[u].Id} відвідана {++j}-ою");

            for (int v = 0; v < n; v++)
            {
                if (adjacencyMatrix[u, v] == 1 && !visited[v])
                {
                    visited[v] = true;
                    parent[v] = u;
                    treeTraversalMatrix[u, v] = 1;
                    queue.Enqueue(v);
                    yield return () => OnEdgeVisited?.Invoke(u, v);
                }
            }
        }

        Console.WriteLine("\nОбхід в ширину завершено.");
        Console.WriteLine("Матриця суміжності дерева обходу: ");
        OnBFSFinished?.Invoke(treeTraversalMatrix);
    }

    public static IEnumerable<Action> DFS(List<Vertex> vertices, int[,] adjacencyMatrix, int startId)
    {
        System.Console.WriteLine("\nОбхід в глибину (DFS):");

        int n = vertices.Count;

        bool[] visited = new bool[n];
        int[] parent = new int[n];
        for (int i = 0; i < n; i++)
            parent[i] = -1;

        int[,] treeTraversalMatrix = new int[n, n];

        int j = 0;
        // рекурсія
        IEnumerable<Action> DFS(int u)
        {
            visited[u] = true;
            yield return () => OnVertexVisited?.Invoke(vertices[u]);
            Console.WriteLine($"Вершина {vertices[u].Id} відвідана {++j}-ою");

            for (int v = 0; v < n; v++)
            {
                if (adjacencyMatrix[u, v] == 1 && !visited[v])
                {
                    parent[v] = u;
                    treeTraversalMatrix[u, v] = 1;
                    yield return () => OnEdgeVisited?.Invoke(u, v);

                    foreach (var action in DFS(v))
                        yield return action;
                }
            }
        }

        foreach (var action in DFS(startId))
            yield return action;

        Console.WriteLine("\nОбхід в глибину завершено.");
        Console.WriteLine("Матриця суміжності дерева обходу: ");
        OnDFSFinished?.Invoke(treeTraversalMatrix);
    }
}