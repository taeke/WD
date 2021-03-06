﻿//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Config.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine
{
    using System.Collections.Generic;
    using WDGameEngine.Interfaces;

    public class Config
    {
        public int MinimumNumberPlayers { get; set; }

        public Dictionary<int, int> NumberOfPlayersIntialArmies { get; set; }

        public int MaximumCards { get; set; }
    }
}
