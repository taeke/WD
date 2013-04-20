//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="TurnType.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Enums
{
    using WDGameEngine.Interfaces;

    /// <summary>
    /// A <see cref="IPlayer"/> can choose to atack or get an army for every third <see cref="Country"/> he owns.
    /// </summary>
    public enum TurnType
    {
        /// <summary>
        /// The <see cref="IPlayer"/> has not chosen yet.
        /// </summary>
        NotChosen,

        /// <summary>
        /// The <see cref="IPlayer"/> has chosen to attack a neighbour.
        /// </summary>
        Attack,

        /// <summary>
        /// The <see cref="IPlayer"/> has chosen to get armies for every third <see cref="Country"/> he owns.
        /// </summary>
        GetArmiesForCountries
    }
}
