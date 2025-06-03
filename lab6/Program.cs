namespace lab6;

static class Program
{
    public static void Main()
    {
        Generator generator = new("4303");
        int[,] dirMatrix = generator.GenerateMatrix();
        int[,] undirMatrix = MatrixUtils.ToSymmetrical(dirMatrix);

        Console.WriteLine("\nМатриця суміжності ненапрямленого графа:");
        MatrixUtils.PrintMatrix(undirMatrix);

        int[,] weights = generator.Weights(undirMatrix);
        Console.WriteLine("\nМатриця ваг:");
        MatrixUtils.PrintWeights(weights);

        Generator.Await();

        VertexFactory vertexFactory = new(10);
        vertexFactory.CreateAll();

        EdgeFactory undirectedEdgeFactory = new(vertexFactory, weights);
        undirectedEdgeFactory.CreateAll(undirMatrix);

        ApplicationConfiguration.Initialize();
        Application.Run(new UndirectedForm(
            vertexFactory.Vertices,
            undirectedEdgeFactory.Edges,
            undirMatrix,
            weights
        ));
    }
}