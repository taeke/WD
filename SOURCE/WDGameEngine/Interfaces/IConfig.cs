//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfig.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Configuration for the Game
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// The minimum number of <see cref="IPlayer"/>.
        /// </summary>
        int MinimumNumberPlayers { get; set; }

        /// <summary>
        /// How many armies a <see cref="IPlayer"/> gets at the start of the game depents on how many players entered the game.
        /// </summary>
        Dictionary<int, int> NumberOfPlayersIntialArmies { get; set; }

        /// <summary>
        /// The maximum number of <see cref="CardType"/> a <see cref="IPlayer"/> may hold before he must exchange cards.
        /// </summary>
        int MaximumCards { get; set; } 
    }
}
