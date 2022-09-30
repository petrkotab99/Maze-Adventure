namespace Maze_Adventure
{
    /// <summary>
    /// Handler for MoveStartEvent
    /// </summary>
    delegate void MoveStartHandler(object sender, MoveStartArgs e);

    /// <summary>
    /// Represent arguments for MoveStartEvent
    /// </summary>
    class MoveStartArgs
    {
        /// <summary>
        /// Selected direction
        /// </summary>
        public Direction Direction { get; private set; }

        /// <summary>
        /// Initialize new arguments for MoveStartEvent.
        /// </summary>
        /// <param name="direction">Selected direction</param>
        public MoveStartArgs(Direction direction)
        {
            Direction = direction;
        }

    }
}
