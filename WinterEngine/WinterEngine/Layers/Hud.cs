using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinterEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WinterEngine.GUI
{
     class Hud : Layer
     {
         bool _isVisible;

         ContentManager guiContent;

         SpriteFont hudFont1;
         SpriteFont hudFont2;
         SpriteFont hudFont3;
         SpriteFont hudFont4;

         Texture2D hudBg1;
         Texture2D hudBg2;
         Texture2D hudBg3;
         Texture2D hudBg4;

          public Hud(int zOrder, bool isVisible) : base(zOrder) {
              _isVisible = isVisible;
          }

          public void Initialize(ContentManager guiContent)
          {
              /*
               * Here I add four preset, premade layersections, mostly for testing purposes.
               * Blah blah
              */

              if (guiContent != null)
              {
                  hudFont1 = guiContent.Load<SpriteFont>("gamefont");
                  hudFont2 = guiContent.Load<SpriteFont>("gamefont");
                  hudFont3 = guiContent.Load<SpriteFont>("gamefont");
                  hudFont4 = guiContent.Load<SpriteFont>("gamefont");
                  hudBg1 = guiContent.Load<Texture2D>(@"PHArt/hudTest1");
                  hudBg2 = guiContent.Load<Texture2D>(@"PHArt/hudTest2");
                  hudBg3 = guiContent.Load<Texture2D>(@"PHArt/hudTest3");
                  hudBg4 = guiContent.Load<Texture2D>(@"PHArt/hudTest4");

                  this.AddLayerSection(0, 0, 306, 160, hudBg1);
              }
              else
              {
                  throw new ArgumentNullException();
              }
          }
     }
}
