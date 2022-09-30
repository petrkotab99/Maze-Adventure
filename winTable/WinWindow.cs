using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Gaming;
using Gaming.Files;
using Gaming.Effects;
using Gaming.Helpers;
using Gaming.Network;
using Gaming.Mechanics;
using Gaming.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Maze_Adventure
{
    /// <summary>
    /// Represent a window with information about race's winner.
    /// </summary>
    class WinWindow : DrawableGameComponent
    {
        /// <summary>
        /// Window's sprite
        /// </summary>
        Sprite sprite;
        /// <summary>
        /// Cups sprites
        /// </summary>
        Sprite cup1, cup2;
        /// <summary>
        /// Winner sprite
        /// </summary>
        Sprite winner;
        /// <summary>
        /// "winner:" sprite
        /// </summary>
        TextSprite winnerText;
        /// <summary>
        /// 1st and 2nd times titles
        /// </summary>
        TextSprite time1Text, time2Text;
        /// <summary>
        /// Times sprites
        /// </summary>
        TextSprite time1, time2;
        Game game;

        /// <summary>
        /// Destination rectamgle
        /// </summary>
        public Rectangle Rectangle { get; set; }

        /// <summary>
        /// Initialize new winning window component.
        /// </summary>
        public WinWindow(Game game, Screen screen)
        : base(game)
        {
            this.game = game;
            Rectangle = new Rectangle((int)(9 * game.Graphics.PreferredBackBufferWidth / 28f), (int)(game.Graphics.PreferredBackBufferHeight / 3f),
                (int)(game.Graphics.PreferredBackBufferWidth / 3f), (int)(game.Graphics.PreferredBackBufferHeight / 3f));

            screen.AddComponent(this);
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            #region Fonts & textures

            Texture2D windowTexture = game.Content.Load<Texture2D>(@"Pictures\WinWindow\window");
            Texture2D cup1Texture= game.Content.Load<Texture2D>(@"Pictures\WinWindow\cup1");
            Texture2D cup2Texture = game.Content.Load<Texture2D>(@"Pictures\WinWindow\cup2");
            Texture2D lazyTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            SpriteFont jambo = game.Content.Load<SpriteFont>(@"Fonts\Jambo40");

            #endregion
            #region Sprites initializing

            sprite = new Sprite(windowTexture, new Vector2(Rectangle.X, Rectangle.Y), null, Color.White, 0f, new Vector2(),
                new Vector2(Rectangle.Width / (float)windowTexture.Width, Rectangle.Height / (float)windowTexture.Height), SpriteEffects.None);
            cup1 = new Sprite(cup1Texture, new Vector2(Rectangle.Width / 10f + Rectangle.X, Rectangle.Height / 8f + Rectangle.Y), null, Color.White, 0f, new Vector2(),
                new Vector2((Rectangle.Width / 8f) / cup1Texture.Width, (Rectangle.Height / 6f) / cup1Texture.Height), SpriteEffects.None);
            cup2 = new Sprite(cup2Texture, new Vector2(Rectangle.Width - Rectangle.Width / 4f + Rectangle.X, Rectangle.Height / 8f + Rectangle.Y), null, Color.White, 0f, new Vector2(),
                new Vector2((Rectangle.Width / 8f) / cup2Texture.Width, (Rectangle.Height / 6f) / cup2Texture.Height), SpriteEffects.None);
            winnerText = new TextSprite(jambo, "Winner: ", new Vector2(Rectangle.Width / 4f + Rectangle.X, Rectangle.Height / 8f + Rectangle.Y), Color.Gold, 0f, new Vector2(),
                new Vector2(1f), SpriteEffects.None);
            time1Text = new TextSprite(jambo, "1st ", new Vector2(Rectangle.Width / 8f + Rectangle.X, 3 * Rectangle.Height / 8f + Rectangle.Y), Color.Gold, 0f, new Vector2(),
                new Vector2(1f), SpriteEffects.None);
            time2Text = new TextSprite(jambo, "2nd ", new Vector2(Rectangle.Width / 8f + Rectangle.X, 5 * Rectangle.Height / 8f + Rectangle.Y), Color.Silver, 0f, new Vector2(),
                new Vector2(1f), SpriteEffects.None);
            time1 = new TextSprite(jambo, string.Empty, new Vector2(Rectangle.Width / 8f + Rectangle.X + jambo.MeasureString("1st ").X, 3 * Rectangle.Height / 8f + Rectangle.Y),
                Color.Gold, 0f, new Vector2(), new Vector2(1f), SpriteEffects.None);
            time2 = new TextSprite(jambo, string.Empty, new Vector2(Rectangle.Width / 8f + Rectangle.X + jambo.MeasureString("2st ").X, 5 * Rectangle.Height / 8f + Rectangle.Y),
                Color.Silver, 0f, new Vector2(), new Vector2(1f), SpriteEffects.None);
            winner = new Sprite(lazyTexture, new Vector2(23 * Rectangle.Width / 40f + Rectangle.X, Rectangle.Height / 20f + Rectangle.Y), null, Color.White,
                0f,new Vector2(), new Vector2(1f), SpriteEffects.None);
            #endregion

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            cup1.Update(gameTime);
            cup2.Update(gameTime);
            winner.Update(gameTime);
            winnerText.Update(gameTime);
            time1Text.Update(gameTime);
            time2Text.Update(gameTime);
            time1.Update(gameTime);
            time2.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game.SpriteBatch.Begin();
            sprite.Draw(game.SpriteBatch);
            cup1.Draw(game.SpriteBatch);
            cup2.Draw(game.SpriteBatch);
            winner.Draw(game.SpriteBatch);
            winnerText.Draw(game.SpriteBatch);
            time1Text.Draw(game.SpriteBatch);
            time2Text.Draw(game.SpriteBatch);
            time1.Draw(game.SpriteBatch);
            time2.Draw(game.SpriteBatch);
            game.SpriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Set the winner of the race and show him.
        /// </summary>
        /// <param name="winner">Race winner</param>
        /// <param name="time1">Time of winner</param>
        /// <param name="time2">Time of 2nd participiant</param>
        public void SetWinner(Sprite winner, float time1, float time2)
        {
            this.winner.Texture = winner.Texture;
            this.winner.Scale = new Vector2(Rectangle.Width/400f, Rectangle.Width/400f);
            //if (!winner.ContainEffect("animation"))
            {
                winner.TryGetEffect("animation", out ISpriteEffect animation);
                this.winner.AddEffect("animation", animation);
            }
            this.winner.SourceRectangle = new Rectangle(0, 2 * 64, 64, 64);
            this.time1.Text.Clear();
            this.time1.Text.Append(time1.ToString("0.00") + 's');
            this.time2.Text.Clear();
            this.time2.Text.Append(time2.ToString("0.00") + 's');
        }

    }
}
