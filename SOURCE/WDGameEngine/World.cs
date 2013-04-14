//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="World.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine
{
    using System.Collections.Generic;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// <inheritDoc/>
    /// </summary>
    public class World : IWorld
    {
        /// <summary>
        /// Backing field for Continents.
        /// </summary>
        private List<Continent> continents = new List<Continent>();

        /// <summary>
        /// Backing field for Countries.
        /// </summary>
        private List<Country> countries = new List<Country>();

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public List<Continent> Continents
        {
            get
            {
                return this.continents;
            }
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public List<Country> Countries
        {
            get 
            {
                return this.countries; 
            }
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public void LoadContinentsAndCountries()
        {
            throw new System.NotImplementedException();
        }
    }
}
