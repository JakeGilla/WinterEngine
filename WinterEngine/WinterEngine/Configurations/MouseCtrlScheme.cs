using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace WinterEngine
{
    public class MouseCtrlScheme : ControlScheme
    {
        public Dictionary<Controls.MouseButtons, string> Scheme
        {
            get { return mScheme; }
        }

        public MouseCtrlScheme(Dictionary<Controls.MouseButtons, string> ctrls)
        {
            mScheme = ctrls;
        }
    }
}
