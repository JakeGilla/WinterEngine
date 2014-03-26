//////////////////////////////////////////////////////////////////////////
////License:  The MIT License (MIT)
////Copyright (c) 2010 David Amador (http://www.david-amador.com)
////
////Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
////
////The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
////
////THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//////////////////////////////////////////////////////////////////////////

/*
 * Much of David Amador's Resolution.cs code has been included in this class
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WinterEngine
{
    public class Camera
    {
        #region Fields
        List<object> senders;
        List<InputEventArgs> eventArgs;
        Viewport viewPort;
        /// <summary>
        /// Smooth will lerp between two points, giving a smooth camera pan.
        /// Direct will simply snap camera to new position.
        /// While there are methods to produce both effects, setting this camera property
        /// will define default behavior.
        /// </summary>
        public enum Style { Smooth, Direct }
        // these two accelerate scrolling if > 1.0f
        private float scrollSpeedX = 4.0f;
        private float scrollSpeedY = 4.0f;
        private float rotation = 0.0f; // in radians, from right = positive X axis = 0 rads
        private float rotationAmt = (float)(Math.PI / 60.0); // in radians, from right = positive X axis = 0 rads
        private float zoomAmount = 5.0f;
        Vector2 position;
        Vector2 center;
        private GraphicsDeviceManager _Device = null;
        // this is the resolution that the game will scale to, this is also the "window" size.
        // If this is smaller than the vres, it will scale down a larger area into a smaller window.
        // If this is larger than the vres, the vres will scale up to it, enlarging the port to fit the screen.
        private int _Width = 1080;
        private int _Height = 720;
        // virtual resolution is the resolution that the game will scale from
        private int _VWidth = 640;
        private int _VHeight = 480;
        private Matrix _transform;
        private Matrix _scaleMatrix;
        private Matrix _translationMatrix;
        private Matrix _rotationMatrix;
        private Matrix _antiRotationMatrix;
        private Matrix _centerMatrix;
        private Matrix _originMatrix;
        private bool _FullScreen = false;
        private bool _dirtyMatrix = true;
        #endregion 

        #region Properties
        /// <summary>
        /// Get virtual Mode Aspect Ratio
        /// </summary>
        /// <returns>aspect ratio</returns>
        public float getVirtualAspectRatio()
        {
            return (float)_VWidth / (float)_VHeight;
        }

        public Point getVirtualResolution()
        {
            return new Point(_VWidth, _VHeight);
        }

        public Point getResolution()
        {
            return new Point(_Width, _Height);
        }

        public Rectangle getTileSafeArea()
        {
            return new Rectangle(0, 0, 0, 0);
        }
        #endregion 

        public Camera() { }
            

        public void Init(ref GraphicsDeviceManager device)
        {
            _Width = device.PreferredBackBufferWidth;
            _Height = device.PreferredBackBufferHeight;
            _Device = device;
            _dirtyMatrix = true;
            ApplyResolutionSettings();

            viewPort = new Viewport();
        }

        public Matrix getTransformationMatrix()
        {
            if (_dirtyMatrix)
            {
                RecreateScaleMatrix();
                RecreateTranslationMatrix();
                RecreateRotationMatrix();
                RecreateShiftMatrices();

                _dirtyMatrix = false;
                _transform = _centerMatrix * _rotationMatrix * _scaleMatrix * _originMatrix * _translationMatrix;
            }
            return _transform;
        }

        /// <summary>
        /// These are the underlying base methods for each type of transform procedure.
        /// If any more transforms need to be added, group them here.
        /// </summary>
        private void RecreateScaleMatrix()
        {
            //_dirtyMatrix = false;
            _scaleMatrix = Matrix.CreateScale(
                            (float)_Device.GraphicsDevice.Viewport.Width / _VWidth,
                            (float)_Device.GraphicsDevice.Viewport.Height / _VHeight,
                            1f);

            //RecreateShiftMatrices();
        }

        private void RecreateTranslationMatrix()
        {
            //_positionChanged = false;
            //_translationMatrix = Matrix.CreateTranslation(viewPort.X, viewPort.Y, 0);
            _translationMatrix = Matrix.CreateTranslation(position.X, position.Y, 0);
            //_translationMatrix = Matrix.CreateTranslation(rotatedDir.X, rotatedDir.Y, 0);
        }

        private void RecreateRotationMatrix()
        {
            //_rotAngChanged = false;
            _rotationMatrix = Matrix.CreateRotationZ(rotation);
            _antiRotationMatrix = Matrix.CreateRotationZ(-rotation);
            //rotatedDir = Vector3.Transform(direction,_antiRotationMatrix);
            //viewPort.X = (int)rotatedDir.X;
            //viewPort.Y = (int)rotatedDir.Y;

            //_rotationMatrix = Matrix.CreateFromAxisAngle(rotationPoint, rotation);
        }

        private void RecreateShiftMatrices()
        {
            //Vector2 center = new Vector2( (float)( (_VWidth / 2) + topLeft.X), (float)( (_VHeight / 2) + topLeft.Y) );
            //center = new Vector2( (float)(topLeft.X + (_VWidth / 2)), (float)(topLeft.Y + (_VHeight / 2) ) );
            //center = new Vector2(0, 0);
            //_centerMatrix = Matrix.CreateTranslation(-(_VWidth / 2) - viewPort.X, -(_VHeight / 2) - viewPort.Y, 0);
            //_originMatrix = Matrix.CreateTranslation(((_VWidth / 2) + viewPort.X), ((_VHeight / 2) + viewPort.Y), 0);
            //_centerMatrix = Matrix.CreateTranslation(-(_VWidth / 2) - 0, -(_VHeight / 2) - 0, 0);
            //_originMatrix = Matrix.CreateTranslation(((_VWidth / 2) + 0), ((_VHeight / 2) + 0), 0);
            _centerMatrix = Matrix.CreateTranslation(new Vector3(-center,0.0f));
            _originMatrix = Matrix.CreateTranslation(new Vector3(center,0.0f));
        }

        public void SubscribeToHandler(InputState iState)
        {
            //em.PlaySound += new EventManager.SoundEventHandler(EventFired);
            // lambda expression
            iState.Controls.FireInputEvent  += (sender, firedEvent) =>
            {
                //senders.Add(sender);
                //eventArgs.Add(firedEvent);

                /*
                 * So I wanted this to be quick, a simple switch will check on at most N items,
                 * where N is the number of buttons assigned a command. This should never exceed 20,
                 * mostly because I fucking say so, but also because even the most complicated RTS or RPG
                 * can be done elegantly so as to not force the player to remember an ungodly amount of
                 * commands. In any case, this area is definitely a candidate for future overhaul pending
                 * future analysis.
                 */
                switch(firedEvent.Command)
                {
                    case Controls.Commands.GameUp :
                        position.Y += (int)scrollSpeedY;
                        //viewPort.Y += (int)scrollSpeedY;
                        //direction = new Vector3(direction.X, direction.Y + (int)scrollSpeedY, 0);
                        _dirtyMatrix = true;
                        break;
                    case Controls.Commands.GameLeft :
                        position.X += (int)scrollSpeedX;
                        //viewPort.X += (int)scrollSpeedX;
                        //direction = new Vector3(direction.X + (int)scrollSpeedX, direction.Y, 0);
                        _dirtyMatrix = true;
                        break;
                    case Controls.Commands.GameDown :
                        position.Y -= (int)scrollSpeedY;
                        //viewPort.Y -= (int)scrollSpeedY;
                        //direction = new Vector3(direction.X, direction.Y - (int)scrollSpeedY, 0);
                        _dirtyMatrix = true;
                        break;
                    case Controls.Commands.GameRight :
                        position.X -= (int)scrollSpeedX;
                        //viewPort.X -= (int)scrollSpeedX;
                        //direction = new Vector3(direction.X - (int)scrollSpeedX, direction.Y, 0);
                        _dirtyMatrix = true;
                        break;
                    case Controls.Commands.RotateCC:
                        rotation += rotationAmt;
                        _dirtyMatrix = true;
                        break;
                    case Controls.Commands.RotateCW:
                        rotation -= rotationAmt;
                        _dirtyMatrix = true;
                        break;
                    case Controls.Commands.Action3:
                        rotation = 0;
                        _dirtyMatrix = true;
                        break;
                    case Controls.Commands.ZoomIn:
                        SetVirtualResolution(_VWidth + (int)(zoomAmount * getVirtualAspectRatio()), _VHeight + (int)zoomAmount);
                        _dirtyMatrix = true;
                        break;
                    case Controls.Commands.ZoomOut:
                        SetVirtualResolution(_VWidth - (int)(zoomAmount * getVirtualAspectRatio()), _VHeight - (int)zoomAmount);
                        _dirtyMatrix = true;
                        break;
                }

                //if(_dirtyMatrix) 

            };
        }

        /*
        public Matrix getScaleMatrix()
        {
            if (_dirtyMatrix) RecreateScaleMatrix();

            return _ScaleMatrix;
        }*/

        public void SetResolution(int Width, int Height, bool FullScreen)
        {
            _Width = Width;
            _Height = Height;

            _FullScreen = FullScreen;

            ApplyResolutionSettings();
        }

        public void SetVirtualResolution(int Width, int Height)
        {
            _VWidth = Width;
            _VHeight = Height;

            viewPort.Width = _VWidth;
            viewPort.Height = _VHeight;

            //center = new Vector2((float)(viewPort.X + (viewPort.Width / 2)), (float)(viewPort.Y + (viewPort.Height / 2)));
            //center = new Vector2( (float)(viewPort.Width / 2), (float)(viewPort.Height / 2) );

            //topLeft.X = 0;
            //topLeft.Y = 0;

            //center = new Vector2((float)(topLeft.X + (_VWidth / 2)), (float)(topLeft.Y + (_VHeight / 2)));

            //rotationPoint = new Vector3(new Vector2(topLeft.X + (_VWidth / 2), topLeft.Y + (_VHeight / 2)), 0);

            _dirtyMatrix = true;
        }

        public void InitializeView(int Width, int Height, int startingX, int startingY)
        {
            SetVirtualResolution(Width, Height);

            viewPort.X = startingX;
            viewPort.Y = startingY;

            //center = new Vector2((float)(viewPort.X + (viewPort.Width / 2)), (float)(viewPort.Y + (viewPort.Height / 2)));

            /*if (rotateAroundCenter)
            {
                rotationPoint = new Vector3(new Vector2(topLeft.X + (_VWidth / 2), topLeft.Y + (_VHeight / 2)), 1);
                rotationPoint = new Vector3(0,0,1);
            }*/

            _dirtyMatrix = true;
        }

        private void ApplyResolutionSettings()
        {

    #if XBOX360
            _FullScreen = true;
    #endif

            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (_FullScreen == false)
            {
                if ((_Width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (_Height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    _Device.PreferredBackBufferWidth = _Width;
                    _Device.PreferredBackBufferHeight = _Height;
                    _Device.IsFullScreen = _FullScreen;
                    _Device.ApplyChanges();
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set.  To do this, we will
                // iterate through the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == _Width) && (dm.Height == _Height))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        _Device.PreferredBackBufferWidth = _Width;
                        _Device.PreferredBackBufferHeight = _Height;
                        _Device.IsFullScreen = _FullScreen;
                        _Device.ApplyChanges();
                    }
                }
            }

            _dirtyMatrix = true;

            _Width = _Device.PreferredBackBufferWidth;
            _Height = _Device.PreferredBackBufferHeight;
        }

        /// <summary>
        /// Sets the device to use the draw pump
        /// Sets correct aspect ratio
        /// </summary>
        public void BeginDraw()
        {
            // Start by reseting viewport to (0,0,1,1)
            FullViewport();
            // Clear to Black
            _Device.GraphicsDevice.Clear(Color.Black);
            // Calculate Proper Viewport according to Aspect Ratio
            ResetViewport();
            // and clear that
            // This way we are gonna have black bars if aspect ratio requires it and
            // the clear color on the rest
            //_Device.GraphicsDevice.Clear(Color.Black);
        }

         public void FullViewport()
        {
            Viewport vp = new Viewport();
            vp.X = vp.Y = 0;
            vp.Width = _Width;
            vp.Height = _Height;
            _Device.GraphicsDevice.Viewport = vp;
        }

         public void ResetViewport()
        {
            float targetAspectRatio = getVirtualAspectRatio();
            // figure out the largest area that fits in this resolution at the desired aspect ratio
            int width = _Device.PreferredBackBufferWidth;
            int height = (int)(width / targetAspectRatio + .5f);
            bool changed = false;

            if (height > _Device.PreferredBackBufferHeight)
            {
                height = _Device.PreferredBackBufferHeight;
                // PillarBox
                width = (int)(height * targetAspectRatio + .5f);
                changed = true;
            }

            // set up the new viewport centered in the backbuffer
            Viewport vp = new Viewport();
            //int bufferCenterX = (_Device.PreferredBackBufferWidth / 2) - (width / 2);
            //int bufferCenterY = (_Device.PreferredBackBufferHeight / 2) - (height / 2);
            vp.X = (_Device.PreferredBackBufferWidth / 2) - (width / 2);
            vp.Y = (_Device.PreferredBackBufferHeight / 2) - (height / 2); ;
            vp.Width = width;
            vp.Height = height;
            vp.MinDepth = 0;
            vp.MaxDepth = 1;

            center = new Vector2((float)(vp.Width / 2), (float)(vp.Height / 2));

            if (changed)
            {
                _dirtyMatrix = true;
            }

            _Device.GraphicsDevice.Viewport = vp;
        }
    }
}
