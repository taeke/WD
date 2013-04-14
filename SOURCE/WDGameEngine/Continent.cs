//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Continent.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine
{
    using System.Collections.Generic;

    /// <summary>
    /// A Continent has a collection of <see cref="Country"/>.
    /// </summary>
    public class Continent
    {
        /// <summary>
        /// Backing field for Countries.
        /// </summary>
        private List<Country> countries = new List<Country>();

        /// <summary>
        /// The <see cref="IPlayer"/> owning this complete Continent returns this amount of new armies in every turn.
        /// </summary>
        public int NewArmiesForOwning { get; set; }

        /// <summary>
        /// The list of <see cref="Country"/> which are part of this Continent.
        /// </summary>
        public List<Country> Countries
        {
            get
            {
                return this.countries;
            }
        }
    }
}
