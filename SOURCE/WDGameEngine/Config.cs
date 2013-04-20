namespace WDGameEngine
{
    using System.Collections.Generic;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// <inheritDoc/>
    /// </summary>
    public class Config
    {
        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public int MinimumNumberPlayers { get; set; }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public Dictionary<int, int> NumberOfPlayersIntialArmies { get; set; }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public int MaximumCards { get; set; }
    }
}
