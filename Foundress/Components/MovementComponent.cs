using Foundress.Entities;
using Microsoft.Xna.Framework;

namespace Foundress.Components
{
    public class MovementComponent : IComponent
    {
        private Entity _entity;
        private readonly float _speed;

        public MovementComponent(float speed)
        {
            _speed = speed;
        }

        public void SetEntity(Entity entity)
        {
            _entity = entity;
        }

        public void Update(GameTime gameTime)
        {
            _entity.Transform.Position += Vector3.Forward * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
