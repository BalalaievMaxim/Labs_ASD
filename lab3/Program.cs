namespace lab3;

static class Program
{
    public static void Main()
    {
        Generator generator = new("4303");
        int[,] matrix = generator.GenerateMatrix();

        System.Console.WriteLine("Матриця суміжності напрямленого графа:");
        Generator.PrintMatrix(matrix);
        System.Console.WriteLine("Матриця суміжності ненапрямленого графа:");
        Generator.PrintMatrix(generator.MakeMatrixUndirected(matrix));

        Generator.Await();

        NodeFactory nodeFactory = new(10);
        nodeFactory.CreateAll();

        LinkFactory directedLinkFactory = new(nodeFactory);
        directedLinkFactory.CreateAll(matrix);
        LinkFactory undirectedLinkFactory = new(nodeFactory, true);
        undirectedLinkFactory.CreateAll(matrix);

        ApplicationConfiguration.Initialize();
        new DirectedForm(nodeFactory, directedLinkFactory).Show();
        Application.Run(new UndirectedForm(nodeFactory, undirectedLinkFactory));
    }
}