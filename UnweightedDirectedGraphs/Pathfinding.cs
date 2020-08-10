using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace UnweightedDirectedGraphs
{
    //queue = new PriorityQueue<Vertex<T>>(Comparer<Vertex<T>>.Create((a, b) => info[a].distance.CompareTo(info[b].distance)));
    class Pathfinding<T> where T : IComparer<T>
    {
        UnweightedDirectedGraph<T> graph;
        public Pathfinding(UnweightedDirectedGraph<T> Graph)
        {
            graph = Graph;
        }
        public Stack<Node<T>> Dijkstras(Node<T> fromNode, Node<T> toNode)
        {
            Dictionary<Node<T>, (Node<T> parent, int distance)> info = new Dictionary<Node<T>, (Node<T> parent, int distance)>();
            HeapTree<Node<T>> queue = new HeapTree<Node<T>>(Comparer<Node<T>>.Create((x, y) => info[x].distance.CompareTo(info[y].distance)));

            foreach(var node in graph.Nodes)
            {
                node.Visited = false;
                info.Add(node, (null, int.MaxValue));
            }

            info[fromNode] = (null, 0);
            queue.Insert(fromNode);

            Node<T> currentNode;

            while(queue.Count > 0)
            {
                currentNode = queue.Pop();
                currentNode.Visited = true;

                int neighborDistance;

                foreach (var edge in currentNode.PointingTo)
                {
                    Node<T> neighbor = edge.ToNode;
                    neighborDistance = edge.Weight + info[currentNode].distance;
                    if(neighborDistance < info[neighbor].distance)
                    {
                        info[neighbor] = (currentNode, neighborDistance);
                        neighbor.Visited = false;
                    }

                    if(neighbor.Visited == false)
                    {
                        queue.Insert(neighbor);
                    }
                }
                if(currentNode == toNode)
                {
                    break;
                }
            }

            //path time

            Stack<Node<T>> path = new Stack<Node<T>>();
            currentNode = info.Where(node => node.Key == toNode).First().Key;

            while(info[currentNode].parent != null)
            {
                path.Push(currentNode);

                currentNode = info[currentNode].parent;
            }
            if(path.Count <= 1)
            {
                return null;
            }
            return path;
        }
    }
}
