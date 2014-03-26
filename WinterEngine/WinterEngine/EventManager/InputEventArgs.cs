using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace WinterEngine
{
    public class InputEventArgs : WinterEventArgs
    {
        private Controls.Commands command;

        public Controls.Commands Command
        {
            get { return command; }
        }

        public InputEventArgs(Controls.Commands command) 
        {
            this.command = command;
        }
    }
}
