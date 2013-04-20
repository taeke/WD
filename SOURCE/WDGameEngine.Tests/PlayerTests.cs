//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerTests.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// The tests for the Player class.
    /// </summary>
    [TestClass]
    public class PlayerTests
    {
        /// <summary>
        /// The instance of the class under test.
        /// </summary>
        private Player player;

        private Mock<Randomize> randomizeMock = new Mock<Randomize>(MockBehavior.Strict);

        /// <summary>
        /// Initializing for every test.
        /// </summary>
        [TestInitialize]
        public void PlayerInitialize()
        {
            this.randomizeMock.Setup(r => r.Next(It.IsAny<int>())).Returns(1);
            this.player = new Player(Color.Red, this.randomizeMock.Object);
        }

        /// <summary>
        /// The tests for the RollTheDices Method.
        /// </summary>
        [TestClass]
        public class TheRollTheDicesMethod : PlayerTests
        {
            /// <summary>
            /// Testing if RollTheDices returns a list with two int if its called with a Country. Because that means
            /// the <see cref="IPlayer"/> is defending and the player may use at most 2 armies.
            /// </summary>
            [TestMethod]
            public void ShouldReturnTwoDicesIfCalledWithCountry()
            {
                Country country = new Country();
                this.player.CountryNumberOfArmies.Add(country, 5);

                // Act
                List<int> dices = this.player.RollTheDices(country);

                // Assert
                Assert.AreEqual(2, dices.Count);
            }

            /// <summary>
            /// Testing if RollTheDices return a list with three in if it is called with a list of armies. Because that
            /// means the <see cref="IPlayer"/> is attacking and the player may use at most 3 armies.
            /// </summary>
            [TestMethod]
            public void ShouldReturnThreeDicesIfCalledWithArmies()
            {
                // Arange

                // Act
                List<int> dices = this.player.RollTheDices(5);

                // Assert
                Assert.AreEqual(3, dices.Count);
            }
        }
    }
}
