using System;

namespace UnweightedDirectedGraphs
{
    class Program
    {
        static void Main(string[] args)
        {
            UnweightedDirectedGraph<int> graph = new UnweightedDirectedGraph<int>();

            graph.AddNode(1);
            graph.AddNode(2);
            graph.AddNode(3);
            graph.AddNode(4);
            graph.AddNode(5);
            graph.AddNode(6);

            graph.AddEdge(2, 3, 1);
            graph.AddEdge(3, 2, 1);

            graph.AddEdge(1, 4, 1);
            graph.AddEdge(1, 5, 1);
            graph.AddEdge(4, 5, 1);
            graph.AddEdge(5, 6, 1);
            

            // var allNodes = graph.BreadthFirst(1);
            var path = graph.BetterPath(graph.Search(1), graph.Search(6));
        }
    }
}
