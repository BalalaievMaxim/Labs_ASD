namespace lab3;

static class Program
{
    public static void Main()
    {
        int[,] matrix = new Generator("4303").GenerateMatrix();
        Generator.PrintMatrix(matrix);
        // Generator.Await();

        NodeFactory nodeFactory = new(10);
        nodeFactory.CreateAll();

        LinkFactory undirectedLinkFactory = new(nodeFactory, true);
        undirectedLinkFactory.CreateAll(matrix);

        ApplicationConfiguration.Initialize();
        // new UndirectedForm(nodeFactory, linkFactory).Show();
        Application.Run(new UndirectedForm(nodeFactory, undirectedLinkFactory));
        // Application.Run(new CircleArrowForm());

    }
}