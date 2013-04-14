//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlayer.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Interfaces
{
    using System.Collections.Generic;
    using System.Drawing;
    using WDGameEngine.Enums;

    /// <summary>
    /// A "Person" playing the game.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// A list with <see cref="Country"/> which are owned by this IPlayer and the number of Armies he placed on this <see cref="Country"/>.
        /// </summary>
        Dictionary<Country, int> CountryNumberOfArmies { get; }

        /// <summary>
        /// Every turn the IPlayer receives an new amount of armies. This holds this amount untill they are placed on one or more of his <see cref="Country"/>.
        /// </summary>
        int NumberOfNewArmies { get; set; }

        /// <summary>
        /// Players are recognized by their color. 
        /// </summary>
        Color Color { get; }

        /// <summary>
        /// Generates a random number like rolling a dice.
        /// </summary>
        Randomize Dice { get; }

        /// <summary>
        /// The <see cref="TurnType"/> the player has chosen for his current turn.
        /// </summary>
        TurnType TurnType { get; set; }

        /// <summary>
        /// The collection of <see cref="CardType"/> the IPLayer owns.
        /// </summary>
        List<CardType> Cards { get; }

        /// <summary>
        /// Returns an descending ordert list with the ints for rolling the dice. This overload is for defending.
        /// </summary>
        /// <param name="country"> The armies on the <see cref="Country"/> playes a roll in how many dice 
        /// the IPlayer may use in case of defence. </param>
        /// <returns> A descending orderd list of ints. </returns>
        List<int> RollTheDices(Country country);

        /// <summary>
        /// Returns an descending ordert list with the ints for rolling the dice. This overload is for attacking.
        /// </summary>
        /// <param name="numberOfArmies"> The armies used for the attack playes a roll in how many dice the IPlayer
        /// can use in case of an attack. </param>
        /// <returns> A descending orderd list of ints. </returns>
        List<int> RollTheDices(int numberOfArmies);
    }
}
