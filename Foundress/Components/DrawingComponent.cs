using Foundress.Entities;
using Foundress.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Foundress.Components
{
    public class DrawingComponent : IDrawableComponent
    {
        private Entity _entity;
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private GraphicsDevice _graphicsDevice;

        public DrawingComponent(GraphicsDevice graphicsDevice, VertexPositionColor[] vertices, short[] indices)
        {
            _graphicsDevice = graphicsDevice;

            _vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(vertices);

            _indexBuffer = new IndexBuffer(_graphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);
            _indexBuffer.SetData(indices);
        }

        public void SetEntity(Entity entity)
        {
            _entity = entity;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, BasicEffect effect)
        {
            effect.World = _entity.Transform.WorldMatrix;
            _graphicsDevice.SetVertexBuffer(_vertexBuffer);
            _graphicsDevice.Indices = _indexBuffer;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }
        }
    }
}
