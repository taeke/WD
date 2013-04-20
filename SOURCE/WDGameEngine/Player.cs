//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using WDGameEngine.Enums;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// <inheritDoc/>
    /// </summary>
    public class Player : IPlayer
    {
        /// <summary>
        /// Backing field for CountryNumberOfArmies.
        /// </summary>
        private Dictionary<Country, int> countryNumberOfArmies = new Dictionary<Country, int>();

        /// <summary>
        /// Backing field for Cards.
        /// </summary>
        private List<CardType> cards = new List<CardType>();

        /// <summary>
        /// Backing field for Color.
        /// </summary>
        private Color color;

        /// <summary>
        /// Backing field for Dice.
        /// </summary>
        private Randomize dice;

        /// <summary>
        /// Instantiates a Player class.
        /// </summary>
        /// <param name="color"> The color for this Player.</param>
        /// <param name="dice"> The <see cref="Randomize"/> instance used to roll the dice. </param>
        public Player(Color color, Randomize dice)
        {
            this.color = color;
            this.dice = dice;
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public Dictionary<Country, int> CountryNumberOfArmies
        {
            get
            {
                return this.countryNumberOfArmies;
            }
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public int NumberOfNewArmies { get; set; }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public Color Color 
        {
            get
            {
               return this.color;
            }
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public Randomize Dice
        {
            get 
            { 
                return this.dice; 
            }
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public TurnType TurnType { get; set; }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public List<CardType> Cards
        {
            get 
            { 
                return this.cards; 
            }
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        /// <param name="country"><inheritDoc/></param>
        public List<int> RollTheDices(Country country)
        {
            List<int> result = new List<int>();
            result.Add(this.dice.Next(6));
            if (this.countryNumberOfArmies[country] > 1)
            {
                result.Add(this.dice.Next(6));
            }

            return result;
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        /// <param name="numberOfArmies"><inheritDoc/></param>
        public List<int> RollTheDices(int numberOfArmies)
        {
            List<int> result = new List<int>();
            result.Add(this.dice.Next(6));
            if (numberOfArmies > 1)
            {
                result.Add(this.dice.Next(6));
            }

            if (numberOfArmies > 2)
            {
                result.Add(this.dice.Next(6));
            }

            return result;
        }
    }
}
