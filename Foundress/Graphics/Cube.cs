using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Foundress.Graphics
{
    public class Cube
    {
        // Base cube size
        private const float BASE_SIZE = 1f;
        
        // Define vertices for each face (24 vertices total - 4 per face, 6 faces)
        public static VertexPositionColor[] vertices = GetScaledVertices(1);

        // Define cube indices (12 triangles = 36 indices)
        public static short[] indices = new short[]
        {
            // Front face (clockwise when viewed from outside)
            0, 1, 2, 0, 2, 3,
            // Back face (clockwise when viewed from outside)
            4, 6, 5, 4, 7, 6,
            // Top face (clockwise when viewed from outside)
            8, 10, 9, 8, 11, 10,
            // Bottom face (clockwise when viewed from outside)
            12, 14, 13, 12, 15, 14,
            // Left face (clockwise when viewed from outside)
            16, 18, 17, 16, 19, 18,
            // Right face (clockwise when viewed from outside)
            20, 22, 21, 20, 23, 22
        };

        /// <summary>
        /// Creates scaled vertices for the cube with custom colors for each face.
        /// </summary>
        /// <param name="scaleLevel">The integer scale level (1 = base size)</param>
        /// <param name="frontColor">Color for the front face</param>
        /// <param name="backColor">Color for the back face</param>
        /// <param name="topColor">Color for the top face</param>
        /// <param name="bottomColor">Color for the bottom face</param>
        /// <param name="leftColor">Color for the left face</param>
        /// <param name="rightColor">Color for the right face</param>
        /// <returns>Scaled vertices array</returns>
        public static VertexPositionColor[] GetScaledVertices(int scaleLevel,
            Color frontColor, Color backColor, Color topColor,
            Color bottomColor, Color leftColor, Color rightColor)
        {
            if (scaleLevel <= 0)
                throw new ArgumentException("Scale level must be greater than 0", nameof(scaleLevel));

            float scale = BASE_SIZE * scaleLevel;

            return new VertexPositionColor[]
            {
                // Front face
                new VertexPositionColor(new Vector3(-scale, -scale, -scale), frontColor),
                new VertexPositionColor(new Vector3(-scale,  scale, -scale), frontColor),
                new VertexPositionColor(new Vector3( scale,  scale, -scale), frontColor),
                new VertexPositionColor(new Vector3( scale, -scale, -scale), frontColor),
                
                // Back face
                new VertexPositionColor(new Vector3(-scale, -scale,  scale), backColor),
                new VertexPositionColor(new Vector3(-scale,  scale,  scale), backColor),
                new VertexPositionColor(new Vector3( scale,  scale,  scale), backColor),
                new VertexPositionColor(new Vector3( scale, -scale,  scale), backColor),
                
                // Top face
                new VertexPositionColor(new Vector3(-scale,  scale, -scale), topColor),
                new VertexPositionColor(new Vector3(-scale,  scale,  scale), topColor),
                new VertexPositionColor(new Vector3( scale,  scale,  scale), topColor),
                new VertexPositionColor(new Vector3( scale,  scale, -scale), topColor),
                
                // Bottom face
                new VertexPositionColor(new Vector3(-scale, -scale, -scale), bottomColor),
                new VertexPositionColor(new Vector3(-scale, -scale,  scale), bottomColor),
                new VertexPositionColor(new Vector3( scale, -scale,  scale), bottomColor),
                new VertexPositionColor(new Vector3( scale, -scale, -scale), bottomColor),
                
                // Left face
                new VertexPositionColor(new Vector3(-scale, -scale, -scale), leftColor),
                new VertexPositionColor(new Vector3(-scale, -scale,  scale), leftColor),
                new VertexPositionColor(new Vector3(-scale,  scale,  scale), leftColor),
                new VertexPositionColor(new Vector3(-scale,  scale, -scale), leftColor),
                
                // Right face
                new VertexPositionColor(new Vector3( scale, -scale, -scale), rightColor),
                new VertexPositionColor(new Vector3( scale, -scale,  scale), rightColor),
                new VertexPositionColor(new Vector3( scale,  scale,  scale), rightColor),
                new VertexPositionColor(new Vector3( scale,  scale, -scale), rightColor)
            };
        }

        /// <summary>
        /// Creates scaled vertices for the cube with a single color.
        /// </summary>
        /// <param name="scaleLevel">The integer scale level (1 = base size)</param>
        /// <param name="color">The color for all faces</param>
        /// <returns>Scaled vertices array</returns>
        public static VertexPositionColor[] GetScaledVertices(int scaleLevel, Color color)
        {
            return GetScaledVertices(scaleLevel, color, color, color, color, color, color);
        }

        /// <summary>
        /// Creates scaled vertices for the cube based on the given scale level.
        /// Scale level 1 = base size, scale level 2 = 2x size, etc.
        /// </summary>
        /// <param name="scaleLevel">The integer scale level (1 = base size)</param>
        /// <returns>Scaled vertices array</returns>
        public static VertexPositionColor[] GetScaledVertices(int scaleLevel)
        {
            return GetScaledVertices(scaleLevel, Color.White);
        }

        /// <summary>
        /// Creates scaled vertices for the cube with the default rainbow colors.
        /// </summary>
        /// <param name="scaleLevel">The integer scale level (1 = base size)</param>
        /// <returns>Scaled vertices array with rainbow colors</returns>
        public static VertexPositionColor[] GetScaledVerticesRainbow(int scaleLevel)
        {
            return GetScaledVertices(scaleLevel, Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Purple);
        }

        /// <summary>
        /// Gets the indices array (same for all scale levels)
        /// </summary>
        /// <returns>The indices array</returns>
        public static short[] GetIndices()
        {
            return indices;
        }
    }
}
