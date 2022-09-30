using Gaming;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Adventure
{
    /// <summary>
    /// Represent a gameMode component.
    /// </summary>
    class GameModeComponent : DrawableGameComponent
    {
        Game game;

        /// <summary>
        /// Sprite for gameMode Text
        /// </summary>
        TextSprite gameModeText;
        /// <summary>
        /// Sprite for gameMode information
        /// </summary>
        TextSprite gameModeInfo;

        /// <summary>
        /// Current game mode
        /// </summary>
        public GameMode GameMode { get; private set; }

        /// <summary>
        /// Current Difficulty
        /// </summary>
        public Difficulty Difficulty { get; set; }

        /// <summary>
        /// Initialize new game mode component.
        /// </summary>
        public GameModeComponent(Game game, Screen screen) 
            : base(game)
        {
            this.game = game;
            GameMode = GameMode.PvP;

            screen.AddComponent(this);
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteFont jambo=game.Content.Load<SpriteFont>(@"Fonts\Jambo70");

            gameModeText = new TextSprite(jambo, "GameMode: ",
                new Vector2(90 * game.Graphics.PreferredBackBufferWidth / 120, game.Graphics.PreferredBackBufferHeight / 30 + jambo.MeasureString("A").Y * 3),
                new Color(68, 42, 15), 0f, new Vector2(), new Vector2(1f), SpriteEffects.None);
            gameModeInfo = new TextSprite(jambo, "",
                new Vector2(90 * game.Graphics.PreferredBackBufferWidth / 120, game.Graphics.PreferredBackBufferHeight / 30 + jambo.MeasureString("A").Y * 4),
                Color.Black, 0f, new Vector2(), new Vector2(1f), SpriteEffects.None);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            gameModeText.Update(gameTime);
            gameModeInfo.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            game.SpriteBatch.Begin();
            gameModeText.Draw(game.SpriteBatch);
            gameModeInfo.Draw(game.SpriteBatch);
            game.SpriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Switch game mode.
        /// </summary>
        public void SwitchMode(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.PvP:
                    #region Text formating

                    gameModeInfo.Text.Clear();
                    gameModeInfo.Text.Append("   Player");
                    gameModeInfo.Text.Append(Environment.NewLine);
                    gameModeInfo.Text.Append("      vs");
                    gameModeInfo.Text.Append(Environment.NewLine);
                    gameModeInfo.Text.Append("   Player");

                    #endregion
                    break;
                case GameMode.PvE:
                    #region Text formating

                    gameModeInfo.Text.Clear();
                    gameModeInfo.Text.Append("   Player");
                    gameModeInfo.Text.Append(Environment.NewLine);
                    gameModeInfo.Text.Append("      vs");
                    gameModeInfo.Text.Append(Environment.NewLine);
                    gameModeInfo.Text.Append(" Computer");
                    gameModeInfo.Text.Append(Environment.NewLine);
                    string difString = Difficulty.ToString().Split('.').Last();
                    int spaces = (10 - difString.Length) / 2;
                    for (int i = 0; i <= spaces; i++)
                    {
                        gameModeInfo.Text.Append(' ');
                    }
                    gameModeInfo.Text.Append("(");
                    gameModeInfo.Text.Append(difString);
                    gameModeInfo.Text.Append(')');

                    #endregion
                    break;
            }
        }

    }
}
