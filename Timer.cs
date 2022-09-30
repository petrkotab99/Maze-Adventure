using System;
using System.Linq;

using Gaming;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Maze_Adventure
{
    /// <summary>
    /// Represent a timer component.
    /// </summary>
    class Timer : DrawableGameComponent
    {
        Game game;
        /// <summary>
        /// Timer's sprite
        /// </summary>
        TextSprite sprite;
        TextSprite timeTextSprite;

        /// <summary>
        /// Time on timer
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// Determine if timer is paused.
        /// </summary>
        public bool Pause { get; set; }

        /// <summary>
        /// Initialize new timer component.
        /// </summary>
        public Timer(Game game, Screen screen)
        : base(game)
        {
            this.game = game;

            screen.AddComponent(this);
        }

        public override void Initialize()
        {
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteFont jambo = game.Content.Load<SpriteFont>(@"Fonts\Jambo70");

            sprite = new TextSprite(jambo, string.Empty,
                new Vector2(88 * game.Graphics.PreferredBackBufferWidth / 120, game.Graphics.PreferredBackBufferHeight / 30 + jambo.MeasureString("A").Y),
                Color.Black, 0f, new Vector2(), new Vector2(1f), SpriteEffects.None);
            timeTextSprite = new TextSprite(jambo, "   Time: ",
                new Vector2(90 * game.Graphics.PreferredBackBufferWidth / 120, game.Graphics.PreferredBackBufferHeight / 30),
                new Color(68, 42, 15), 0f, new Vector2(), new Vector2(1f), SpriteEffects.None);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Pause)
            {
                Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                #region Text formating

                sprite.Text.Clear();
                string timeString = Math.Round(Time, 2).ToString("0.00").Replace(',', '.');
                int spaces = (10 - timeString.Length) / 2;
                for (int i = 0; i <= spaces + 1; i++)
                {
                    sprite.Text.Append(' ');
                }
                sprite.Text.Append(timeString);
                sprite.Text.Append('s');

                #endregion
            }

            sprite.Update(gameTime);
            timeTextSprite.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game.SpriteBatch.Begin();
            sprite.Draw(game.SpriteBatch);
            timeTextSprite.Draw(game.SpriteBatch);
            game.SpriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Restart timer counting.
        /// </summary>
        public void Restart()
        {
            Time = 0f;
        }

    }
}
