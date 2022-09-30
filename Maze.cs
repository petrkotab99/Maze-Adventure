
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Gaming;
using Gaming.Files;
using Gaming.Effects;
using Gaming.Helpers;
using Gaming.Mechanics;
using Gaming.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Gaming.Input;
using System.Collections.ObjectModel;

namespace Maze_Adventure
{
    /// <summary>
    /// Represent a maze.
    /// </summary>
    class Maze : DrawableGameComponent
    {

        /// <summary>
        /// Entities that want to reach goal.
        /// </summary>
        List<Entity> entities = new List<Entity>();
        /// <summary>
        /// Tiles in maze
        /// </summary>
        Tile[,] tiles;
        /// <summary>
        /// Random number generator for maze generation.
        /// </summary>
        Random random = new Random();
        /// <summary>
        /// Sprite batch for drawing textures.
        /// </summary>
        SpriteBatch spriteBatch;
        /// <summary>
        /// Maze's grid
        /// </summary>
        Grid grid;
        /// <summary>
        /// Needed time for reaching destination of one tile lenght.
        /// </summary>
        float travelTime;
        float tilesWidth;
        float tilesHeight;
        Texture2D pathTextures;
        Texture2D wallTextures;
        Texture2D doorTexture;
        Tile goal;
        Tile spawn;
        Timer timer;
        float[] times = new float[2];
        Entity winner;

        public event VictoryEventHandler OnVictory;

        #region props

        public Tile ClickedTile { get; set; }
        public Input Input { get; private set; }

        public ReadOnlyCollection<Entity> Entities
        {
            get => entities.AsReadOnly();
        }

        /// <summary>
        /// Number of tiles in maze's height
        /// </summary>
        public int Height
        {
            get => tiles.GetLength(1);
        }

        /// <summary>
        /// Number of tiles in maze's width
        /// </summary>
        public int Width
        {
            get => tiles.GetLength(0);
        }

        /// <summary>
        /// Determine if grid is visible.
        /// </summary>
        public bool GridVisible { get; set; }

        /// <summary>
        /// Spawn tile
        /// </summary>
        public Tile Spawn
        {
            get => spawn;
            set
            {
                spawn = value;
                foreach (var entity in entities)
                {
                    entity.Teleport(spawn);
                }
            }
        }

        /// <summary>
        /// Goal tile
        /// </summary>
        public Tile Goal
        {
            get => goal;
            set
            {
                goal = value;
                goal.Sprite.Texture = doorTexture;
                goal.Sprite.Scale = new Vector2(tilesWidth / doorTexture.Width, tilesHeight / doorTexture.Height);
                goal.Collision = false;
            }
        }

        /// <summary>
        /// Width of one tile
        /// </summary>
        public float TileWidth
        {
            get
            {
                return Spawn.Sprite.Texture.Width * Spawn.Sprite.Scale.X;
            }
        }

        /// <summary>
        /// Height of one tile
        /// </summary>
        public float TileHeight
        {
            get
            {
                return Spawn.Sprite.Texture.Height * Spawn.Sprite.Scale.Y;
            }
        }

        /// <summary>
        /// Entities speed at x axis.
        /// </summary>
        public float SpeedX { get; private set; }

        /// <summary>
        /// Entities speed at y axis.
        /// </summary>
        public float SpeedY { get; private set; }

        public float AISpeedX { get; set; }
        public float AISpeedY { get; set; }

        /// <summary>
        /// Needed time for reaching destination of one tile lenght.
        /// </summary>
        public float TravelTime
        {
            get => travelTime;
            set
            {
                float distance = Spawn.Sprite.Texture.Width * Spawn.Sprite.Scale.X;
                SpeedX = distance / value;
                distance = Spawn.Sprite.Texture.Height * Spawn.Sprite.Scale.Y;
                SpeedY = distance / value;
                travelTime = value;
            }
        }

        #endregion

        /// <summary>
        /// Initialize new maze.
        /// </summary>
        /// <param name="rectangle">Destination rectangle</param>
        /// <param name="tilesTexture">Texture of tiles in maze</param>
        /// <param name="width">Number of tiles in maze's width</param>
        /// <param name="height">Number of tiles in maze's height</param>
        public Maze(Game game, Screen screen, Rectangle rectangle, int width, int height, Texture2D wallTextures, Texture2D pathTextures, Texture2D doorTexture, Timer timer)
        : base(game)
        {
            Input = game.Input;
            this.timer = timer;
            this.doorTexture = doorTexture;
            this.pathTextures = pathTextures;
            this.wallTextures = wallTextures;
            this.timer = timer;
            InitializeTiles(rectangle, width, height);
            Generate();
            RefreshLinks();

            grid = new Grid(width, height, rectangle, game.GraphicsDevice, 1f);
            TravelTime = 200f;

            spriteBatch = game.SpriteBatch;
            screen.AddComponent(this);
        }

        /// <summary>
        /// Initialize tiles in maze.
        /// </summary>
        /// <param name="rectangle">Destination rectangle</param>
        /// <param name="tilesTexture">Texture of tiles in maze</param>
        /// <param name="width">Number of tiles in maze's width</param>
        /// <param name="height">Number of tiles in maze's height</param>
        void InitializeTiles(Rectangle rectangle, int width, int height)
        {
            tilesWidth = rectangle.Width / (float)width;
            tilesHeight = rectangle.Height / (float)height;

            tiles = new Tile[width, height];
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    Texture2D texture = pathTextures[random.Next(0, pathTextures.Length)];
                    Sprite tileSprite = new Sprite(texture)
                    {
                        Color = Color.White,
                        Position = new Vector2(rectangle.X + i * tilesWidth, rectangle.Y + j * tilesHeight),
                        Scale = new Vector2(tilesWidth / texture.Width, tilesHeight / texture.Height),
                    };
                    tiles[i, j] = new Tile(tileSprite, i, j, false);
                }
            }
        }

        /// <summary>
        /// Generate maze's enviroment
        /// </summary>
        void Generate()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (i == 0 || j == 0 || i == tiles.GetLength(0) - 1 || j == tiles.GetLength(1) - 1)
                    {
                        SetWall(tiles[i, j]);
                        continue;
                    }
                    if (i % 2 == 0 && j % 2 == 0)
                    {
                        SetWall(tiles[i, j]);
                        switch (random.Next(0, 4))
                        {
                            case 0:
                                if (tiles[i - 1, j].Collision)
                                    goto case 1;
                                SetWall(tiles[i - 1, j]);
                                break;
                            case 1:
                                if (tiles[i, j - 1].Collision)
                                    goto case 2;
                                SetWall(tiles[i, j - 1]);
                                break;
                            case 2:
                                if (tiles[i + 1, j].Collision)
                                    goto case 3;
                                SetWall(tiles[i + 1, j]);
                                break;
                            case 3:
                                if (tiles[i, j + 1].Collision)
                                    goto case 0;
                                SetWall(tiles[i, j + 1]);
                                break;
                        }
                    }
                }
            }
            for (int i = 1; i < tiles.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < tiles.GetLength(1) - 1; j++)
                {
                    if (tiles[i + 1, j].Collision && tiles[i - 1, j].Collision && tiles[i, j + 1].Collision && tiles[i, j - 1].Collision && i % 2 == 1 && j % 2 == 1)
                        switch (random.Next(0, 4))
                        {
                            case 0:
                                SetPath(tiles[i - 1, j]);
                                break;
                            case 1:
                                SetPath(tiles[i - 1, j]);
                                break;
                            case 2:
                                SetPath(tiles[i - 1, j]);
                                break;
                            case 3:
                                SetPath(tiles[i - 1, j]);
                                break;
                        }
                }
            }
            Spawn = GetTile(1, random.Next(1, Height / 2) * 2 - 1);
            Goal = GetTile(Width - 1, random.Next(1, Height / 2) * 2 - 1);
        }

        /// <summary>
        /// Set tile to wall.
        /// </summary>
        /// <param name="tile">Tile that you wanna switch</param>
        void SetWall(Tile tile)
        {
            tile.Collision = true;
            tile.Sprite.Texture = wallTextures;
            tile.Sprite.Scale = new Vector2(tilesWidth / tile.Sprite.Texture.Width, tilesHeight / tile.Sprite.Texture.Height);
        }

        void SetPath(Tile tile)
        {
            tile.Collision = false;
            tile.Sprite.Texture = pathTextures;
            tile.Sprite.Scale = new Vector2(tilesWidth / tile.Sprite.Texture.Width, tilesHeight / tile.Sprite.Texture.Height);
        }

        /// <summary>
        /// Refresh all links for each tile.
        /// </summary>
        void RefreshLinks()
        {
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    tiles[i, j].Clear();
                    Tile tile;
                    if (i != 0)
                    {
                        tile = tiles[i - 1, j];
                        if (!tile.Collision)
                            tiles[i, j].AddLink(tile);
                    }
                    if (j != 0)
                    {
                        tile = tiles[i, j - 1];
                        if (!tile.Collision)
                            tiles[i, j].AddLink(tile);
                    }
                    if (i != tiles.GetLength(0) - 1)
                    {
                        tile = tiles[i + 1, j];
                        if (!tile.Collision)
                            tiles[i, j].AddLink(tile);
                    }
                    if (j != tiles.GetLength(1) - 1)
                    {
                        tile = tiles[i, j + 1];
                        if (!tile.Collision)
                            tiles[i, j].AddLink(tile);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var tile in tiles)
            {
                tile.Update(gameTime);
            }
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (var tile in tiles)
            {
                tile.Draw(spriteBatch);
            }
            foreach (var entity in entities)
            {
                entity.Draw(spriteBatch);
            }
            if (GridVisible)
                grid.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Return tile from maze's tiles array.
        /// </summary>
        /// <param name="x">1. dimension index</param>
        /// <param name="y">2. dimension index</param>
        /// <returns>Selected tiles in maze's tiles array</returns>
        public Tile GetTile(int x, int y)
        {
            return tiles[x, y];
        }

        /// <summary>
        /// Add entity to the maze.
        /// </summary>
        /// <param name="entity">Entity for adding</param>
        public void AddEntity(Entity entity)
        {
            if (!entities.Contains(entity))
                entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            if (entities.Contains(entity))
                entities.Remove(entity);
        }

        public void Reset()
        {
            foreach (var tile in tiles)
            {
                if (tile.Collision)
                    SetPath(tile);
            }
            Generate();
            RefreshLinks();
        }

        public void RechGoal(Entity entity)
        {
            if (!entities.Contains(entity))
                throw new Exception("Entitity is not in maze!");
            if (winner == null)
            {
                winner = entity;
                times[0] = timer.Time;
                entities.Remove(entity);
            }
            else
            {
                times[1] = timer.Time;
                OnVictory?.Invoke(this, new VictoryEventArgs(winner, times.ToArray()));
                winner = null;
            }
        }

        public void ClearWinner()
        {
            winner = null;
        }

    }
}
