using System;
using System.Linq;

using Gaming;
using Gaming.Input;
using Gaming.Effects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using GameBase = Microsoft.Xna.Framework.Game;


namespace Maze_Adventure
{
    class Game : GameBase
    {
        #region Props
        public GraphicsDeviceManager Graphics { get; set; }
        public ScreenManager ScreenManager { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public Input Input { get; set; }

        public Rectangle ScreenRectangle
        {
            get => new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
        }
        #endregion

        #region Sprites & textures

        Texture2D background;

        Sprite entity1Sprite;
        Sprite entity2Sprite;
        Sprite boatSprite;

        #endregion
        #region Components

        Maze maze;

        WinWindow winWindow;
        Counter counter;
        Timer timer;
        GameModeComponent gameMode;

        #endregion
        #region Input

        Key exitKey;
        Key fullscreenKey;
        Key gridKey;
        Key switchModeKey;
        Key easyKey;
        Key normalKey;
        Key hardKey;
        Key imposibleKey;

        #endregion

        Screen levelScreen;

        Player player1;
        Player player2;
        AI ai;

        bool restart = false;

        public Game()
        {
            Graphics = new GraphicsDeviceManager(this);
            ScreenManager = new ScreenManager();
            Input = new Input(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Window.Title = "Maze Adventure";
            IsMouseVisible = true;
            #region Graphics

            Graphics.PreferredBackBufferWidth = 1920;
            Graphics.PreferredBackBufferHeight = 1080;
            Graphics.IsFullScreen = false;
            Graphics.ApplyChanges();

            #endregion;
            #region Input

            exitKey = new Key(Input, true, this, Keys.Escape);
            fullscreenKey = new Key(Input, true, this, Keys.F11);
            gridKey = new Key(Input, true, maze, Keys.F2);
            switchModeKey = new Key(Input, true, this, Keys.Space);
            easyKey = new Key(Input, true, this, Keys.NumPad1);
            normalKey = new Key(Input, true, this, Keys.NumPad2);
            hardKey = new Key(Input, true, this, Keys.NumPad3);
            imposibleKey = new Key(Input, true, this, Keys.NumPad4);

            exitKey.OnPress += ExitGame;
            fullscreenKey.OnPress += FullScreen;
            gridKey.OnPress += GridKey_OnPress;
            switchModeKey.OnPress += SwitchModeKey_OnPress;
            easyKey.OnPress += EasyKey_OnPress;
            normalKey.OnPress += NormalKey_OnPress;
            hardKey.OnPress += HardKey_OnPress;
            imposibleKey.OnPress += ImposibleKey_OnPress;

            #endregion

            levelScreen = new Screen(this, ScreenManager, true);

            winWindow = new WinWindow(this, levelScreen);
            counter = new Counter(this, levelScreen);
            timer = new Timer(this, levelScreen);
            gameMode = new GameModeComponent(this, levelScreen);

            base.Initialize();
        }

        

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            #region Sprites & textures initializing

            background = Content.Load<Texture2D>(@"Pictures\background");
            Texture2D wallTexture = Content.Load<Texture2D>(@"Pictures\ground");
            Texture2D pathTexture = Content.Load<Texture2D>(@"Pictures\stonePath");

            SpriteFont jambo = Content.Load<SpriteFont>(@"Fonts\Jambo70");

            boatSprite = new Sprite(Content.Load<Texture2D>(@"Pictures\boat"),
                new Vector2(Graphics.PreferredBackBufferWidth * 37 / 40f, Graphics.PreferredBackBufferHeight * 34 / 40f),
                null, new Color(200, 200, 200), 0f, new Vector2(), new Vector2(0.2f), SpriteEffects.FlipHorizontally);

            #endregion
            #region Maze initializing

            Rectangle mazeRectangle = new Rectangle(Graphics.PreferredBackBufferWidth / 60, Graphics.PreferredBackBufferHeight / 30,
                29 * Graphics.PreferredBackBufferWidth / 40, 14 * Graphics.PreferredBackBufferHeight / 15);
            maze = new Maze(this, levelScreen, mazeRectangle, 33, 21, wallTexture, pathTexture, Content.Load<Texture2D>(@"Pictures\Entrace"), timer);
            maze.OnVictory += Maze_OnVictory;

            #endregion
            #region Players Initializing
            entity1Sprite = new Sprite(Content.Load<Texture2D>(@"Pictures\male1_walk"))
            {
                Color = Color.White,
                SourceRectangle = new Rectangle(0, 2 * 64, 64, 64),
                Scale = new Vector2(maze.Spawn.Sprite.Texture.Width * maze.Spawn.Sprite.Scale.X / 64, maze.Spawn.Sprite.Texture.Height * maze.Spawn.Sprite.Scale.Y / 64),
            };
            entity1Sprite.AddEffect("animation", new Animation(8, 4, 64, 64, 0, 2, 100f));
            entity2Sprite = new Sprite(Content.Load<Texture2D>(@"Pictures\female1_walk"))
            {
                Color = Color.White,
                SourceRectangle = new Rectangle(0, 2 * 64, 64, 64),
                Scale = new Vector2(maze.Spawn.Sprite.Texture.Width * maze.Spawn.Sprite.Scale.X / 64, maze.Spawn.Sprite.Texture.Height * maze.Spawn.Sprite.Scale.Y / 64),
            };
            entity2Sprite.AddEffect("animation", new Animation(8, 4, 64, 64, 0, 2, 100f));
            player1 = new Player(maze, entity1Sprite);
            player1.SetInput(Input, Keys.W, Keys.A, Keys.S, Keys.D);
            player1.MoveStart += Player_MoveStart;
            player2 = new Player(maze, entity2Sprite);
            player2.SetInput(Input, Keys.Up, Keys.Left, Keys.Down, Keys.Right);
            player2.MoveStart += Player_MoveStart;
            ai = new AI(maze, entity2Sprite);
            ai.MoveStart += Player_MoveStart;
            SwitchMode(gm);
            #endregion

            ScreenManager.SwitchScreen(levelScreen);
            winWindow.Visible = false;
            winWindow.Enabled = false;
            counter.Visible = false;
            counter.Enabled = false;

            base.LoadContent();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (restart)
            {
                SwitchMode(gameMode.GameMode);
                restart = false;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);
            SpriteBatch.Begin();
            SpriteBatch.Draw(background, ScreenRectangle, Color.White);
            boatSprite.Draw(SpriteBatch);
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void BackToMaze()
        {
            if (!winWindow.Enabled)
                return;

            timevisible = true;
            maze.Visible = true;
            maze.Enabled = true;
            timer.Visible = true;
            timer.Enabled = true;

            winWindow.Enabled = false;
            winWindow.Visible = false;

            maze.Reset();
            maze.AddEntity(player1);
            timer.Restart();
            restart = true;
        }

        #region Switchs

        void SwitchMode(GameMode gameMode)
        {
            timer.Restart();
            maze.ClearWinner();
            maze.AddEntity(player1);
            switch (gameMode)
            {
                case GameMode.PvP:
                    if (maze.Entities.Contains(ai))
                        maze.RemoveEntity(ai);
                    maze.AddEntity(player2);
                    player1.ReSpawn();
                    player2.ReSpawn();

                    this.gameMode.SwitchMode(gameMode);
                    break;
                case GameMode.PvE:
                    if (maze.Entities.Contains(player2))
                        maze.RemoveEntity(player2);
                    maze.AddEntity(ai);
                    player1.ReSpawn();
                    ai.ReSpawn();
                    this.gameMode.SwitchMode(gameMode);
                    ai.Go();
                    break;
            }
        }

        void SwitchDifficulty(Difficulty difficulty)
        {
            if (gameMode.GameMode == GameMode.PvP)
                return;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    ai.SpeedMultiplier = 0.8f;
                    break;
                case Difficulty.Normal:
                    ai.SpeedMultiplier = 0.9f;
                    break;
                case Difficulty.Hard:
                    ai.SpeedMultiplier = 0.99f;
                    break;
                case Difficulty.Imposible:
                    ai.SpeedMultiplier = 1f;
                    break;
            }

            gameMode.Difficulty = difficulty;
            SwitchMode(gameMode.GameMode);
        }
        #endregion
        #region Events

        private void FullScreen(object sender, EventArgs e)
        {
            Graphics.IsFullScreen = !Graphics.IsFullScreen;
            Graphics.ApplyChanges();
        }

        private void ExitGame(object sender, EventArgs e)
        {
            Exit();
        }

        private void GridKey_OnPress(object sender, EventArgs e)
        {
            maze.GridVisible = !maze.GridVisible;
        }

        private void Player_MoveStart(object sender, MoveStartArgs e)
        {
            ((Entity)sender).Sprite.TryGetEffect("animation", out ISpriteEffect animation);
            ((Animation)animation).Y = (int)e.Direction - 1;
        }


        bool timevisible = true;
        private void Maze_OnVictory(object sender, VictoryEventArgs e)
        {
            maze.Visible = false;
            maze.Enabled = false;
            winWindow.Enabled = true;
            winWindow.Visible = true;
            counter.Enabled = true;
            counter.Visible = true;
            winWindow.SetWinner(e.Winner.Sprite, e.Times[0], e.Times[1]);
            timer.Visible = false;
            timer.Enabled = false;
            timevisible = false;
        }

        private void SwitchModeKey_OnPress(object sender, EventArgs e)
        {
            if (gm == GameMode.PvE)
                gm = GameMode.PvP;
            else
                gm = GameMode.PvE;
            restart = true;
        }
        private void HardKey_OnPress(object sender, EventArgs e)
        {
            SwitchDifficulty(Difficulty.Hard);
        }

        private void NormalKey_OnPress(object sender, EventArgs e)
        {
            SwitchDifficulty(Difficulty.Normal);
        }

        private void EasyKey_OnPress(object sender, EventArgs e)
        {
            SwitchDifficulty(Difficulty.Easy);
        }

        private void ImposibleKey_OnPress(object sender, EventArgs e)
        {
            SwitchDifficulty(Difficulty.Imposible);
        }

        #endregion
    }
}
