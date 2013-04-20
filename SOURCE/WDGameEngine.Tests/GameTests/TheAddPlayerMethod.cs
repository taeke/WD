//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="TheAddPlayerMethod.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    using System;
    using System.Drawing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WDGameEngine.Enums;

    /// <summary>
    /// The tests for the AddPlayer Method.
    /// </summary>
    [TestClass]
    public class TheAddPlayerMethod : GameBaseTests
    {
        /// <summary>
        /// Test if adding an <see cref="IPlayer"/> returns a Color.
        /// </summary>
        [TestMethod]
        public void AddPlayerShouldReturnColor()
        {
            // Arange

            // Act
            Color playerColor = this.Game.AddPlayer();

            // Assert
            Assert.IsNotNull(playerColor);
        }

        /// <summary>
        /// Testing if every <see cref="IPlayer"/> gets anonther Color assigned.
        /// </summary>
        [TestMethod]
        public void AddPlayerShouldReturnADifferentColorForEachPlayer()
        {
            // Arange

            // Act
            this.SettingUpTillNormalGameStart();

            // Assert.
            Assert.AreNotEqual(this.PlayerHelpers[0].Player.Color, this.PlayerHelpers[1].Player.Color);
        }

        /// <summary>
        /// Testing if adding to many <see cref="IPlayer"/> throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddingToManyPlayersShouldThrowException()
        {
            // Arange SettingUpTillNormalGameStart allready adds two players.
            this.SettingUpTillNormalGameStart();
            this.Game.AddPlayer();

            // Act
            this.Game.AddPlayer();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if adding an <see cref="IPlayer"/> after the game starts throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddPlayerAfterCallingStartGameShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);

            // Act
            this.Game.AddPlayer();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }
    }
}
