using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Maze_Adventure
{
    /// <summary>
    /// Represent a grid.
    /// </summary>
    class Grid
    {
        /// <summary>
        /// Grid's lines texture
        /// </summary>
        Texture2D texture;
        /// <summary>
        /// Tile's width
        /// </summary>
        float spaceX;
        /// <summary>
        /// Tile's height
        /// </summary>
        float spaceY;
        /// <summary>
        /// Grid's destination rectangle
        /// </summary>
        Rectangle desRectangle;

        /// <summary>
        /// Represent a number of tiles in grid's width.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Represent a number of tiles in grid's height.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Grid's lines color
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Grid's lines width
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Initialize new grid.
        /// </summary>
        /// <param name="x">Number of tiles int grid's width.</param>
        /// <param name="y">Number of tiles in grid's height.</param>
        /// <param name="desRectangle">Grid's destination rectangle</param>
        /// <param name="graphicsDevice">Graphics device for creating lines texture.</param>
        /// <param name="width">Grid's lines width</param>
        public Grid(int x, int y, Rectangle desRectangle, GraphicsDevice graphicsDevice, float width)
        {
            X = x;
            Y = y;
            Width = width;
            this.desRectangle = desRectangle;
            Color = Color.Black;

            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });

            spaceX = desRectangle.Width / (float)x;
            spaceY = desRectangle.Height / (float)y;
        }

        /// <summary>
        /// Draw grid
        /// </summary>
        /// <param name="spriteBatch">Sprite batch for draw the grid.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < X + 1; i++)
            {
                for (int j = 0; j < Y + 1; j++)
                {
                    spriteBatch.Draw(texture, new Rectangle((int)(i * spaceX + desRectangle.X - Width/2f), desRectangle.Y, (int)Width, desRectangle.Height), Color);
                    spriteBatch.Draw(texture, new Rectangle(desRectangle.X, (int)(j * spaceY + desRectangle.Y - Width/2f), desRectangle.Width, (int)Width), Color);
                }
            }
        }

    }
}
