using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Foundress.Components;
using Foundress.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Foundress.Entities
{
    public class AntEntity : Entity
    {
        public BoundingBox Bounds { get; set; }

        public AntEntity(GraphicsDevice graphicsDevice)
        {

            this.AddComponent(new DrawingComponent(graphicsDevice, Cube.GetScaledVertices(1, Color.Black), Cube.GetIndices()));
        }
    }
}
