using System.Diagnostics;

namespace lab3;

static class Program
{
    public static void Main()
    {
        NodeFactory nodeFactory = new(10);
        nodeFactory.CreateAll();

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm(nodeFactory));
    }
}