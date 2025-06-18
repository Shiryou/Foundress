using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Foundress.Components
{
    public interface IDrawableComponent : IComponent
    {
        void Draw(GameTime gameTime, BasicEffect basicEffect);
    }
}
