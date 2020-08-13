using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Text;


namespace GraphsLibrary
{
    public class Node<T>
    {
        public T Value;
        public List<Edge<T>> PointingTo;
        public bool Visited;
        public Node<T> Parent;
        public float DistanceFromStart;
        public float DistanceToEnd;
        public Vector2 Position;
        public Node(T value = default)
        {
            Value = value;
            PointingTo = new List<Edge<T>>();
            //Node<T> Parent = new Node<T>();
            DistanceFromStart = 0;
            DistanceToEnd = float.MaxValue;
            Position = Vector2.Zero;
        }

        public override string ToString()
        {
            return $"Value: {Value}, PointTo Count: {PointingTo.Count}";
        }
    }
}
