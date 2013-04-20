//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="ThePlaceNewArmiesMethod.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WDGameEngine.Enums;

    /// <summary>
    /// The tests for the PlaceNewArmies Method.
    /// </summary>
    [TestClass]
    public class ThePlaceNewArmiesMethod : GameBaseTests
    {
        /// <summary>
        /// Testing if calling PlaceNewArmies before calling StartGame throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PlaceNewArmiesBeforeCallingStartGameShouldThrowException()
        {
            // Arange
            this.SettingUpTillNormalGameStart();

            // Act
            this.Game.PlaceNewArmies(1, string.Empty);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if Calling PlaceNewArmies throws an InvalidOperationException if the currentGameFase not is PlaceNewArmies or PlaceInitialArmies.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PlaceNewArmiesWhileNotInPlaceNewArmiesFaseShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.ExchangeCards, TurnType.Attack); // We are in ExchangeCards GameFase.

            // Act
            this.Game.PlaceNewArmies(1, this.CurrentPlayer.CountryNumberOfArmies.First().Key.Name);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing of Calling PlaceNewArmies with countryname is null throws an argumentnullexception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PlaceNewArmiesWithCountryNameIsNullShouldThrowAnException()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);

            // Act
            this.Game.PlaceNewArmies(1, null); // Call PlaceNewArmies once
            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing of Calling PlaceNewArmies with countryname is string.Empty throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PlaceNewArmiesWithCountryIsEmptyShouldThrowAnException()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);

            // Act
            this.Game.PlaceNewArmies(1, string.Empty); // Call PlaceNewArmies once
            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing of Calling PlaceNewArmies with countryname is not excisting <see cref="Country"/> name throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PlaceNewArmiesWithCountryNotExcistingShouldThrowAnException()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);

            // Act
            this.Game.PlaceNewArmies(1, "bestaatniet"); // Call PlaceNewArmies once

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing of Calling PlaceNewArmies with count smaller then 1 throws an argumentexception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PlaceNewArmiesWithCountSmallerThenOneShouldThrowAnException()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);

            // Act
            this.Game.PlaceNewArmies(0, "1"); // Call PlaceNewArmies once
            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if CallingPlaceNewArmies calls the Event PlayerCountryArmiesChanged.
        /// </summary>
        [TestMethod]
        public void PlaceNewArmiesShouldCallPlayerCountryArmiesChanged()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);
            this.EventHelper.SetupCountForPlayerCountryArmiesChanged(GameFase.PlaceInitialArmies);

            // Act
            this.Game.PlaceNewArmies(1, this.CurrentPlayer.CountryNumberOfArmies.First().Key.Name); // Call PlaceNewArmies once

            // Assert
            Assert.AreEqual(1, this.EventHelper.PlayerCountryArmiesChangedCount);
        }

        /// <summary>
        /// Testing if Placing More Armies then owned trows an ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PlaceNewArmiesWithMoreArmiesThenOwnedShouldThrowException()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);

            // Act
            this.Game.PlaceNewArmies(
                this.CurrentPlayer.NumberOfNewArmies + 1,
                this.CurrentPlayer.CountryNumberOfArmies.First().Key.Name);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if Calling PlaceNewArmies on an Country the player does not own throws a ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PlaceNewArmiesOnANotOwnedCountryShouldThrowException()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);

            // Act
            this.Game.PlaceNewArmies(1, this.EventHelper.OtherPlayerHelper.Player.CountryNumberOfArmies.First().Key.Name);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if Calling PlaceNewArmies will add the right amount of armies on the <see cref="Country"/>.
        /// </summary>
        [TestMethod]
        public void PlaceNewArmiesPlacesTheRightAmountArmies()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);
            int oldValue = this.CurrentPlayer.CountryNumberOfArmies.First().Value;

            // Act
            this.Game.PlaceNewArmies(3, this.CurrentPlayer.CountryNumberOfArmies.First().Key.Name); // Call PlaceNewArmies once

            // Assert
            Assert.AreEqual(oldValue + 3, this.CurrentPlayer.CountryNumberOfArmies.First().Value);
        }

        /// <summary>
        /// Testing if PlaceNewArmies lowers the numberOfNewArmies on the player with the same amount.
        /// </summary>
        [TestMethod]
        public void PlaceNewArmiesShouldLowerTheSameAmountOnNewArmies()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);
            int oldValue = this.EventHelper.CurrentPlayerHelper.Player.NumberOfNewArmies;

            // Act
            this.Game.PlaceNewArmies(3, this.CurrentPlayer.CountryNumberOfArmies.First().Key.Name); // Call PlaceNewArmies once

            // Assert
            Assert.AreEqual(oldValue - 3, this.EventHelper.CurrentPlayerHelper.Player.NumberOfNewArmies);
        }
    }
}
