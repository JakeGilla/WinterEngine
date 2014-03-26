using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WinterEngine.GUI
{
    class Layer
    {
        private bool hasLayerSections = false;
        private int _zOrder;
        private int _sectionID = 0;
        public List<LayerSection> _layerSections;
        public List<string> _strings;
        Color _color;

        #region properties
        public bool HasLayerSections
        {
            get { return hasLayerSections; }
        }
        #endregion

        public Layer(int zOrder)
        {
            _zOrder = zOrder;
            _layerSections = new List<LayerSection>();
        }

        /// <summary>
        /// Adds a LayerSection to the list.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="lSectionRightX"></param>
        /// <param name="lSectionDownY"></param>
        /// <param name="img"></param>
        public void AddLayerSection(int x, int y, int rwidth, int rheight, Texture2D img)
        {
            // make sure the rect has positive area
            if (rwidth < 0)
            {
                rwidth = 0;
            }

            if (rheight < 0)
            {
                rheight = 0;
            }

            Rectangle r = new Rectangle(x, y, rwidth, rheight);

            Point p = new Point(x, y);
                
            _layerSections.Add(new LayerSection(r, p, _sectionID, img));

            CheckLayerSections(); // set/reset flag

            _sectionID++;
        }

        /// <summary>
        /// Removes a specific LayerSection
        /// </summary>
        /// <param name="layerSectionID"></param>
        public void RemoveLayerSection(int layerSectionID)
        {

        }

        /// <summary>
        /// Returns the values of a specific LayerSection based on the order #.
        /// </summary>
        public void GetLayerSection()
        {

        }

        /// <summary>
        /// This just clears the LayerSections list and resets the flag.
        /// </summary>
        public void DeleteLayerSections()
        {
            _layerSections.Clear();
            hasLayerSections = false;
        }

        private void CheckLayerSections()
        {
            if (_layerSections.Count() == 0)
                hasLayerSections = false;
            else
                hasLayerSections = true;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this._layerSections.ElementAt(0)._background, this._layerSections.ElementAt(0)._bounds, Color.White);
        }
    }
}
