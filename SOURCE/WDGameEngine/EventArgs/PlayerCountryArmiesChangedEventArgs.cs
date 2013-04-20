//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerCountryArmiesChangedEventArgs.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.EventArgs
{
    using System.Drawing;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// Information about a change to the number of armies for <see cref="IPlayer"/> on a <see cref="Country"/>. This
    /// can also mean the <see cref="IPlayer"/> receives a new <see cref="Country"/>.
    /// </summary>
    public class PlayerCountryArmiesChangedEventArgs
    {
        /// <summary>
        /// The Color of the <see cref="IPlayer"/>.
        /// </summary>
        public readonly Color PlayerColor;

        /// <summary>
        /// The <see cref="Country"/>.
        /// </summary>
        public readonly string Country;

        /// <summary>
        /// The number of armies on the <see cref="Country"/> for this <see cref="IPlayer"/>.
        /// </summary>
        public readonly int Armies;

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="playerColor"> The Color of the <see cref="IPlayer"/> that owns the <see cref="Country"/>. </param>
        /// <param name="countryName"> The <see cref="Country"/> for the <see cref="IPlayer"/>.</param>
        /// <param name="armies"> The number of armies on the  <see cref="Country"/>.</param>
        public PlayerCountryArmiesChangedEventArgs(Color playerColor, string countryName, int armies)
        {
            this.PlayerColor = playerColor;
            this.Country = countryName;
            this.Armies = armies;
        }
    }
}
