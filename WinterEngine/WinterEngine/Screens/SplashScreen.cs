using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WinterEngine
{
    public abstract class SplashScreen : GameScreen
    {
        protected  GameTime time;
        protected  TimeSpan delta;
        protected  TimeSpan duration;
        protected  bool durationHasElapsed;

        protected SplashScreen(Game game) : base(game)
        {
            delta = TimeSpan.Zero;
        }

        protected abstract void onUpdate(GameTime gt);

    }
}
