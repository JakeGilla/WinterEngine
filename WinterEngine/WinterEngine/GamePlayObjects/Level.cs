using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using WinterEngine;

namespace WinterEngine.GamePlayObjects
{
    class Level : DrawableGameComponent
    {

        // Structure of the level
        MainGameClass _game;
        Camera _camera;
        EventManager eventManager;
        private Block[,] blocks;
        private Texture2D[] backgroundLayers;
        Texture2D playerImage;
        public int _levelIndex;
        const int _backgrounds = 1;
        private Vector2 startingPoint;

        // Level content
        public ContentManager LevelContent
        {
            get { return levelContent; }
        }
        ContentManager levelContent;

        private Random random = new Random(1);


        /// <summary>
        /// Constructs a new level.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider that will be used to construct a ContentManager.
        /// </param>
        /// <param name="fileStream">
        /// A stream containing the tile data.
        /// </param>
        public Level(IServiceProvider serviceProvider, Game game, int levelIndex, Texture2D background)
            : base(game)
        {

            _levelIndex = levelIndex;

            _game = (MainGameClass)game;

            _camera = _game.MainCamera;

            _camera.InitializeView(640,480, 0, 0);

            // Create a new content manager to load content used just by this level.
            levelContent = new ContentManager(serviceProvider, "Content");

            //makes a new array of backgrounds
            backgroundLayers = new Texture2D[_backgrounds];
            for (int i = 0; i < backgroundLayers.Length; ++i)
            {
                // Choose a random segment if each background layer for level variety.
                int segmentIndex = levelIndex;
                backgroundLayers[i] = LevelContent.Load<Texture2D>("PHArt/bgDefault.jpg");
            }
        }

        /// <summary>
        /// Unloads the level content.
        /// </summary>
        public new void Dispose()
        {
            levelContent.Unload();
        }

          #region Update
          public override void Update(GameTime gameTime)
          {

          }
          #endregion

          #region Draw

          /// <summary>
          /// Draw everything in the level from background to foreground.
          /// </summary>
          public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
          {

          }
          #endregion


          /// <summary>
          /// Draws each tile in the level.
          /// </summary>
          private void DrawLayers(SpriteBatch spriteBatch)
          {

          }
     }
}
