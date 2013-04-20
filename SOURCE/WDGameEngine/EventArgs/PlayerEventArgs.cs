//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerEventArgs.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.EventArgs
{
    using System.Drawing;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// Information about the <see cref="IPlayer"/> for which an event gets fired.
    /// </summary>
    public class PlayerEventArgs
    {
        /// <summary>
        /// The Color of the <see cref="IPlayer"/> for which the event gets fired.
        /// </summary>
        public readonly Color PlayerColor;

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="playerColor"> The Color of the <see cref="IPlayer"/> for which the event gets fired. </param>
        public PlayerEventArgs(Color playerColor)
        {
            this.PlayerColor = playerColor;
        }
    }
}
