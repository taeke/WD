//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerReceivesNewCardEventArgs.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.EventArgs
{
    using System.Drawing;
    using WDGameEngine.Enums;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// Information about a new <see cref="CardType"/> for an <see cref="IPlayer"/>.
    /// </summary>
    public class PlayerReceivesNewCardEventArgs
    {
        /// <summary>
        /// The Color of the <see cref="IPlayer"/>.
        /// </summary>
        public readonly Color PlayerColor;

        /// <summary>
        /// The <see cref="CardType"/> this <see cref="IPlayer"/> receives.
        /// </summary>
        public readonly CardType CardType;

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="playerColor"> The Color of the <see cref="IPlayer"/> that gets the <see cref="CardType"/>. </param>
        /// <param name="cardType"> The <see cref="CardType"/> the <see cref="IPlayer"/> gets.</param>
        public PlayerReceivesNewCardEventArgs(Color playerColor, CardType cardType)
        {
            this.PlayerColor = playerColor;
            this.CardType = cardType;
        }
    }
}
