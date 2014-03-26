using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace WinterEngine
{
    public class GamePlayCtrlScheme : ControlScheme
    {
        public Dictionary<Keys, string> KScheme
        {
            get { return kScheme; }
        }

        public Dictionary<Controls.MouseButtons, string> MScheme
        {
            get { return mScheme; }
        }

        public GamePlayCtrlScheme(Dictionary<Keys, string> kCtrls, Dictionary<Controls.MouseButtons, string> mCtrls)
        {
            mScheme = mCtrls;
            kScheme = kCtrls;
        }
    }
}
