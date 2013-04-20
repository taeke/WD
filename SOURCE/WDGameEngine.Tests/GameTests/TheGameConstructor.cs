//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="TheGameConstructor.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// Testing the constructor of the Game Class.
    /// </summary>
    [TestClass]
    public class TheGameConstructor
    {
        /// <summary>
        /// Testing if the constructor throws an ArgumentNullException when world parameter is Null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowExceptionWhenIWorldIsNull()
        {
            // Arange

            // Act
            Game game = new Game(
                null,
                new List<Color>() { Color.Red, Color.Blue },
                c => { return new PlayerHelper(c).Player; },
                new Config(),
                new Mock<Randomize>(MockBehavior.Strict).Object);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if the constructor throws an ArgumentNullException when playerColors parameter is Null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowExceptionWhenPlayerColorsIsNull()
        {
            // Arange
            Mock<IWorld> worldMock = new Mock<IWorld>(MockBehavior.Strict);

            // Act
            Game game = new Game(
                worldMock.Object,
                null,
                c => { return new PlayerHelper(c).Player; },
                new Config(),
                new Mock<Randomize>(MockBehavior.Strict).Object);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if the constructor throws an ArgumentNullException when playerFactory parameter is Null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowExceptionWhenPlayerFactoryIsNull()
        {
            // Arange
            Mock<IWorld> worldMock = new Mock<IWorld>(MockBehavior.Strict);

            // Act
            Game game = new Game(
                worldMock.Object, 
                new List<Color>() { Color.Red, Color.Blue }, 
                null,
                new Config(),
                new Mock<Randomize>(MockBehavior.Strict).Object);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if the constructor throws an ArgumentNullException when config parameter is Null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowExceptionWhenConfigIsNull()
        {
            // Arange
            Mock<IWorld> worldMock = new Mock<IWorld>(MockBehavior.Strict);

            // Act
            Game game = new Game(
                worldMock.Object,
                new List<Color>() { Color.Red, Color.Blue },
                c => { return new PlayerHelper(c).Player; },
                null,
                new Mock<Randomize>(MockBehavior.Strict).Object);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if the constructor throws an ArgumentNullException when random parameter is Null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowExceptionWhenRandomIsNull()
        {
            // Arange
            Mock<IWorld> worldMock = new Mock<IWorld>(MockBehavior.Strict);

            // Act
            Game game = new Game(
                worldMock.Object,
                new List<Color>() { Color.Red, Color.Blue },
                c => { return new PlayerHelper(c).Player; },
                new Config(),
                null);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if the constructor throws an ArgumentException when the number of colors in playerColors is less then the minimum number of players.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenNumberOfColorsIsLessThenMinimumPlayers()
        {
            Mock<IWorld> worldMock = new Mock<IWorld>(MockBehavior.Strict);
            Config config = new Config();
            config.MinimumNumberPlayers = 2;
            Game game = new Game(
                worldMock.Object,
                new List<Color>() { Color.Red },
                c => { return new PlayerHelper(c).Player; },
               config,
                new Mock<Randomize>(MockBehavior.Strict).Object);
        }
    }
}
