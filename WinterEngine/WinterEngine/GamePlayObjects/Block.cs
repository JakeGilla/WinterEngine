using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WinterEngine.GamePlayObjects
{
     public enum BlockCollision
     {
          /// A passable tile is one which does not hinder player motion at all.
          Passable = 0,

          // It is completely solid.
          Impassable = 1,

          // A non-"portable" <-- get it, tile that portals will not be able to land on
          NonPortable = 2,
     }

     public class Block
     {
          public BlockCollision blkCollision;

          public Texture2D blockTexture;
          Vector2 position;

          public Rectangle blockRect;

          private Vector2 portalSpawnPointTop;
          private Vector2 portalSpawnPointLeft;
          private Vector2 portalSpawnPointBottom;
          private Vector2 portalSpawnPointRight;

          public Vector2 PortalSpawnPointTop
          {
               get { return portalSpawnPointTop; }
          }

          public Vector2 PortalSpawnPointLeft
          {
               get { return portalSpawnPointLeft; }
          }

          public Vector2 PortalSpawnPointBottom
          {
               get { return portalSpawnPointBottom; }
          }

          public Vector2 PortalSpawnPointRight
          {
               get { return portalSpawnPointRight; }
          }


          public Block(Texture2D block, Vector2 where, BlockCollision collision)
          {
               this.blockTexture = block;
               this.position = where;
               blkCollision = collision;
          }

          public void Initialize()
          {

          }

          public void Update(GameTime gameTime)
          {
               blockRect = new Rectangle((int)position.X, (int)position.Y, blockTexture.Width, blockTexture.Height);

               //I don't think this belongs in the Update method. Oh well.

          }

          public void Draw(SpriteBatch spriteBatch)
          {
               spriteBatch.Draw(blockTexture, position, Color.White);
          }

          public void GetPortalSpawnPoint()
          {
               portalSpawnPointTop.X = (float)blockRect.X;
               portalSpawnPointTop.Y = (float)blockRect.Y - 18;

               portalSpawnPointLeft.X = (float)blockRect.X - 18;
               portalSpawnPointLeft.Y = (float)blockRect.Y;

               portalSpawnPointBottom.X = (float)blockRect.X;
               portalSpawnPointBottom.Y = (float)blockRect.Y + (float)blockRect.Height;

               portalSpawnPointRight.X = (float)blockRect.X + (float)blockRect.Width;
               portalSpawnPointRight.Y = (float)blockRect.Y;
          }
     }
}
