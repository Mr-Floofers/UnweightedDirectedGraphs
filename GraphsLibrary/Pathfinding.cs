using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GraphsLibrary
{
    public class Pathfinding<T> //where T : IComparer<T>
    {
        UnweightedDirectedGraph<T> graph;
        //Func<Node<T>, Node<T>, float> Heuristics;
        //delegate float Heuristics(Node<T> first, Node<T> second);
        public Pathfinding(UnweightedDirectedGraph<T> Graph)
        {
            graph = Graph;            
        }
        public (Stack<Node<T>>, Queue<Node<T>>) Dijkstras(Node<T> fromNode, Node<T> toNode)
        {
            //Dictionary<Node<T>, (Node<T> parent, int distance)> info = new Dictionary<Node<T>, (Node<T> parent, int distance)>();
            HeapTree<Node<T>> queue = new HeapTree<Node<T>>(Comparer<Node<T>>.Create((x, y) => x.DistanceFromStart.CompareTo(y.DistanceFromStart)));
            Queue<Node<T>> visitedNodes = new Queue<Node<T>>();

            foreach(var node in graph.Nodes)
            {
                node.Visited = false;
                node.Parent = null;
                node.DistanceFromStart = float.MaxValue;
            }

            fromNode.DistanceFromStart = 0;
            queue.Insert(fromNode);

            Node<T> currentNode;

            while(queue.Count > 0)
            {
                currentNode = queue.Pop();
                currentNode.Visited = true;
                visitedNodes.Enqueue(currentNode);

                float neighborDistance;

                foreach (var edge in currentNode.PointingTo)
                {
                    Node<T> neighbor = edge.ToNode;
                    neighborDistance = edge.Weight + currentNode.DistanceFromStart;
                    if(neighborDistance.CompareTo(neighbor.DistanceFromStart) < 0)
                    {
                        neighbor.Parent = currentNode; 
                        neighbor.DistanceFromStart = neighborDistance;
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
            currentNode = toNode;

            while(currentNode.Parent != null)
            {
                path.Push(currentNode);

                currentNode = currentNode.Parent;
            }
            return (path, visitedNodes);
        }

        public void AStar(Node<T> fromNode, Node<T> toNode, Func<Node<T>, Node<T>, float> heuristicFunc = null)
        {
            if (heuristicFunc is null)
            {
                heuristicFunc = Manhattan;
            }
            HeapTree<Node<T>> queue = new HeapTree<Node<T>>(Comparer<Node<T>>.Create((x, y) => x.DistanceToEnd.CompareTo(y.DistanceToEnd)));

            foreach (var node in graph.Nodes)
            {
                node.Visited = false;
                node.Parent = null;
                node.DistanceFromStart = float.MaxValue;
                node.DistanceToEnd = float.MaxValue;
            }

            fromNode.DistanceFromStart = 0;
            fromNode.DistanceToEnd = heuristicFunc(fromNode, toNode);
            queue.Insert(fromNode);

            Node<T> currentNode;

            while (queue.Count > 0)
            {
                currentNode = queue.Pop();
                currentNode.Visited = true;

                float neighborDistance;

                foreach (var edge in currentNode.PointingTo)
                {
                    Node<T> neighbor = edge.ToNode;
                    neighborDistance = edge.Weight + currentNode.DistanceFromStart;
                    if (neighborDistance.CompareTo(neighbor.DistanceFromStart) < 0)
                    {
                        neighbor.Parent = currentNode;
                        neighbor.DistanceFromStart = neighborDistance;
                        neighbor.Visited = false;
                        neighbor.DistanceToEnd = heuristicFunc(neighbor, toNode);
                    }

                    if (neighbor.Visited == false)
                    {
                        queue.Insert(neighbor);
                    }
                }
                if (currentNode == toNode)
                {
                    break;
                }
            }
        }


        //heuristics
        public float Manhattan(Node<T> fromNode, Node<T> endNode)
        {
            var disX = Math.Abs(fromNode.Position.X - endNode.Position.X);
            var disY = Math.Abs(fromNode.Position.Y - endNode.Position.Y);

            return disX + disY;
        }
    }
}
