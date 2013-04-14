//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="CardType.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Enums
{
    /// <summary>
    /// A <see cref="IPlayer"/> can win a CardType if he attacks a <see cref="Country"/> and wins. The index is also the number of amries when 
    /// a <see cref="IPlayer"/> exchanges a set of 3 of these cards.
    /// </summary>
    public enum CardType
    {
        /// <summary>
        /// Is a factor in how many armies a <see cref="IPlayer"/> gets once he exchanges a set of CardType which has this ArmyType for new Armies.
        /// </summary>
        Artillery = 4,

        /// <summary>
        /// Is a factor in how many armies a <see cref="IPlayer"/> gets once he exchanges a set of CardType which has this ArmyType for new Armies.
        /// </summary>
        Infantry = 6,

        /// <summary>
        /// Is a factor in how many armies a <see cref="IPlayer"/> gets once he exchanges a set of CardType which has this ArmyType for new Armies.
        /// </summary>
        Cavalry = 8,
    }
}