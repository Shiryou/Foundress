using System.Collections.Generic;
using Foundress.Components;
using Foundress.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Foundress.Entities
{
    public class Entity
    {
        public Transform Transform { get; } = new Transform();
        private readonly List<IComponent> _components = new();
        private readonly List<IDrawableComponent> _drawableComponents = new();

        public void AddComponent(IComponent component)
        {
            component.SetEntity(this);
            _components.Add(component);
            if (component is IDrawableComponent drawable)
                _drawableComponents.Add(drawable);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var c in _components)
                c.Update(gameTime);
        }

        public void Draw(GameTime gameTime, BasicEffect basicEffect)
        {
            foreach (var d in _drawableComponents)
                d.Draw(gameTime, basicEffect);
        }
    }
}
