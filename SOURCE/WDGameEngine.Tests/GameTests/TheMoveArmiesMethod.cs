//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="TheMoveArmiesMethod.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WDGameEngine.Enums;

    /// <summary>
    /// The tests for the MoveArmies Method.
    /// </summary>
    [TestClass]
    public class TheMoveArmiesMethod : GameBaseTests
    {
        /// <summary>
        /// Testing if calling MoveArmies before calling StartGame throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveArmiesBeforeCallingStartGameShouldThrowException()
        {
            // Arange
            this.SettingUpTillNormalGameStart();

            // Act
            this.Game.MoveArmies(string.Empty, string.Empty, 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if Calling MoveArmies throws an InvalidOperationException if the currentGameFase not is MoveArmies.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveArmiesWhileNotInMoveArmiesFaseShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);

            // Act
            this.Game.MoveArmies("1", "2", 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling MoveArmies with countryFrom is Null should throw an ArgumentNullException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MoveArmiesWithCountryFromIsNullShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.MoveArmiesAfterAttack, TurnType.Attack);

            // Act
            this.Game.MoveArmies(null, "2", 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing of Calling MoveArmies with CountryFrom is string.Empty throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveArmiesWithCountryFromIsEmptyShouldThrowAnException()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.MoveArmiesAfterAttack, TurnType.Attack);

            // Act
            this.Game.MoveArmies(string.Empty, "2", 1); // Call MoveArmies once
            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing of Calling MoveArmies with CountryFrom is not excisting <see cref="Country"/> name throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveArmiesWithCountryFromNotExcistingShouldThrowAnException()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.MoveArmiesAfterAttack, TurnType.Attack);

            // Act
            this.Game.MoveArmies("bestaatniet", "2", 1); // Call MoveArmies once
            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling MoveArmies with countryTo is Null should throw an ArgumentNullException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MoveArmiesWithCountryToIsNullShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.MoveArmiesAfterAttack, TurnType.Attack);

            // Act
            this.Game.MoveArmies("1", null, 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing of Calling MoveArmies with CountryTo is string.Empty throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttackWithCountryToIsEmptyShouldThrowAnException()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);

            // Act
            this.Game.MoveArmies("1", string.Empty, 1); // Call MoveArmies once
            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing of Calling MoveArmies with CountryTo is not excisting <see cref="Country"/> name throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttackWithCountryToNotExcistingShouldThrowAnException()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);

            // Act
            this.Game.MoveArmies("1", "bestaatniet", 1); // Call MoveArmies once
            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling MoveArmies from another <see cref="Country"/> where the last attack game from throws an ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveArmiesAfterAttackFromOtherCountryAsLastAttackShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.MoveArmiesAfterAttack, TurnType.Attack);

            // Act
            this.Game.MoveArmies("5", "1", 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// After an attack the amount of armies used in the attack with a maximum of 3 must stay on the attacked <see cref="Country"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveArmiesAfterAttackToManyMoveisShouldThrowException()
        {
            this.SettingUpTillGameFase(GameFase.MoveArmiesAfterAttack, TurnType.Attack);

            // Act
            Country fromCountry = this.WorldHelper.World.Countries.Find(c => c.Name == "2");
            this.Game.MoveArmies("2", "1", this.CurrentPlayer.CountryNumberOfArmies[fromCountry] - 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling MoveArmies to a <see cref="Country"/> which is not an neighbour of the <see cref=""/> where they come 
        /// from throws an ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveArmiesToNotNeighbourShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.MoveArmiesEndOfTurn, TurnType.Attack);

            // Act
            this.Game.MoveArmies("1", "4", 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling MoveArmies with an from <see cref="Country"/> which is not owned throws an ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveArmiesWithFromNotOwnedShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.MoveArmiesEndOfTurn, TurnType.Attack);

            // Act
            this.Game.MoveArmies("6", "1", 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling MoveArmies with an to <see cref="Country"/> which is not owned throws an ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveArmiesWithToNotOwnedShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.MoveArmiesEndOfTurn, TurnType.Attack);

            // Act
            this.Game.MoveArmies("5", "6", 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if you can not call MoveArmies twice in the same fase.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MoveArmiesASecondTimeInTheSameFaseShouldThrowException()
        {
            this.SettingUpTillGameFase(GameFase.MoveArmiesEndOfTurn, TurnType.Attack);
            this.Game.MoveArmies("1", "2", 1);

            // Act
            this.Game.MoveArmies("1", "2", 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if you call MoveArmies with to many armies throws an ArgumentException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoveArmiesWithToManyArmiesShouldThrowException()
        {
            this.SettingUpTillGameFase(GameFase.MoveArmiesEndOfTurn, TurnType.Attack);

            // Act
            Country fromCountry = this.WorldHelper.World.Countries.Find(c => c.Name == "1");
            this.Game.MoveArmies("1", "2", this.CurrentPlayer.CountryNumberOfArmies[fromCountry]);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if you call MoveArmies with zero armies throws an ArgumentException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoveArmiesWithZeroArmiesShouldThrowException()
        {
            this.SettingUpTillGameFase(GameFase.MoveArmiesEndOfTurn, TurnType.Attack);

            // Act
            this.Game.MoveArmies("1", "2", 0);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if you call MoveArmies calls PlayerCountryArmiesChanged
        /// </summary>
        [TestMethod]
        public void MoveArmiesShouldCallPlayerCountryArmiesChanged()
        {
            this.SettingUpTillGameFase(GameFase.MoveArmiesEndOfTurn, TurnType.Attack);
            this.EventHelper.SetupCountForPlayerCountryArmiesChanged(GameFase.MoveArmiesEndOfTurn);

            // Act
            Country fromCountry = this.WorldHelper.World.Countries.Find(c => c.Name == "1");
            this.Game.MoveArmies("1", "2", this.CurrentPlayer.CountryNumberOfArmies[fromCountry] - 1);

            // Assert
            Assert.AreEqual(2, EventHelper.PlayerCountryArmiesChangedCount);
        }
    }
}
