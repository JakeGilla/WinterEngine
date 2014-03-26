using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WinterEngine
{
    public class VideoSplashScreen : SplashScreen, IDisposable
    {
        ContentManager content;
        VideoPlayer videoPlayer;
        Video video;
        Rectangle viewRec;
        TimeSpan timeElapsed;

        public VideoSplashScreen(Game game, TimeSpan dur) : base(game)
        {
            videoPlayer = new VideoPlayer();
            viewRec = new Rectangle(0, 0, Resolution.getResolution().X,
                Resolution.getResolution().Y);
            duration = dur;
        }

        protected override void onUpdate(GameTime gt)
        {
            base.delta = gt.ElapsedGameTime;
            timeElapsed += delta;
            if (timeElapsed > duration)
            {
                videoPlayer.Pause();
                ExitScreen();
            }
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            video = content.Load<Video>(ConfigManager.VideoSplashDefaultFile);

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            content.Unload();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // update the video stream
            videoPlayer.Play(video);
            onUpdate(gameTime);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(videoPlayer.GetTexture(), viewRec,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Dispose()
        {

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
            videoPlayer.Pause();
            ExitScreen();
        }
    }
}
