using Foundress.Entities;

using Microsoft.Xna.Framework;

namespace Foundress.Components
{
    public interface IComponent
    {
        void SetEntity(Entity entity) {}
        void Update(GameTime gameTime);
    }
}
