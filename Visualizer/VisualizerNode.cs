using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphsLibrary;
using Microsoft.Xna.Framework;

namespace Visualizer
{
    class VisualizerNode : Node<Vector2>
    {
        public Rectangle HitBox;

        public VisualizerNode(Vector2 pos) : base(pos)
        {

        }
    }
}
