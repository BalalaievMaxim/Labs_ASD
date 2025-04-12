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
        LinkFactory linkFactory = new(nodeFactory);
        linkFactory.CreateAll(matrix);

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm(nodeFactory, linkFactory));
    }
}