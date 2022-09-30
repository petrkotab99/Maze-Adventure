using System;
using System.Linq;
using System.Collections.Generic;

using Gaming;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Maze_Adventure
{
    /// <summary>
    /// Represent a entity
    /// </summary>
    class Entity
    {
        /// <summary>
        /// Maze where entity is located
        /// </summary>
        protected Maze maze;
        /// <summary>
        /// Selected direction
        /// </summary>
        protected Direction goal;
        /// <summary>
        /// Contains future directions
        /// </summary>
        protected Queue<Direction> goals;
        /// <summary>
        /// Traveled distance from last direction change
        /// </summary>
        float walked = 0f;
        /// <summary>
        /// X coordinate
        /// </summary>
        protected int x;
        /// <summary>
        /// Y coordinate
        /// </summary>
        protected int y;
        /// <summary>
        /// Speed multiplier
        /// </summary>
        protected float speedSlow = 1f;

        /// <summary>
        /// Occure when entity change direction.
        /// </summary>
        public event MoveStartHandler MoveStart;

        /// <summary>
        /// entity's sprite
        /// </summary>
        public Sprite Sprite { get; private set; }

        /// <summary>
        /// Initialize new entity
        /// </summary>
        /// <param name="maze">Entity's location</param>
        /// <param name="sprite">Entity's sprite</param>
        public Entity(Maze maze, Sprite sprite)
        {
            this.maze = maze;
            Sprite = sprite;
            sprite.Position = maze.Spawn.Sprite.Position;
            x = maze.Spawn.X;
            y = maze.Spawn.Y;

            goal = Direction.None;
            goals = new Queue<Direction>();

            maze.AddEntity(this);
        }

        /// <summary>
        /// Move eneity to some direction.
        /// </summary>
        /// <param name="direction">Selected direction</param>
        public void Move(Direction direction)
        {
            goals.Enqueue(direction);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            if (goal == Direction.None && x == maze.Goal.X && y == maze.Goal.Y)
                maze.RechGoal(this);
            if (goal == Direction.None && goals.Count > 0)
            {
                goal = goals.Dequeue();
                MoveStart?.Invoke(this, new MoveStartArgs(goal));
                #region Verify
                switch (goal)
                {
                    case Direction.Up:
                        if (y == 0)
                        {
                            goal = Direction.None;
                            break;
                        }
                        if (maze.GetTile(x, y - 1).Collision)
                        {
                            goal = Direction.None;
                            break;
                        }
                        break;
                    case Direction.Left:
                        if (x == 0)
                        {
                            goal = Direction.None;
                            break;
                        }
                        if (maze.GetTile(x - 1, y).Collision)
                        {
                            goal = Direction.None;
                            break;
                        }
                        break;
                    case Direction.Down:
                        if (y == maze.Height - 1)
                        {
                            goal = Direction.None;
                            break;
                        }
                        if (maze.GetTile(x, y + 1).Collision)
                        {
                            goal = Direction.None;
                            break;
                        }
                        break;
                    case Direction.Right:
                        if (x == maze.Width - 1)
                        {
                            goal = Direction.None;
                            break;
                        }
                        if (maze.GetTile(x + 1, y).Collision)
                        {
                            goal = Direction.None;
                            break;
                        }
                        break;
                }
                #endregion
            }
            float distance;
            #region Walk
            switch (goal)
            {
                case Direction.Up:
                    if (walked >= maze.TileHeight)
                    {
                        y--;
                        Sprite.Position = maze.GetTile(x, y).Sprite.Position;
                        goal = Direction.None;
                        walked = 0;
                        return;
                    }
                    distance = maze.SpeedY * speedSlow * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    Sprite.Position = new Vector2(Sprite.Position.X, Sprite.Position.Y - distance);
                    walked += distance;
                    break;
                case Direction.Left:
                    if (walked >= maze.TileWidth)
                    {
                        x--;
                        Sprite.Position = maze.GetTile(x, y).Sprite.Position;
                        goal = Direction.None;
                        walked = 0;
                        return;
                    }
                    distance = maze.SpeedY * speedSlow * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    Sprite.Position = new Vector2(Sprite.Position.X - distance, Sprite.Position.Y);
                    walked += distance;
                    break;
                case Direction.Down:
                    if (walked >= maze.TileHeight)
                    {
                        y++;
                        Sprite.Position = maze.GetTile(x, y).Sprite.Position;
                        goal = Direction.None;
                        walked = 0;
                        return;
                    }
                    distance = maze.SpeedY * speedSlow * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    Sprite.Position = new Vector2(Sprite.Position.X, Sprite.Position.Y + distance);
                    walked += distance;
                    break;
                case Direction.Right:
                    if (walked >= maze.TileWidth)
                    {
                        x++;
                        Sprite.Position = maze.GetTile(x, y).Sprite.Position;
                        goal = Direction.None;
                        walked = 0;
                        return;
                    }
                    distance = maze.SpeedY * speedSlow * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    Sprite.Position = new Vector2(Sprite.Position.X + distance, Sprite.Position.Y);
                    walked += distance;
                    break;
            }
            #endregion
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        /// <summary>
        /// Teleport entity to the specified tile.
        /// </summary>
        public void Teleport(Tile tile)
        {
            x = tile.X;
            y = tile.Y;
            Sprite.Position = tile.Sprite.Position;
        }

        /// <summary>
        /// Teleport entity to maze's spawn.
        /// </summary>
        public void ReSpawn()
        {
            x = maze.Spawn.X;
            y = maze.Spawn.Y;
            Sprite.Position = maze.Spawn.Sprite.Position;
        }

    }
}
