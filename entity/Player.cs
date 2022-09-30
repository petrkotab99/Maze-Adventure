using System;
using System.Linq;

using Gaming;
using Gaming.Input;

using Microsoft.Xna.Framework.Input;


namespace Maze_Adventure
{
    /// <summary>
    /// Represent a entity with keyboard input.
    /// </summary>
    class Player : Entity
    {
        /// <summary>
        /// Key for moving up
        /// </summary>
        Key moveUpKey;
        /// <summary>
        /// Key for moving left
        /// </summary>
        Key moveLeftKey;
        /// <summary>
        /// Key for moving down
        /// </summary>
        Key moveDownKey;
        /// <summary>
        /// Key for moving right
        /// </summary>
        Key moveRightKey;
        Input input;

        /// <summary>
        /// Occure when some of key of player release.
        /// </summary>
        public event EventHandler OnRelease
        {
            add
            {
                moveUpKey.OnRelease += value;
                moveLeftKey.OnRelease += value;
                moveDownKey.OnRelease += value;
                moveRightKey.OnRelease += value;
            }
            remove
            {
                moveUpKey.OnRelease -= value;
                moveLeftKey.OnRelease -= value;
                moveDownKey.OnRelease -= value;
                moveRightKey.OnRelease -= value;
            }
        }

        /// <summary>
        /// Initialize new player.
        /// </summary>
        /// <param name="maze">Player's location</param>
        /// <param name="sprite">Player's sprite</param>
        public Player(Maze maze, Sprite sprite)
            : base(maze, sprite) { }

        /// <summary>
        /// Set input for player
        /// </summary>
        /// <param name="input">Input for handling keys</param>
        /// <param name="moveUp">Key for moving up</param>
        /// <param name="moveLeft">Key for moving left</param>
        /// <param name="moveDown">Key for moving down</param>
        /// <param name="moveRight">Key for moving right</param>
        public void SetInput(Input input, Keys moveUp, Keys moveLeft, Keys moveDown, Keys moveRight)
        {
            this.input = input;
            moveUpKey = new Key(input, false, this, moveUp);
            moveUpKey.OnPress += MoveUpKey_OnPress;
            moveLeftKey = new Key(input, false, this, moveLeft);
            moveLeftKey.OnPress += MoveLeftKey_OnPress;
            moveDownKey = new Key(input, false, this, moveDown);
            moveDownKey.OnPress += MoveDownKey_OnPress;
            moveRightKey = new Key(input, false, this, moveRight);
            moveRightKey.OnPress += MoveRightKey_OnPress;
        }

        private void MoveRightKey_OnPress(object sender, EventArgs e)
        {
            if (goals.Count == 0 && goal==Direction.None)
                Move(Direction.Right);
        }

        private void MoveDownKey_OnPress(object sender, EventArgs e)
        {
            if (goals.Count == 0 && goal == Direction.None)
                Move(Direction.Down);
        }

        private void MoveLeftKey_OnPress(object sender, EventArgs e)
        {
            if (goals.Count == 0 && goal == Direction.None)
                Move(Direction.Left);
        }

        private void MoveUpKey_OnPress(object sender, EventArgs e)
        {
            if (goals.Count == 0 && goal == Direction.None)
                Move(Direction.Up);
        }

    }
}
