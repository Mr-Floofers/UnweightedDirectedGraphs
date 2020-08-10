using System;
using System.Collections.Generic;
using System.Text;

namespace UnweightedDirectedGraphs
{
    public class Node<T>
    {
        public T Value;
        public List<Edge<T>> PointingTo;
        public bool Visited;
        public Node(T value = default)
        {
            Value = value;
            PointingTo = new List<Edge<T>>();
        }

        public override string ToString()
        {
            return $"Value: {Value}, PointTo Count: {PointingTo.Count}";
        }
    }
}
