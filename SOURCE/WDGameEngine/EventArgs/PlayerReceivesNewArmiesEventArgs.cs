//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerReceivesNewArmiesEventArgs.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.EventArgs
{
    using System.Drawing;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// Information about a change to the number of new armies for <see cref="IPlayer"/>. New armies are the armies NOT yet placed
    /// on a <see cref="Country"/>
    /// </summary>
    public class PlayerReceivesNewArmiesEventArgs
    {
        /// <summary>
        /// The Color of the <see cref="IPlayer"/>.
        /// </summary>
        public readonly Color PlayerColor;

        /// <summary>
        /// The total number of new armies for this <see cref="IPlayer"/>. New armies are the armies NOT yet placed
        /// on a <see cref="Country"/>
        /// </summary>
        public readonly int Armies;

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="playerColor"> The Color of the <see cref="IPlayer"/> that gets the <see cref="Country"/>. </param>
        /// <param name="totalArmies"> The total number of new armies the <see cref="IPlayer"/> owns. New armies are the armies NOT yet placed
        /// on a <see cref="Country"/> </param>
        public PlayerReceivesNewArmiesEventArgs(Color playerColor, int totalArmies)
        {
            this.PlayerColor = playerColor;
            this.Armies = totalArmies;
        }
    }
}
