using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace WinterEngine
{
    public class MainMenuCtrlScheme : ControlScheme
    {
        public Dictionary<Keys, string> KScheme
        {
            get { return kScheme; }
        }

        public Dictionary<Controls.MouseButtons, string> MScheme
        {
            get { return mScheme; }
        }

        public MainMenuCtrlScheme(Dictionary<Keys, string> kCtrls, Dictionary<Controls.MouseButtons, string> mCtrls)
        {
            mScheme = mCtrls;
            kScheme = kCtrls;
        }
    }
}
