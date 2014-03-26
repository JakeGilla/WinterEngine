using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace WinterEngine
{
    public abstract class ControlScheme
    {
        protected Dictionary<Keys,string> kScheme;
        protected Dictionary<Controls.MouseButtons, string> mScheme;
    }
}
