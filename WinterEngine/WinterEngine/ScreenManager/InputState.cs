#region File Description
//-----------------------------------------------------------------------------
// InputState.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
#endregion

namespace WinterEngine
{
    /// <summary>
    /// Helper for reading input from keyboard, gamepad, and touch input. This class 
    /// tracks both the current and previous state of the input devices, and implements 
    /// query methods for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary>
    public class InputState
    {
        #region Fields
        public const int MaxInputs = 4;
        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly GamePadState[] CurrentGamePadStates;

        public readonly KeyboardState[] LastKeyboardStates;
        public readonly GamePadState[] LastGamePadStates;

        public readonly bool[] GamePadWasConnected;

        private bool useKeyAndMouse;

        KeyboardState prevKeyboardState;
        KeyboardState currKeyboardState;

        MouseState prevMouseState;
        MouseState currMouseState;

        private Controls controls;

        public Controls Controls
        {
            get { return controls; }
        }

        Controls.Commands command;

        //public TouchCollection TouchState;
        //public readonly List<GestureSample> Gestures = new List<GestureSample>();
        #endregion

        #region Initialization
        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState(bool useKeyAndMouse, ConfigManager cm)
        {
            this.useKeyAndMouse = useKeyAndMouse;

            controls = cm.GetControls();

            // we only check for one player
            currKeyboardState = Keyboard.GetState();
            prevKeyboardState = Keyboard.GetState();
            currMouseState = Mouse.GetState();
            prevMouseState = Mouse.GetState();

            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            CurrentGamePadStates = new GamePadState[MaxInputs];
            LastKeyboardStates = new KeyboardState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];
            GamePadWasConnected = new bool[MaxInputs];

          /*if (useKeyAndMouse)
            {
                // we only check for one player
                currKeyboardState = Keyboard.GetState();
                prevKeyboardState = Keyboard.GetState();
                currMouseState = Mouse.GetState();
                prevMouseState = Mouse.GetState();
            }
                else 
            {
                CurrentKeyboardStates = new KeyboardState[MaxInputs];
                CurrentGamePadStates = new GamePadState[MaxInputs];
                LastKeyboardStates = new KeyboardState[MaxInputs];
                LastGamePadStates = new GamePadState[MaxInputs];
                GamePadWasConnected = new bool[MaxInputs];
            } */

        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public void Update()
        {
            if (useKeyAndMouse)
            {
                if (FocusStateMGMT.ApplicationIsActivated())
                {
                    prevKeyboardState = currKeyboardState;
                    prevMouseState = currMouseState;

                    currKeyboardState = Keyboard.GetState();
                    currMouseState = Mouse.GetState();

                    CheckControls();

                    CheckMouse();

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

                    for (int i = 0; i < MaxInputs; i++)
                    {
                        LastKeyboardStates[i] = CurrentKeyboardStates[i];
                        LastGamePadStates[i] = CurrentGamePadStates[i];

                        CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                        CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                        // Keep track of whether a gamepad has ever been
                        // connected, so we can detect if it is unplugged.
                        if (CurrentGamePadStates[i].IsConnected)
                        {
                            GamePadWasConnected[i] = true;
                        }
                    }
                }
            }
            else
            {
              /*for (int i = 0; i < MaxInputs; i++)
                {
                    LastKeyboardStates[i] = CurrentKeyboardStates[i];
                    LastGamePadStates[i] = CurrentGamePadStates[i];

                    CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                    CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                    // Keep track of whether a gamepad has ever been
                    // connected, so we can detect if it is unplugged.
                    if (CurrentGamePadStates[i].IsConnected)
                    {
                        GamePadWasConnected[i] = true;
                    }
                }*/
            }

            /*TouchState = TouchPanel.GetState();

            Gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                Gestures.Add(TouchPanel.ReadGesture());
            }*/
        }


        /// <summary>
        /// Helper for checking if a key was newly pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer,
                                            out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentKeyboardStates[i].IsKeyDown(key) &&
                        LastKeyboardStates[i].IsKeyUp(key));
            }
            else
            {
                // Accept input from any player.
                return (IsNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Three, out playerIndex) ||
                        IsNewKeyPress(key, PlayerIndex.Four, out playerIndex));
            }
        }


        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a button press
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer,
                                                     out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button) &&
                        LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                // Accept input from any player.
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                        IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }


        /// <summary>
        /// Checks for a "menu select" input action.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsMenuSelect(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
                   IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "menu cancel" input action.
        /// The controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When the action
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsMenuCancel(PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "menu up" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsMenuUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Up, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.DPadUp, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "menu down" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsMenuDown(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.DPadDown, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex);
        }


        /// <summary>
        /// Checks for a "pause the game" input action.
        /// The controllingPlayer parameter specifies which player to read
        /// input for. If this is null, it will accept input from any player.
        /// </summary>
        public bool IsPauseGame(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex) ||
                   IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
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

        #endregion

        #region Private Methods
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

        private bool MouseMoved()
        {
            if (currMouseState.X != prevMouseState.X || currMouseState.Y != prevMouseState.Y)
                return true;
            else
                return false;
        }

        private void CheckControls()
        {
            Keys[] pKeys = prevKeyboardState.GetPressedKeys();
            Keys[] cKeys = currKeyboardState.GetPressedKeys();

            if(cKeys.Length > 0)
                command = Controls.CompareKeys(cKeys);
        }

        private void CheckMouse()
        {
            ButtonState[] mButtons = new ButtonState[3];
            int bsPrevMouseScrollValue;
            int bsCurrMouseScrollValue;


            mButtons[0] = currMouseState.LeftButton;
            mButtons[1] = currMouseState.MiddleButton;
            mButtons[2] = currMouseState.RightButton;
            bsPrevMouseScrollValue = prevMouseState.ScrollWheelValue;
            bsCurrMouseScrollValue = currMouseState.ScrollWheelValue;

            for(int i = 0; i < mButtons.Length ; i++)
            {
                if (mButtons[i] == ButtonState.Pressed)
                {
                    switch(i)
                    {
                        case (int)Controls.MouseButtons.LeftClick :
                            Controls.HandleMouseInput(Controls.MouseButtons.LeftClick);
                            break;
                        case (int)Controls.MouseButtons.MiddleClick :
                            Controls.HandleMouseInput(Controls.MouseButtons.MiddleClick);
                            break;
                        case (int)Controls.MouseButtons.RightClick :
                            Controls.HandleMouseInput(Controls.MouseButtons.RightClick);
                            break;

                    }
                }
            }

            if (bsPrevMouseScrollValue != bsCurrMouseScrollValue)
            {
                if (bsPrevMouseScrollValue > bsCurrMouseScrollValue)
                {
                    Controls.HandleMouseInput(Controls.MouseButtons.WheelUp);
                }
                else
                {
                    Controls.HandleMouseInput(Controls.MouseButtons.WheelDown);
                }
            }
        }

        #endregion
    }
}
