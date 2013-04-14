//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
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
        /// Backing field for NumberOfNewArmies.
        /// </summary>
        private int numberOfNewArmies;

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
        public int NumberOfNewArmies
        {
            get
            {
                return this.numberOfNewArmies;
            }

            set
            {
                this.numberOfNewArmies = value;
            }
        }

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
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        /// <param name="numberOfArmies"><inheritDoc/></param>
        public List<int> RollTheDices(int numberOfArmies)
        {
            throw new System.NotImplementedException();
        }
    }
}
