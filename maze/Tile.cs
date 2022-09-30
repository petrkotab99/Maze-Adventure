using System;
using System.Linq;

using Gaming;
using Gaming.Algorithms.PathFinding;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Maze_Adventure
{
    /// <summary>
    /// Represent a tile in maze.
    /// </summary>
    class Tile : Node
    {

        /// <summary>
        /// X coordinate in maze.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y coordinates in maze.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Tile's sprite
        /// </summary>
        public Sprite Sprite { get; private set; }

        /// <summary>
        /// Determine if tile has collision with entities.
        /// </summary>
        public bool Collision { get; set; }

        /// <summary>
        /// Initialize new tile.
        /// </summary>
        /// <param name="sprite">Tile's sprite</param>
        /// <param name="x">X coordinate in maze.</param>
        /// <param name="y">Y coordinate in maze.</param>
        /// <param name="neighbors">Neighbors tiles that are linked with this tile</param>
        public Tile(Sprite sprite, int x, int y, bool collision, params Tile[] neighbors)
            : base(neighbors)
        {
            Sprite = sprite;
            X = x;
            Y = y;
            Collision = collision;
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        /// <summary>
        /// Add link with a neighbor tile.
        /// </summary>
        /// <param name="tile"></param>
        public void AddLink(Tile tile)
        {
            if (!tile.Collision)
                AddNeighbor(tile);
        }

    }
}
