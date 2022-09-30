using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Gaming.Input
{
    /// <summary>
    /// Represent a clickable destination for mouse
    /// </summary>
    public class ClickDestination
    {
        /// <summary>
        /// Clickable destination
        /// </summary>
        public Rectangle Destination { get; private set; }
        /// <summary>
        /// Determine holding
        /// </summary>
        public bool LastState { get; private set; }

        /// <summary>
        /// Sender of the clickable destination
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Occure when you click with left button in destination
        /// </summary>
        public event MouseEventHandler OnLeftPress;
        /// <summary>
        /// Occure when you click witrh right button in destination
        /// </summary>
        public event MouseEventHandler OnRightPress;
        /// <summary>
        /// Occure when you click with middle button in destination
        /// </summary>
        public event MouseEventHandler OnMiddlePress;
        /// <summary>
        /// Occure when mouse are hoover in destination
        /// </summary>
        public event MouseEventHandler OnHoover;
        /// <summary>
        /// Occure when mouse enter destination
        /// </summary>
        public event MouseEventHandler OnEnter;
        /// <summary>
        /// Occure when mouse leave destination
        /// </summary>
        public event MouseEventHandler OnLeave;

        /// <summary>
        /// Initialize new ClickDestination
        /// </summary>
        /// <param name="lastState">last mouse state</param>
        /// <param name="destination">clickable destination</param>
        /// <param name="sender">destination sender</param>
        public ClickDestination(bool lastState, Rectangle destination, object sender)
        {
            LastState = lastState;
            Destination = destination;
            Sender = sender;
        }

        /// <summary>
        /// Initialize new ClickDestination
        /// </summary>
        /// <param name="input">Input for handling click destination</param>
        /// <param name="lastState">last mouse state</param>
        /// <param name="destination">clickable destination</param>
        /// <param name="sender">destination sender</param>
        public ClickDestination(Input input, bool lastState, Rectangle destination, object sender)
        {
            LastState = lastState;
            Destination = destination;
            Sender = sender;
            input.AddClickDestination(this);
        }

        public void Update(MouseState state, MouseState lastState)
        {
            if (!Destination.Contains(state.Position))
            {
                if (Destination.Contains(lastState.Position))
                    OnLeave?.Invoke(Sender, new MouseEventArgs(state.Position));
                return;
            }
            OnHoover?.Invoke(Sender, new MouseEventArgs(state.Position));
            if (!Destination.Contains(lastState.Position))
            {
                OnEnter?.Invoke(Sender, new MouseEventArgs(state.Position));
                CheckPress(state);
            }
            else
                CheckPress(state, lastState);

        }

        /// <summary>
        /// Check for pressed keys
        /// </summary>
        /// <param name="state">current mouse state</param>
        /// <param name="lastState">last mouse state</param>
        private void CheckPress(MouseState state, MouseState lastState)
        {
            if (LastState)
            {
                if (state.LeftButton == ButtonState.Pressed && lastState.LeftButton == ButtonState.Released)
                    OnLeftPress?.Invoke(Sender, new MouseEventArgs(state.Position));
                if (state.RightButton == ButtonState.Pressed && lastState.RightButton == ButtonState.Released)
                    OnRightPress?.Invoke(Sender, new MouseEventArgs(state.Position));
                if (state.MiddleButton == ButtonState.Pressed && lastState.MiddleButton == ButtonState.Released)
                    OnMiddlePress?.Invoke(Sender, new MouseEventArgs(state.Position));
            }
            else
            {
                if (state.LeftButton == ButtonState.Pressed)
                    OnLeftPress?.Invoke(Sender, new MouseEventArgs(state.Position));
                if (state.RightButton == ButtonState.Pressed)
                    OnRightPress?.Invoke(Sender, new MouseEventArgs(state.Position));
                if (state.MiddleButton == ButtonState.Pressed)
                    OnMiddlePress?.Invoke(Sender, new MouseEventArgs(state.Position));
            }
        }

        /// <summary>
        /// Check for pressed keys
        /// </summary>
        /// <param name="state">current mouse state</param>
        private void CheckPress(MouseState state)
        {
            if (state.LeftButton == ButtonState.Pressed)
                OnLeftPress?.Invoke(Sender, new MouseEventArgs(state.Position));
            if (state.RightButton == ButtonState.Pressed)
                OnRightPress?.Invoke(Sender, new MouseEventArgs(state.Position));
            if (state.MiddleButton == ButtonState.Pressed)
                OnMiddlePress?.Invoke(Sender, new MouseEventArgs(state.Position));
        }

    }
}
