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

namespace WinterEngine.GamePlayObjects
{
     class AnimatedSprite : Sprite
     {
          float timeSinceLastFrame;
          float millisecondsPerFrame = 3.33F;
          Point sheetSize;
          Point currentFrame;

          AnimatedSprite(Texture2D texture, Vector2 position, Color color, float rotation) 
               : base(texture, position, color, rotation)
          {
               // yup

          }

          void Animate(GameTime gameTime) {
               timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
               if (timeSinceLastFrame > millisecondsPerFrame)
               {
                    timeSinceLastFrame -= millisecondsPerFrame;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X)
                    {
                         currentFrame.X = 0;
                    }
               }
          }
     }
}
