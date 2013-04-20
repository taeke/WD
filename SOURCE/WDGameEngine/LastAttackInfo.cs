//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="LastAttackInfo.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine
{
    using WDGameEngine.Interfaces;

    /// <summary>
    /// Information abbout the last attack.
    /// </summary>
    public class LastAttackInfo
    {
        /// <summary>
        /// Was the attacking (current) <see cref="IPlayer"/> the winner.
        /// </summary>
        public bool AttackerIsWinner { get; set; }

        /// <summary>
        /// Which <see cref="Country"/> did the attack game from.
        /// </summary>
        public Country AttackingCountry { get; set; }

        /// <summary>
        /// Which <see cref="Country"/> was attacked.
        /// </summary>
        public Country DefendingCountry { get; set; }

        /// <summary>
        /// The number of armies used in the attack.
        /// </summary>
        public int AttackingArmies { get; set; }
    }
}
