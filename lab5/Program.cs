namespace lab5;

static class Program
{
    public static void Main()
    {
        Generator generator = new("4303");
        int[,] matrix = generator.GenerateMatrix();

        Console.WriteLine("\nМатриця суміжності напрямленого графа:");
        Generator.Print(matrix);

        Generator.Await();

        VertexFactory vertexFactory = new(10);
        vertexFactory.CreateAll();

        EdgeFactory directedEdgeFactory = new(vertexFactory);
        directedEdgeFactory.CreateAll(matrix);

        ApplicationConfiguration.Initialize();
        Application.Run(new DirectedForm(
            vertexFactory.Vertices,
            directedEdgeFactory.Edges,
            matrix,
            GraphTraversalStrategy.BFS
        ));
        vertexFactory.Reset();
        directedEdgeFactory.Reset();
        Application.Run(new DirectedForm(
            vertexFactory.Vertices,
            directedEdgeFactory.Edges,
            matrix,
            GraphTraversalStrategy.DFS
        ));
    }
}