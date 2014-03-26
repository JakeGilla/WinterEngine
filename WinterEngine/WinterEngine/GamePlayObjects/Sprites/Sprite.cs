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

namespace WinterEngine
{
     class Sprite
     {
         Texture2D _texture;
          Vector2 _position;
          Color _color;
          float _rotation;

          public Sprite(Texture2D texture, Vector2 position, Color color, float rotation) {
               if(texture != null)
                    _texture = texture;
               _position = position;
               _color = color;
               _rotation = rotation;
          }
     }
}
