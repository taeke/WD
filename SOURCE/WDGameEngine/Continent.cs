﻿//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Continent.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine
{
    using System.Collections.Generic;
    using WDGameEngine.Interfaces;

    public class Continent
    {
        /// <summary>
        /// The <see cref="IPlayer"/> owning this complete Continent gets this amount of new armies in every turn.
        /// </summary>
        public int NewArmiesForOwning { get; set; }

        public string Name { get; set; }

        public List<Country> Countries { get; set; }
    }
}
