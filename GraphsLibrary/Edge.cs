using System;
using System.Collections.Generic;
using System.Text;

namespace GraphsLibrary
{
    public class Edge<T>
    {
        public Node<T> FromNode;
        public Node<T> ToNode;
        public float Weight;

        public Edge(Node<T> fromNode, Node<T> toNode, float weight)
        {
            FromNode = fromNode;
            ToNode = toNode;
            Weight = weight;
        }
        public override string ToString()
        {
            return $"SourceNode Value: {FromNode.Value}, EndNode Value: {ToNode.Value}, Weight: {Weight}";
        }
    }
}
