//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="TheStartGameMethod.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using WDGameEngine.Enums;
    using WDGameEngine.EventArgs;

    /// <summary>
    /// The tests for the StartGame Method.
    /// </summary>
    [TestClass]
    public class TheStartGameMethod : GameBaseTests
    {
        /// <summary>
        /// Testing if starting the Game without players will throw a InvalideOperationException. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StartGameWithoutPlayersShouldThrowException()
        {
            // Arange

            // Act
            this.Game.StartGame();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if starting the Game without enough players will throw a InvalideOperationException. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StartGameWithoutEnoughPlayersShouldThrowException()
        {
            // Arange
            this.Game.AddPlayer();

            // Act
            this.Game.StartGame();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if starting the Game calls LoadContinentsAndCountries on the worldMock.
        /// </summary>
        [TestMethod]
        public void StartGameShouldCallLoadContinentsAndCountries()
        {
            // Arange
            this.SettingUpTillNormalGameStart();
            this.EventHelper.CurrentGameFase = GameFase.PlaceInitialArmies;

            // Act
            this.Game.StartGame();

            // Assert
            this.WorldHelper.WorldMock.Verify(w => w.LoadContinentsAndCountries(), Times.Once());
        }

        /// <summary>
        /// Starting the game should throw a InvalidOperationException when LoadContinentsAndCountries didn't load any countries. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StartGameShouldThrowExceptionWhenNoCountries()
        {
            // Arange
            this.WorldHelper.WorldMock.Setup(w => w.Countries).Returns<List<Country>>(null);
            this.Game.AddPlayer();
            this.Game.AddPlayer();

            // Act
            this.Game.StartGame();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Starting the game should throw a InvalidOperationException when LoadContinentsAndCountries didn't load enough countries. 
        /// for all <see cref="IPlayer"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StartGameShouldThrowExceptionWhenNotCountriesForAllPlayers()
        {
            // Arange
            this.WorldHelper.WorldMock.Setup(w => w.Countries).Returns(new List<Country>() { new Country() });
            this.Game.AddPlayer();
            this.Game.AddPlayer();

            // Act
            this.Game.StartGame();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing of starting the game calls the PlayerCountryArmiesChanged event calls for every <see cref="Country"/>.
        /// </summary>
        [TestMethod]
        public void StartGameShouldCallPlayerCountryArmiesChanged()
        {
            // Arange
            this.EventHelper.SetupCountForPlayerCountryArmiesChanged(GameFase.PlaceInitialArmies);

            // Act
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);

            // Assert
            Assert.AreEqual(this.WorldHelper.World.Countries.Count, this.EventHelper.PlayerCountryArmiesChangedCount);
        }

        /// <summary>
        /// Testing if Starting the Game calls the PlayerGetsTurn event only once.
        /// </summary>
        [TestMethod]
        public void StartGameShouldCallPlayerPlayerGetsTurnOnce()
        {
            // Arange
            this.SettingUpTillNormalGameStart();
            this.EventHelper.SetupCountForPlayerGetsTurn(GameFase.PlaceInitialArmies);
            this.EventHelper.CurrentGameFase = GameFase.PlaceInitialArmies;

            // Act
            this.Game.StartGame();

            // Assert
            Assert.AreEqual(1, this.EventHelper.PlayerGetsTurnCount);
        }

        /// <summary>
        /// Testing if Calling StartGame calls the event PlayerReceivesNewArmies.
        /// </summary>
        [TestMethod]
        public void StartGameShouldCallPlayerReceivesNewArmies()
        {
            // Arange
            this.SettingUpTillNormalGameStart();
            this.EventHelper.SetupCountForPlayerReceicesNewArmies(GameFase.PlaceInitialArmies);
            this.EventHelper.CurrentGameFase = GameFase.PlaceInitialArmies;

            // Act
            this.Game.StartGame();

            // Assert 
            Assert.AreEqual(2, this.EventHelper.PlayerReceivesNewArmiesCount);
        }

        /// <summary>
        /// Testing if Calling StartGame puts Game in the PlaceInitialArmies Fase.
        /// </summary>
        [TestMethod]
        public void StartGameShouldSetGameFasePlaceInitialArmies()
        {
            // Act and Arange
            try
            {
                this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
        }

        /// <summary>
        /// testing if Calling  StartGame twice throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        public void StartGameASecondTimeShouldThrowAnException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);

            // Act
            this.Game.StartGame();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }
    }
}
