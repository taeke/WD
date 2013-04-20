//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Continent.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine
{
    using System.Collections.Generic;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// A Continent has a collection of <see cref="Country"/>.
    /// </summary>
    public class Continent
    {
        /// <summary>
        /// The <see cref="IPlayer"/> owning this complete Continent returns this amount of new armies in every turn.
        /// </summary>
        public int NewArmiesForOwning { get; set; }

        /// <summary>
        /// The name of the Continent.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of <see cref="Country"/> which are part of this Continent.
        /// </summary>
        public List<Country> Countries { get; set; }
    }
}
