using System;
using System.Linq;

using Gaming;
using Gaming.Algorithms.PathFinding;


namespace Maze_Adventure
{
    /// <summary>
    /// Represent a entity that can go throught entire maze.
    /// </summary>
    class AI : Entity
    {
        /// <summary>
        /// Alghoritm for patch finding
        /// </summary>
        BreadthFirstSearch breadthFirstSearch = new BreadthFirstSearch();

        /// <summary>
        /// Speed multiplier
        /// </summary>
        public float SpeedMultiplier
        {
            get => speedSlow;
            set { speedSlow = value; }
        }

        /// <summary>
        /// Initialize new AI
        /// </summary>
        /// <param name="maze">AI's maze</param>
        /// <param name="sprite">AI's sprite</param>
        public AI(Maze maze, Sprite sprite)
            : base(maze, sprite)
        {
            speedSlow = 0.5f;
        }

        /// <summary>
        /// Find path and go to goal.
        /// </summary>
        public void Go()
        {
            goals.Clear();
            goal = Direction.None;
            breadthFirstSearch.Start = maze.GetTile(x, y);
            breadthFirstSearch.Goal = maze.Goal;
            breadthFirstSearch.FindPath();
            Tile last = maze.GetTile(x, y);
            foreach (var tile in breadthFirstSearch.Path)
            {
                if ((Tile)tile == maze.GetTile(x, y))
                    continue;
                if (((Tile)tile).X > last.X)
                    Move(Direction.Right);
                else if (((Tile)tile).X < last.X)
                    Move(Direction.Left);
                else if (((Tile)tile).Y > last.Y)
                    Move(Direction.Down);
                else
                    Move(Direction.Up);
                last = (Tile)tile;
            }
        }


    }
}
