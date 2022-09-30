using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Gaming.Input
{
    /// <summary>
    /// Represent a handler for input
    /// </summary>
    public class Input : GameComponent
    {

        /// <summary>
        /// Current keyboard state
        /// </summary>
        KeyboardState keyboardState = new KeyboardState();
        /// <summary>
        /// Last keyboard state
        /// </summary>
        KeyboardState lastKeyboardState;
        /// <summary>
        /// Current mouse state
        /// </summary>
        MouseState mouseState = new MouseState();
        /// <summary>
        /// Last mouse state
        /// </summary>
        MouseState lastMouseState;
        /// <summary>
        /// List of all keys.
        /// </summary>
        List<Key> keys = new List<Key>();
        /// <summary>
        /// List of all clickdestinations
        /// </summary>
        List<ClickDestination> clickDestinations = new List<ClickDestination>();

        /// <summary>
        /// Initialize new input
        /// </summary>
        public Input(Game game) 
            : base(game)
        {
            game.Components.Add(this);
            keyboardState = new KeyboardState();
        }

        public override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();

            foreach (var key in keys)
                key.Update(keyboardState, lastKeyboardState);
            foreach (var clickDestination in clickDestinations)
                clickDestination.Update(mouseState, lastMouseState);

            base.Update(gameTime);
        }

        /// <summary>
        /// Add key for handling
        /// </summary>
        /// <param name="key">Key that you wanna add</param>
        public void AddKey(Key key)
        {
            keys.Add(key);
        }

        /// <summary>
        /// Remove key from handling
        /// </summary>
        /// <param name="key">Key that you wanna remove</param>
        public void RemoveKey(Key key)
        {
            keys.Remove(key);
        }

        /// <summary>
        /// Add clickdestination
        /// </summary>
        /// <param name="clickDestination">Clickdestination that you wanna add</param>
        public void AddClickDestination(ClickDestination clickDestination)
        {
            clickDestinations.Add(clickDestination);
        }

        /// <summary>
        /// Remove clickdestination
        /// </summary>
        /// <param name="clickDestination">Clickdestination that you wanna remove</param>
        public void RemoveClickDestination(ClickDestination clickDestination)
        {
            clickDestinations.Remove(clickDestination);
        }

        /// <summary>
        /// Return actual mouse position
        /// </summary>
        /// <returns>Actual mouse position</returns>
        public Point GetMousePosition()
        {
            return mouseState.Position;
        }

    }
}
