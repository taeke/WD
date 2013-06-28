//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="VisualCountry.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace Data.Model
{
    using System.Collections.Generic;

    public class VisualCountry
    {
        public long ID { get; set; }

        /// <summary>
        /// The name of the Country.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The centerpoint for showing the number of armies on the Country.
        /// </summary>
        public MapPoint NumberCenterPoint { get; set; }

        /// <summary>
        /// The endpoint for showing the number of armies on the Country. If the number fits within the
        /// border it will be the same as NumberCenterPoint but otherwise this will point to the Country
        /// while the number is drawn outside the Country border.
        /// </summary>
        public MapPoint NumberEndPoint { get; set; }

        /// <summary>
        /// The points for drawing the Country.
        /// </summary>
        public List<MapPoint> BorderPoints { get; set; }
    }
}
