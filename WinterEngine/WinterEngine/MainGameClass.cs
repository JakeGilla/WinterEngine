using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WinterEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGameClass : Microsoft.Xna.Framework.Game
    {
        #region data members
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Camera mainCamera;

        public Camera MainCamera
        {
            get { return mainCamera; }
        }
        ScreenManager screenManager;
        ConfigManager config;
        bool playFullScreen = false;
        #endregion data members

        public GraphicsDeviceManager Graphics
        {
            get { return graphics; }
        }

        public MainGameClass()
        {
            // handle for the video card and sets the 
            // also defines the content folder
            // 
            graphics = new GraphicsDeviceManager(this);
            config = new ConfigManager();

               
            /* shouldn't need this if Resolution works fine
            * 
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            */
            /*
            Resolution.Init(ref graphics);
            // Change Virtual Resolution 
            Resolution.SetVirtualResolution(640, 480);
            Resolution.SetResolution(1920, 1080, false);

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
            */
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            config.Initialize();
            InputState inputState = new InputState(true, config);
            mainCamera = new Camera();
            mainCamera.SubscribeToHandler(inputState);
            // Create the screen manager component.
            screenManager = new ScreenManager(this, inputState);
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 720;
            //Resolution.Init(ref graphics);
            mainCamera.Init(ref graphics);
            // Change Virtual Resolution
            bool b;
            Point p = config.GetStartUpResolution(out b); //used out to get all three SetRes params
            //Resolution.SetResolution(p.X, p.Y, b);
            mainCamera.SetResolution(p.X, p.Y, b);
            p = config.GetStartUpVResolution();
            //Resolution.SetVirtualResolution(p.X, p.Y);
            mainCamera.InitializeView(p.X, p.Y, 50, 50);

            Content.RootDirectory = "Content";
            Components.Add(screenManager);
            /*
            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
            */

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Create a new SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(this), null);
            screenManager.AddScreen(new MainMenuScreen(this), null);

            if (config.PlaySplashScreen)
            {
                screenManager.AddScreen(new RenderedSplashScreen(this,"Dick", new System.TimeSpan(0,0,10)), null);
                //screenManager.AddScreen(new VideoSplashScreen(new System.TimeSpan(0,0,10)), null);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
