#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using WinterEngine.AI;
using WinterEngine.GamePlayObjects;
using WinterEngine.GUI;
using WinterEngine.Scenes;
using WinterEngine.Sounds;
#endregion

namespace WinterEngine
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen, IDrawable
    {
        #region Fields
        MainGameClass mGame;
        ContentManager content;
        EventManager eventManager;
        SoundManager soundManager;
        AIManager aiManager;
        SceneManager sceneManager;
        SpriteFont gameFont;
        int drawOrder = 0;
        bool visible = true;

        Viewport singlePlayerVP = new Viewport(0, 0, 800, 600); // just added this, 4/5/2013
        Vector2 playerPosition = new Vector2(100, 100);
        Vector2 enemyPosition = new Vector2(100, 100);

        Texture2D _texture;

        Random random = new Random();

        float pauseAlpha;

        Hud gui;

        #endregion

        public int DrawOrder
        { get { return drawOrder; } }

        public bool Visible
        { get { return visible; }
            set { visible = value; }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(Game game) : base(game)
        {
            mGame = (MainGameClass)game;
            mGame.MainCamera.InitializeView(640,480,0,0);
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            eventManager = new EventManager();

            soundManager = new SoundManager();
            aiManager = new AIManager();
            sceneManager = new SceneManager();

            soundManager.SubscribeToHandler(eventManager);

            eventManager.FireSoundEvent(new Player(), new SoundEventArgs(1.0, 1));
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("gamefont");
            //_texture = content.Load<Texture2D>(@"PHArt/alley_1024x768");
            _texture = content.Load<Texture2D>(@"Backgrounds/testBackground");

            // need to add an img to test layer
            //_guiBG = content.Load<Texture2D>(@"PHArt/NEEDTOADD");

            gui = new Hud(0, true);

            try
            {
                gui.Initialize(content);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} exception caught.", e);
            }

            //MainGameClass.Graphics  = vp;

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                // Apply some random jitter to make the enemy move around.
                const float randomization = 10;

                enemyPosition.X += (float)(random.NextDouble() - 0.5) * randomization;
                enemyPosition.Y += (float)(random.NextDouble() - 0.5) * randomization;

                // Apply a stabilizing force to stop the enemy moving off the screen.
                Vector2 targetPosition = new Vector2(
                    ScreenManager.GraphicsDevice.Viewport.Width / 2 - gameFont.MeasureString("Insert Gameplay Here").X / 2, 
                    200);

                enemyPosition = Vector2.Lerp(enemyPosition, targetPosition, 0.05f);

                // TODO: this game isn't very fun! You could probably improve
                // it by inserting something more interesting in this space :-)
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(this.game), ControllingPlayer);
            }
            else
            {
                /*
                // Otherwise move the background to scroll
                Vector2 movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.A))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.D))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.W))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.S))
                    movement.Y++;

                Vector2 thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                //playerPosition += movement * 2;
                singlePlayerVP.X += (int)movement.X * 2;
                singlePlayerVP.Y += (int)movement.Y * 2;
                */
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            mGame.MainCamera.BeginDraw();
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,Color.Black, 0, 0);
            spriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                BlendState.AlphaBlend, null, null, null, null,
                mGame.MainCamera.getTransformationMatrix());
            spriteBatch.Draw(_texture, Vector2.Zero, Color.White);
            spriteBatch.DrawString(gameFont, "// TODO", playerPosition, Color.Green);
            spriteBatch.DrawString(gameFont, "Insert Gameplay Here",enemyPosition, Color.DarkRed);
            //gui.Draw(spriteBatch);
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
        #endregion
    }
}
