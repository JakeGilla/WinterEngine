using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WinterEngine
{
    public class Input
    {
        KeyboardState prevKeyboardState;
        KeyboardState currKeyboardState;

        MouseState prevMouseState;
        MouseState currMouseState;

        // these variable should not be here
        // I need to make a logical object to
        // contain global variables like resolution
        // settings or other configurations
        /*
        private Vector2 gameScreenSize;
        private Vector2 gameScreenCoords;
        public Rectangle CartGameBounds;
        private double gameScreenX;
        private double gameScreenY;
        private double halfGameScreenX;
        private double halfGameScreenY;
        private float angleInRads;
          */

        public KeyboardState KeyboardState
        {
            get { return currKeyboardState; }

        }

        public MouseState MouseState
        {
            get { return currMouseState; }

        }

        /* same with these properties ...
        public Vector2 GameScreenCoords
        {
            get { return gameScreenCoords; }

        }

        public double GameScreenX
        {

            get { return gameScreenX; }
        }

        public double GameScreenY
        {

            get { return gameScreenY; }
        }

        public Vector2 GameScreenSize
        {

            get { return gameScreenSize; }
        }

        public Input(Vector2 gameScreenSize)
        {
            // TODO: Complete member initialization
            this.gameScreenSize = gameScreenSize;

        }
        */

        public void Initialize()
        {

            currKeyboardState = Keyboard.GetState();
            prevKeyboardState = Keyboard.GetState();
            currMouseState = Mouse.GetState();
            prevMouseState = Mouse.GetState();

            /*
            halfGameScreenX = GameScreenSize.X / 2;
            halfGameScreenY = GameScreenSize.Y / 2;

            CartGameBounds = new Rectangle(0, 0, (int)GameScreenSize.X, (int)GameScreenSize.Y);*/
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (FocusStateMGMT.ApplicationIsActivated())
            {

                if (MouseMoved())
                {
                    // This algorithm simply determines what quadrant
                    // the mouse cursor is in. There is a "blind spot"
                    // at the origin that is not being accounted for yet ...
                    /*
                    if (currMouseState.X < 0)
                        gameScreenX = -halfGameScreenX;
                    else if (currMouseState.X > GameScreenSize.X)
                        gameScreenX = halfGameScreenX;
                    else
                        gameScreenX = currMouseState.X - halfGameScreenX;

                    if (currMouseState.Y < 0)
                        gameScreenY = halfGameScreenY;
                    else if (currMouseState.Y > GameScreenSize.Y)
                        gameScreenY = -halfGameScreenY;
                    else
                        gameScreenY = halfGameScreenY - currMouseState.Y;


                    gameScreenCoords.X = (float)gameScreenX;
                    gameScreenCoords.Y = (float)gameScreenY;
                     */
                }

                prevKeyboardState = currKeyboardState;
                prevMouseState = currMouseState;

                currKeyboardState = Keyboard.GetState();
                currMouseState = Mouse.GetState();
            }
        }

        private bool MouseMoved()
        {
            if (currMouseState.X != prevMouseState.X || currMouseState.Y != prevMouseState.Y)
                return true;
            else
                return false;
        }

        public bool IsKeyPressedOnce(Keys key)
        {
            if (FocusStateMGMT.ApplicationIsActivated())
            {
                if (currKeyboardState.IsKeyDown(key) && (currKeyboardState != prevKeyboardState))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool IsMouseButtonPressedOnce(string mButton)
        {
            if (FocusStateMGMT.ApplicationIsActivated())
            {
                if (mButton == "LeftClick")
                {
                    if (currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton != ButtonState.Pressed)
                        return true;
                    else
                        return false;
                }
                else if (mButton == "RightClick")
                {
                    if (currMouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton != ButtonState.Pressed)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private int InWhatQuadrant(float x, float y)
        {
            if (x > 0 && y > 0)
                return 1;
            else if (x < 0 && y > 0)
                return 2;
            else if (x < 0 && y < 0)
                return 3;
            else if (x > 0 && y < 0)
                return 4;
            else
                return 0;
        }

        /*
        public float GetAngle()
        {

            int q;

            q = InWhatQuadrant(gameScreenCoords.X, gameScreenCoords.Y);

            if (q == 1)
                angleInRads = -((float)Math.Asin(gameScreenCoords.Y / gameScreenCoords.Length()));
            else if (q == 2)
                angleInRads = -((float)Math.Acos(gameScreenCoords.X / gameScreenCoords.Length()));
            else if (q == 3)
                angleInRads = ((float)Math.Asin(gameScreenCoords.Y / gameScreenCoords.Length())) - (float)Math.PI;
            else if (q == 4)
                angleInRads = -((float)Math.Asin(gameScreenCoords.Y / gameScreenCoords.Length())) - ((float)Math.PI * 2);
            return angleInRads;
        }*/

        //public float GetAngle(float cordX, float cordY)
        //{
        //    int q;
        //    float angle;

        //    q = InWhatQuadrant(cordX, cordY);

        //    switch (q)
        //    {
        //        case 1:
        //            angle = -((float)Math.Asin(gameScreenCoords.Y / gameScreenCoords.Length()));
        //            break;
        //        case 2:
        //            angle = -((float)Math.Acos(gameScreenCoords.X / gameScreenCoords.Length()));
        //            break;
        //        case 3:
        //            angle = ((float)Math.Asin(gameScreenCoords.Y / gameScreenCoords.Length())) - (float)Math.PI;
        //            break;
        //        case 4:
        //            angle = -((float)Math.Asin(gameScreenCoords.Y / gameScreenCoords.Length())) - ((float)Math.PI * 2);
        //            break;
        //        default:
        //            angle = -99;
        //            break;
        //    }

        //    return angle;
        //}

        //public Vector2 UnitVector()
        //{
        //    Vector2 unitVector;

        //    float magnitude;

        //    magnitude = (float)Math.Sqrt((gameScreenCoords.X * gameScreenCoords.X) + (gameScreenCoords.Y * gameScreenCoords.Y));

        //    unitVector = gameScreenCoords / magnitude;

        //    return unitVector;
        //}
    }
}
