//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Country.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine
{
    using System.Collections.Generic;

    /// <summary>
    /// A Country is part of a <see cref="Continent"/> and has one or more neighbour Countries who share a border or can reach each other over see.
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Backing field for Neighbours.
        /// </summary>
        private List<Country> neighbours = new List<Country>();

        /// <summary>
        /// The name of the Country.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A list of Country object which share a border with this instance or can reach this instance over see.
        /// </summary>
        public List<Country> Neighbours 
        {
            get
            {
                return this.neighbours;
            }
        }
    }
}
