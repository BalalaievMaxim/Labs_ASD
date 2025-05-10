namespace lab4;

static class Program
{
    public static void Main()
    {
        Generator generator = new("4303");
        int[,] matrix = generator.GenerateMatrix();

        // Напрямлений граф
        System.Console.WriteLine("\nМатриця суміжності напрямленого графа:");
        MatrixUtils.Print(matrix);
        (int, int)[] dirDegrees = MatrixUtils.DirGraphVerticeDegrees(matrix);
        MatrixUtils.PrintVerticeDegrees(dirDegrees);
        MatrixUtils.CheckForRegularity(dirDegrees);
        MatrixUtils.CheckForHangingVertices(dirDegrees);

        // Ненапрямлений граф
        System.Console.WriteLine("\nМатриця суміжності ненапрямленого графа:");
        MatrixUtils.Print(MatrixUtils.ToUndirected(matrix));
        int[] undirDegrees = MatrixUtils.UndirGraphVerticeDegrees(matrix);
        MatrixUtils.PrintVerticeDegrees(undirDegrees);
        MatrixUtils.CheckForRegularity(undirDegrees);
        MatrixUtils.CheckForHangingVertices(undirDegrees);

        // Модифікований граф
        System.Console.WriteLine("\nНовий напрямлений граф");
        generator.K = 1 - generator.GetN(3) * 0.005f - generator.GetN(4) * 0.005f - 0.27f;
        System.Console.WriteLine($"Коефіцієнт k = {generator.K}");
        int[,] newMatrix = generator.GenerateMatrix();
        System.Console.WriteLine("Матриця суміжності:");
        MatrixUtils.Print(newMatrix);

        // Пункт 1
        (int, int)[] newDirDegrees = MatrixUtils.DirGraphVerticeDegrees(newMatrix);
        MatrixUtils.PrintVerticeDegrees(newDirDegrees);
        // Пункт 2
        MatrixUtils.PrintPathsLength2(newMatrix);
        MatrixUtils.PrintPathsLength3(newMatrix);
        // Пункт 3
        int[,] reachMatrix = MatrixUtils.GetReachabilityMatrix(newMatrix);
        System.Console.WriteLine("Матриця досяжності:");
        MatrixUtils.Print(reachMatrix);
        // Пункт 4
        int[,] strongConnectivityMatrix = MatrixUtils.GetStrongConnectivityMatrix(reachMatrix);
        System.Console.WriteLine("Матриця сильної зв'язності:");
        MatrixUtils.Print(MatrixUtils.GetStrongConnectivityMatrix(reachMatrix));
        // Пункт 5
        var components = MatrixUtils.GetStrongConnectedComponents(strongConnectivityMatrix);
        MatrixUtils.Print(components);

        Generator.Await();

        VertexFactory verticeFactory = new(10);
        verticeFactory.CreateAll();

        EdgeFactory directedEdgeFactory = new(verticeFactory);
        directedEdgeFactory.CreateAll(matrix);
        EdgeFactory undirectedEdgeFactory = new(verticeFactory, true);
        undirectedEdgeFactory.CreateAll(matrix);

        EdgeFactory newDirectedEdgeFactory = new(verticeFactory);
        newDirectedEdgeFactory.CreateAll(newMatrix);

        // Пункт 6
        var (condVertices, condEdges) = MatrixUtils.ComputeCondensation(components, verticeFactory.Vertices, undirectedEdgeFactory.Edges);

        ApplicationConfiguration.Initialize();
        new DirectedForm(verticeFactory.Vertices, directedEdgeFactory.Edges).Show();
        new UndirectedForm(verticeFactory.Vertices, undirectedEdgeFactory.Edges).Show();
        new DirectedForm(verticeFactory.Vertices, newDirectedEdgeFactory.Edges).Show();
        Application.Run(new DirectedForm(condVertices, condEdges));
    }
}