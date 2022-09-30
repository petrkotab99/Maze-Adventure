using System;
using System.Linq;

using Gaming;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Maze_Adventure
{
    /// <summary>
    /// Represent a counter component.
    /// </summary>
    class Counter : DrawableGameComponent
    {

        Game game;
        /// <summary>
        /// Sprite for number
        /// </summary>
        TextSprite numberSprite;
        /// <summary>
        /// Sprite for 1st half of string
        /// </summary>
        TextSprite str1;
        /// <summary>
        /// Sprite for 2nd part of string
        /// </summary>
        TextSprite str2;
        /// <summary>
        /// Number the is showed in the middle of string
        /// </summary>
        float number = 3f;
        /// <summary>
        /// Displayed string
        /// </summary>
        public string Str { get; set; }

        /// <summary>
        /// Initialize new counter component
        /// </summary>
        public Counter(Game game, Screen screen)
        : base(game)
        {
            this.game = game;
            Str = "===";

            screen.AddComponent(this);
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteFont jambo = game.Content.Load<SpriteFont>(@"Fonts\Jambo70");
            numberSprite = new TextSprite(jambo, "3",
                new Vector2(game.Graphics.PreferredBackBufferWidth * 29 / 60f, game.Graphics.PreferredBackBufferHeight * 6 / 8f),
                new Color(215, 155, 45), 0f, new Vector2(), new Vector2(1f), SpriteEffects.None);
            str1= new TextSprite(jambo, Str,
                new Vector2(game.Graphics.PreferredBackBufferWidth * 29 / 60f - jambo.MeasureString(Str).X, game.Graphics.PreferredBackBufferHeight * 6 / 8f),
                new Color(215, 155, 45), 0f, new Vector2(), new Vector2(1f), SpriteEffects.None);
            str2= new TextSprite(jambo, Str,
                new Vector2(game.Graphics.PreferredBackBufferWidth * 29 / 60f + jambo.MeasureString("3").X, game.Graphics.PreferredBackBufferHeight * 6 / 8f),
                new Color(215, 155, 45), 0f, new Vector2(), new Vector2(1f), SpriteEffects.None);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            number -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (number <= 0)
            {
                number += 3f;
                Visible = false;
                Enabled = false;
                game.BackToMaze();
            }
            numberSprite.Text.Clear();
            numberSprite.Text.Append(Math.Round(number));

            numberSprite.Update(gameTime);
            str1.Update(gameTime);
            str2.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game.SpriteBatch.Begin();
            numberSprite.Draw(game.SpriteBatch);
            str1.Draw(game.SpriteBatch);
            str2.Draw(game.SpriteBatch);
            game.SpriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
