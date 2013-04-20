//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="IWDRepository.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Loading the persistant part of the <see cref="IGame"/> from a database.
    /// </summary>
    public interface IWDRepository
    {
        /// <summary>
        /// Gets the list of <see cref="Continent"/>.
        /// </summary>
        /// <returns></returns>
        List<Continent> GetContinents();

        /// <summary>
        /// Gets the list of <see cref="Neighbours"/>.
        /// </summary>
        /// <returns></returns>
        List<Neighbours> GetNeighbours();
    }
}
