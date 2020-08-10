using System;
using System.Collections.Generic;
using System.Text;

namespace UnweightedDirectedGraphs
{
    public class UnweightedDirectedGraph<T>
    {

        public HashSet<Node<T>> Nodes;
        public int Count => Nodes.Count;


        public UnweightedDirectedGraph()
        {
            Nodes = new HashSet<Node<T>>();
        }

        public void AddNode(T value)
        {
            if (Search(value) != null)
            {
                return;
            }

            Nodes.Add(new Node<T>(value));
        }

        public bool AddEdge(T FromValue, T ToValue, int Weight)
        {
            Node<T> FromNode = Search(FromValue);
            Node<T> ToNode = Search(ToValue);
            if (FromNode != null && ToNode != null)
            {
                FromNode.PointingTo.Add(new Edge<T>(FromNode, ToNode, Weight));
                return true;
            }
            return false;
        }

        public bool Remove(T value)
        {
            Node<T> removeNode = Search(value);
            if (removeNode == null)
            {
                return false;
            }

            foreach (var node in Nodes)
            {
                if (TryGetEdges(node, removeNode, out List<Edge<T>> edges))
                {
                    foreach (var edge in edges)
                    {
                        RemoveEdge(edge);
                    }
                }
            }
            Nodes.Remove(removeNode);
            return true;
        }

        public bool RemoveEdge(Edge<T> removeEdge)
        {
            if (!TryGetEdges(removeEdge.FromNode, removeEdge.ToNode, out List<Edge<T>> edges))
            {
                return false;
            }
            foreach (var edge in edges)
            {
                if (edge.Weight == removeEdge.Weight)
                {
                    removeEdge.FromNode.PointingTo.Remove(edge);
                    return true;
                }
            }
            return false;

        }

        public bool TryGetEdges(Node<T> FromNode, Node<T> ToNode, out List<Edge<T>> edges)
        {
            edges = new List<Edge<T>>();
            foreach (var edge in FromNode.PointingTo)
            {
                if (edge.ToNode == ToNode)
                {
                    edges.Add(edge);
                }
            }
            return edges.Count >= 1;
        }

        public Node<T> Search(T value)
        {
            foreach (Node<T> node in Nodes)
            {
                if (node.Value.Equals(value))
                {
                    return node;
                }
            }
            return null;
        }
        /// <summary>
        /// First, cheack start node
        /// Next, have a visited list made of nodes and a stack to hold your nodes
        /// while the stack has items:
        /// set current node to stack pop
        /// add current node to visited
        /// loop through all neibors
        /// if the neibor is not visted: push the neibor to the stack
        /// </summary>
        /// <param name="FromNode"></param>
        /// <param name="ToNode"></param>
        public List<Node<T>> DepthFirst(Node<T> fromNode, Node<T> toNode)
        {
            if (fromNode == null)
            {
                return null;
            }
            Stack<Node<T>> stack = new Stack<Node<T>>();
            stack.Push(fromNode);
            List<Node<T>> nodes = new List<Node<T>>();
            List<T> values = new List<T>();

            while (stack.Count > 0)
            {
                var node = stack.Pop();
                nodes.Add(node);
                values.Add(node.Value);

                foreach (var edge in node.PointingTo)
                {
                    if (!nodes.Contains(edge.ToNode))
                    {
                        stack.Push(edge.ToNode);
                    }
                }

                if(node == toNode)
                {
                    return nodes;
                }
            }

            return nodes;
        }

        //public List<T> BreadthFirst(T value, T toValue)
        //{
        //    return breadthFirst(Search(value), Search(toValue));
        //}

        List<Node<T>> breadthFirst(Node<T> fromNode, Node<T> toNode)
        {
            if (fromNode == null)
            {
                return null;
            }
            Queue<Node<T>> queue = new Queue<Node<T>>();
            queue.Enqueue(fromNode);
            List<Node<T>> nodes = new List<Node<T>>();
            List<T> values = new List<T>();

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (!nodes.Contains(node))//this is to make sure there aren't repets in the final list
                {
                    nodes.Add(node);
                    values.Add(node.Value);

                    foreach (var edge in node.PointingTo)
                    {
                        if (!nodes.Contains(edge.ToNode))
                        {
                            queue.Enqueue(edge.ToNode);
                        }
                    }

                    if(node == toNode)
                    {
                        return nodes;
                    }
                }
            }


            return nodes;
        }

        //public List<Node<T>> SingleShortestPath(Node<T> fromNode, Node<T> toNode)
        //{
        //    if (fromNode == null)
        //    {
        //        return null;
        //    }
        //    Queue<Node<T>> queue = new Queue<Node<T>>();
        //    queue.Enqueue(fromNode);
        //    List<Node<T>> nodes = new List<Node<T>>();
        //    List<List<Node<T>>> paths = new List<List<Node<T>>>();
        //    List<int> pathWieghts = new List<int>();

        //    bool go;
        //    bool stop = false;

        //    while (queue.Count > 0)
        //    {
        //        var node = queue.Dequeue();
        //        if (!nodes.Contains(node))
        //        {
        //            nodes.Add(node);
        //            go = true;
        //            if(nodes.Count <= 2)
        //            {
        //                for (int i = 0; i < node.PointingTo.Count; i++)
        //                {
        //                    paths.Add(new List<Node<T>>());
        //                    paths[paths.Count - 1].Add(node.PointingTo[i].ToNode);
        //                    pathWieghts.Add(node.PointingTo[i].Weight);
        //                }
        //                go = false;
        //            }

        //            if (go && TryGetEdges(nodes[nodes.Count - 2], node, out List<Edge<T>> edges))
        //            {
        //                for (int i = 0; i < edges.Count; i++)
        //                {
        //                    for (int j = 0; j < paths.Count; j++)
        //                    {
        //                        if (edges[i].FromNode == paths[j][paths[j].Count - 1])
        //                        {
        //                            paths[j].Add(edges[i].ToNode);
        //                            pathWieghts[j] += edges[i].Weight;
        //                        }
        //                        else
        //                        {
        //                            foreach (var path in paths)
        //                            {
        //                                if (path[path.Count - 1] == toNode || pathWieghts.Count < Nodes.Count)
        //                                {
        //                                    goto done;
        //                                }
        //                            }
        //                            int smallestIndex = 0;
        //                            for (int k = 0; k < pathWieghts.Count; k++)
        //                            {
        //                                if (pathWieghts[k] < pathWieghts[smallestIndex])
        //                                {
        //                                    smallestIndex = k;
        //                                }
        //                            }
        //                            paths.Add(new List<Node<T>>());
        //                            pathWieghts.Add(pathWieghts[smallestIndex]);
        //                            for (int k = 0; k < paths[smallestIndex].Count; k++)
        //                            {
        //                                paths[paths.Count - 1].Add(paths[smallestIndex][k]);
        //                            }
        //                            paths[paths.Count - 1].Add(edges[i].ToNode);
        //                            pathWieghts[pathWieghts.Count - 1] += edges[i].Weight;
        //                        }
        //                    }
        //                }
        //            }

        //            done:

        //            foreach (var edge in node.PointingTo)
        //            {
        //                if (!nodes.Contains(edge.ToNode))
        //                {
        //                    queue.Enqueue(edge.ToNode);
        //                }
        //            }
        //        }

        //    }

        //    //int smallestweight = pathWieghts[0];
        //    //for (int i = 0; i < pathWieghts.Count; i++)
        //    //{
        //    //    if(pathWieghts[i] < pathWieghts[smallestweight])
        //    //    {
        //    //        smallestweight = i;
        //    //    }
        //    //}

        //    return nodes;
        //}

        //public List<Node<T>> BetterSingleShortestPath(Node<T> fromNode, Node<T> toNode)
        //{
        //    List<Node<T>> pred = new List<Node<T>>();
        //    List<int> distances = new List<int>();

        //    List<Node<T>> path = new List<Node<T>>();
        //    Node<T> currentNode = toNode;

        //    while(pred[])
        //}

        public (List<Node<T>>, bool) BetterPath(Node<T> fromNode, Node<T> toNode)//true = breadth, false = depth
        {
            List<Node<T>> depthFirstPath = DepthFirst(fromNode, toNode);
            List<Node<T>> breadthFirstPath = breadthFirst(fromNode, toNode);

            int depthDistance = 0;
            int breadthDistance = 0;

            for (int i = 0; i < depthFirstPath.Count - 1; i++)
            {
                foreach (var edge in depthFirstPath[i].PointingTo)
                {
                    if (edge.ToNode == depthFirstPath[i + 1])
                    {
                        depthDistance += edge.Weight;
                    }
                }
            }

            for (int i = 0; i < breadthFirstPath.Count - 1; i++)
            {
                foreach (var edge in breadthFirstPath[i].PointingTo)
                {
                    if (edge.ToNode == breadthFirstPath[i + 1])
                    {
                        breadthDistance += edge.Weight;
                    }
                }
            }
            return depthDistance <= breadthDistance ? (depthFirstPath, false) : (breadthFirstPath, true);

        }
    }
}
