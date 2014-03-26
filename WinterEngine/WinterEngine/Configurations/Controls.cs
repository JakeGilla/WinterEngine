using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Input;

namespace WinterEngine
{
    public class Controls
    {
        public enum Commands 
        {
            MenuUp,
            MenuLeft,
            MenuDown,
            MenuRight,
            GameUp,
            GameLeft,
            GameDown,
            GameRight,
            Action1,
            Action2,
            Action3,
            Action4,
            RotateCW,
            RotateCC,
            ZoomIn,
            ZoomOut
        }

        public enum MouseButtons
        {
            LeftClick,
            MiddleClick,
            RightClick,
            WheelUp,
            WheelDown
        }

        public delegate void InputEventHandler(Object sender, InputEventArgs inpEventArgs);
        public event InputEventHandler FireInputEvent;

        MainMenuCtrlScheme mmControlScheme;

        public MainMenuCtrlScheme MmControlScheme
        {
            get { return mmControlScheme; }
            //set { mmControlScheme = value; }
        }

        GamePlayCtrlScheme gpControlScheme;

        public GamePlayCtrlScheme GpControlScheme
        {
            get { return gpControlScheme; }
            //set { gpControlScheme = value; }
        }

        List<ControlScheme> additionalSchemes;

        string profile = "Default";

        public Controls(MainMenuCtrlScheme mmCtrls, GamePlayCtrlScheme gpCtrls) 
        {
            mmControlScheme = mmCtrls;
            gpControlScheme = gpCtrls;
        }

        public Commands CompareKeys(Keys[] keys)
        {
           Commands command = Commands.MenuUp;

            for(int i = 0; i < keys.Length; i++)
            {
                foreach (KeyValuePair<Keys, string> key in gpControlScheme.KScheme)
                {
                    if(keys[i] == key.Key) // <-- fucking confusing
                    {
                        command = (Commands)Enum.Parse(typeof(Commands), key.Value);

                        if (FireInputEvent != null)
                        {
                            FireInputEvent(this, new InputEventArgs(command));
                        }

                        break;
                    }
                }
            }

            return command;
        }

        public void HandleMouseInput(MouseButtons button)
        {
            Commands command;

            foreach (KeyValuePair<MouseButtons, string> mb in gpControlScheme.MScheme)
            {
                if (button == mb.Key)
                {
                    command = (Commands)Enum.Parse(typeof(Commands), mb.Value);

                    if (FireInputEvent != null)
                    {
                        FireInputEvent(this, new InputEventArgs(command));
                    }

                    break;
                }
            }
        }
    }
}
