//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="TheChooseTurnTypeMethod.cs">
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
    /// The tests for the ChooseTurnType Method.
    /// </summary>
    [TestClass]
    public class TheChooseTurnTypeMethod : GameBaseTests
    {
        /// <summary>
        /// Testing if calling ChooseTurnType before calling StartGame throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChooseTurnTypeBeforeCallingStartGameShouldThrowException()
        {
            // Arange
            this.SettingUpTillNormalGameStart();

            // Act
            this.Game.ChooseTurnType(TurnType.Attack);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if Calling ChooseTurnType throws an InvalidOperationException if the currentGameFase not is ChooseTurnType.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChooseTurnTypeWhileNotInChooseTurnTypeFaseShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);

            // Act 
            this.Game.ChooseTurnType(TurnType.Attack);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if callling ChooseTurnType throws an ArgumentOutOfRangeException if the TurnType enum is out of range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ChooseTurnTypeWithAnOutOfRangeValueShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.ChooseTurnType, TurnType.Attack); // We are in ChooseTurnType GameFase.

            // Act
            this.Game.ChooseTurnType((TurnType)100);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }
    }
}
