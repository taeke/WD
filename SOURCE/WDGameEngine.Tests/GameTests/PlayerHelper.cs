//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerHelper.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Moq;
    using WDGameEngine.Enums;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// We need a Mock of <see cref="IPlayer"/> for a lot of the tests. This class is a wrapper for this Mock.
    /// So we have one place for the variables we use for the setup of this Mock. Because we can't reach the internal list
    /// of <see cref="IPlayer"/> in the <see cref="Game"/> class we can also use these variables if we need information of
    /// the internals of <see cref="Game"/> in the tests. And for that we need to record all the changes the internals make
    /// and keep thes instances up to date. <see cref="Game"/> will inform us off it's internal state changes
    /// with a couple of events and we need to update these mock instances if these events are called to make this possible. 
    /// This class also helps in controlling a specific state of the Mock which in normal game play whould be more randomly.
    /// </summary>
    public class PlayerHelper
    {
        /// <summary>
        /// Backing field for PlayerMock.
        /// </summary>
        private Mock<IPlayer> playerMock = new Mock<IPlayer>(MockBehavior.Strict);

        /// <summary>
        /// Backing field for NumberOfNewArmies.
        /// </summary>
        private int numberOfNewArmies;

        /// <summary>
        /// Backing field for CountryNumberOfArmies.
        /// </summary>
        private Dictionary<Country, int> countryNumberOfArmies = new Dictionary<Country, int>();

        /// <summary>
        /// Backing field for Cards.
        /// </summary>
        private List<CardType> cards = new List<CardType>();

        /// <summary>
        /// Creates an instance of the Helper class and makes the setup for the mock.
        /// </summary>
        /// <param name="c"> The Color for the <see cref="IPlayer"/> instance. </param>
        public PlayerHelper(Color c)
        {
            this.playerMock.Setup(p => p.Color).Returns(c);
            this.playerMock.Setup(p => p.NumberOfNewArmies).Returns(this.numberOfNewArmies);
            this.playerMock.SetupProperty(p => p.NumberOfNewArmies);
            this.playerMock.SetupProperty(p => p.TurnType);
            this.playerMock.Setup(p => p.CountryNumberOfArmies).Returns(this.CountryNumberOfArmies);
            this.playerMock.Setup(p => p.Cards).Returns(this.cards);
        }

        /// <summary>
        /// Keep track of changes in the number of new armies the <see cref="IPlayer"/> receives.
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
        /// Keep track of changes in the number of armies a <see cref="IPlayer"/> got on each of his <see cref="Country"/>.
        /// </summary>
        public Dictionary<Country, int> CountryNumberOfArmies
        {
            get
            {
                return this.countryNumberOfArmies;
            }
        }

        /// <summary>
        /// The mock for <see cref="IPlayer"/>.
        /// </summary>
        public Mock<IPlayer> PlayerMock
        {
            get
            {
                return this.playerMock;
            }
        }

        /// <summary>
        /// The mocked <see cref="IPlayer"/> object.
        /// </summary>
        public IPlayer Player
        {
            get
            {
                return this.playerMock.Object;
            }
        }

        /// <summary>
        /// The list of <see cref="CardType"/> the <see cref="IPlayer"/> owns.
        /// </summary>
        public List<CardType> Cards
        {
            get
            {
                return this.cards;
            }

            set
            {
                this.cards = value;
            }
        }

        /// <summary>
        /// Mocks the Dices the <see cref="IPlayer"/> throws. Would normally be random but we want fixed values for the tests.
        /// </summary>
        /// <param name="number"> the number for all the dice. </param>
        /// <param name="isAttacker"> 3 dices for an attacker 2 for a defender. </param>
        public void SetDices(int number, bool isAttacker)
        {
            List<int> dices = isAttacker ? new List<int>() { number, number, number } : new List<int>() { number, number };
            if (isAttacker)
            {
                this.playerMock.Setup(p => p.RollTheDices(It.IsAny<int>())).Returns(dices);
            }
            else
            {
                this.playerMock.Setup(p => p.RollTheDices(It.IsAny<Country>())).Returns(dices);
            }
        }
    }
}
