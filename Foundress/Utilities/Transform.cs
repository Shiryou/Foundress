using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Foundress.Utilities
{
    public class Transform
    {
        public Vector3 Position = Vector3.Zero;
        public Quaternion Rotation = Quaternion.Identity;
        public Vector3 Scale = Vector3.One;

        public Matrix WorldMatrix =>
            Matrix.CreateScale(Scale) *
            Matrix.CreateFromQuaternion(Rotation) *
            Matrix.CreateTranslation(Position);
    }
}
