//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="IWorld.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// All the available <see cref="Continent"/> and <see cref="Country"/>.
    /// </summary>
    public interface IWorld
    {
        /// <summary>
        /// The list of available <see cref="Continent"/>.
        /// </summary>
        List<Continent> Continents { get; }

        /// <summary>
        /// The list of available <see cref="Country"/>.
        /// </summary>
        List<Country> Countries { get; }

        /// <summary>
        /// Loads the Continents and Countries.
        /// </summary>
        void LoadContinentsAndCountries();
    }
}
