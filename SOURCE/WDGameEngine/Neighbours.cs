//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Neighbours.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine
{
    /// <summary>
    /// Helper class for loading the neighbours list in <see cref="Country"/>.
    /// </summary>
    public class Neighbours
    {
        /// <summary>
        /// One side of the neighbour connection.
        /// </summary>
        public string Country1 { get; set; }

        /// <summary>
        /// The other side of the neighbour connection.
        /// </summary>
        public string Country2 { get; set; }
    }
}
