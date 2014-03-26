using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace WinterEngine
{
    public class RenderedSplashScreen : SplashScreen
    {
        Texture2D background;
        List<string> strList;
        string msg;
        ContentManager content;
        Rectangle viewRec;
        Vector2 center;
        TimeSpan duration;
        TimeSpan timeElapsed;
        SpriteFont font;

        public RenderedSplashScreen(Game game, string msg, TimeSpan dTime) :base(game)
        {
            this.msg = msg;
            viewRec = new Rectangle(0, 0, Resolution.getResolution().X,
                Resolution.getResolution().Y);
            center = new Vector2(viewRec.Width / 2, viewRec.Height / 2);
            timeElapsed = new TimeSpan(0,0,0);
            duration = dTime;
        }

        protected override void onUpdate(GameTime gt)
        {
            base.delta = gt.ElapsedGameTime;
            timeElapsed += delta;
            if (timeElapsed > duration)
            {
                ExitScreen();
            }
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            background = content.Load<Texture2D>("Backgrounds/jesusSplash");
            font = content.Load<SpriteFont>("splashFont");

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // update the video stream
            onUpdate(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(background, viewRec,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            spriteBatch.DrawString(ScreenManager.Font, msg, center, Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void HandleInput(InputState input)
        {
            // Look up inputs for the active player profile.
            PlayerIndex playerIndex = new PlayerIndex();
            if (input.IsNewKeyPress(Keys.Space, null, out playerIndex))
            {
                onCancel();
            }

            base.HandleInput(input);
        }

        private void onCancel()
        {
            ExitScreen();
        }
    }
}
