namespace Maze_Adventure
{
    /// <summary>
    /// Handler for VictoryEvent
    /// </summary>
    delegate void VictoryEventHandler(object sender, VictoryEventArgs e);

    /// <summary>
    /// Represent a arguments for VictoryEvents
    /// </summary>
    class VictoryEventArgs
    {

        /// <summary>
        /// Winner of the race
        /// </summary>
        public Entity Winner { get; private set; }

        /// <summary>
        /// Times of participants
        /// </summary>
        public float[] Times { get; private set; }

        /// <summary>
        /// Initialize new victory arguments.
        /// </summary>
        /// <param name="winner">Winner of the race</param>
        /// <param name="times">Times of participants</param>
        public VictoryEventArgs(Entity winner, float[] times)
        {
            Winner = winner;
            Times = times;
        }

    }
}
