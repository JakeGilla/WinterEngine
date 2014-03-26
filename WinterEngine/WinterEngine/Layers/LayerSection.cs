using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WinterEngine.GUI
{
    /// <summary>
    /// This struct defines a rectangular space on the screen to display text and images.
    /// Each layer section has a position, bounds, BG image, and ordinal number. The index
    /// allows more than one section in a layer to be identified and allows access to the
    /// sections in a specific order (for updating or .... other stuff).
    /// </summary>
    public struct LayerSection
    {
        #region attributes
        const int FIRST = 0;
        public Rectangle _bounds;
        public Point _position;
        public int _order;
        //enum ZDEFAULTS { FIRST = 0 };
        public Texture2D _background;

        #endregion

        #region properties
        public Rectangle Bounds
        {
            get { return this._bounds; }
            set { this._bounds = value; }
        }

        public Point Position
        {
            get { return this._position; }
            set { this._position = value; }
        }

        public int Order
        {
            get { return this._order; }
            set { this._order = value; }
        }

        #endregion


        /* I don't think I need this.
        
        public static LayerSection DefaultSection
        {
            get { return new LayerSection(new Rectangle(0, 0, Resolution.getVirtualResolution().X, Resolution.getVirtualResolution().Y), Point.Zero, FIRST, null); }
        }
        */

        #region constructor
        public LayerSection(Rectangle bounds, Point position, int order, Texture2D background)
        {
            _bounds = bounds;
            _position = position;
            _order = order;
            _background = background;
        }

        #endregion
    }


}
