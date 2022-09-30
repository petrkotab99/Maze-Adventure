using System;
using System.Linq;

using Microsoft.Xna.Framework.Input;


namespace Gaming.Input
{
    /// <summary>
    /// Represent a keyboard's key
    /// </summary>
    public class Key
    {

        /// <summary>
        /// Keys that must be pressed when players want to do action
        /// </summary>
        public Keys[] Keys { get; private set; }

        /// <summary>
        /// Determine if holding is enabled
        /// </summary>
        public bool LastKey { get; private set; }

        public object Sender { get; private set; }

        /// <summary>
        /// Happen when keys are pressed
        /// </summary>
        public event EventHandler OnPress;

        /// <summary>
        /// Happen when keys are released
        /// </summary>
        public event EventHandler OnRelease;


        /// <summary>
        /// Initialize new key
        /// </summary>
        /// <param name="lastKey">Determine if holding is enabled</param>
        /// <param name="keys">Keys that must be pressed when players want to do action</param>
        public Key(bool lastKey, object sender, params Keys[] keys)
        {
            Keys = keys;
            LastKey = lastKey;
            Sender = sender;
        }

        /// <summary>
        /// Initialize new key
        /// </summary>
        /// <param name="input">Input for handling key</param>
        /// <param name="lastKey">Determine if holding is enabled</param>
        /// <param name="keys">Keys that must be pressed when player want to do action</param>
        public Key(Input input ,bool lastKey, object sender, params Keys[] keys)
        {
            Keys = keys;
            LastKey = lastKey;
            Sender = sender;
            input.AddKey(this);
        }

        internal void Update(KeyboardState state, KeyboardState lastState)
        {
            CheckPress(state, lastState);
            CheckRelease(state, lastState);
        }

        /// <summary>
        /// Check if keys are pressed
        /// </summary>
        internal void CheckPress(KeyboardState state, KeyboardState lastState)
        {
            if (LastKey)
            {
                foreach (var key in Keys)
                {
                    if (!(lastState.IsKeyUp(key) && state.IsKeyDown(key)))
                        return;
                }
                OnPress?.Invoke(Sender, new EventArgs());
            }
            else
            {
                foreach (var key in Keys)
                {
                    if (!state.IsKeyDown(key))
                        return;
                }
                OnPress?.Invoke(Sender, new EventArgs());
            }
        }

        /// <summary>
        /// Check if keys are released
        /// </summary>
        internal void CheckRelease(KeyboardState state, KeyboardState last)
        {
            foreach (var key in Keys)
            {
                if (!(state.IsKeyUp(key) && last.IsKeyDown(key)))
                    return;
            }
            OnRelease?.Invoke(Sender, new EventArgs());
        }

    }
}
