//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="TheExchangeCardsMethod.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WDGameEngine.Enums;

    /// <summary>
    /// The tests for the ExchangeCards Method.
    /// </summary>
    [TestClass]
    public class TheExchangeCardsMethod : GameBaseTests
    {
        /// <summary>
        /// Testing if calling ExchangeCards before calling StartGame throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExchangeCardsBeforeCallingStartGameShouldThrowException()
        {
            // Arange
            this.SettingUpTillNormalGameStart();

            // Act
            this.Game.ExchangeCards(new List<CardType>());

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if Calling ExchangeCards throws an InvalidOperationException if the currentGameFase not is ExchangeCards.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ExchangeCardsWhileNotInExchangeCardsFaseShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.ChooseTurnType, TurnType.Attack);

            // Act
            this.Game.ExchangeCards(new List<CardType>() { CardType.Artillery, CardType.Artillery, CardType.Artillery });

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling ExchangeCards with a cards parameter null throws an ArgumentNullException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExchangeCardsWhithCardsIsNullShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.ExchangeCards, TurnType.Attack);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);

            // Act
            this.Game.ExchangeCards(null);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling ExchangeCards with cards.Count less then three throws an ArgumentOutOfRangeException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExchangingLessThenThreeCardsShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.ExchangeCards, TurnType.Attack);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);

            // Act
            this.Game.ExchangeCards(new List<CardType>() { CardType.Artillery, CardType.Artillery });

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling ExchangeCards with cards.Count more then three throws an ArgumentOutOfRangeException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExchangingMoreThenThreeCardsShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.ExchangeCards, TurnType.Attack);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);

            // Act
            this.Game.ExchangeCards(new List<CardType>() { CardType.Artillery, CardType.Artillery, CardType.Artillery, CardType.Artillery });

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling ExchangeCards with cards the <see cref="IPlayers"/> does not own throws an ArgumentOutOfRangeException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExchangingCardsThePlayerDoesNotOwnShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.ExchangeCards, TurnType.Attack);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);

            // Act
            this.Game.ExchangeCards(new List<CardType>() { CardType.Cavalry, CardType.Cavalry, CardType.Cavalry });

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling ExchangeCards with cards the <see cref="IPlayers"/> does not own throws an ArgumentOutOfRangeException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExchangingCardsThePlayerDoesNotOwnAllDiffShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.ExchangeCards, TurnType.Attack);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);

            // Act
            this.Game.ExchangeCards(new List<CardType>() { CardType.Cavalry, CardType.Artillery, CardType.Infantry });

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling ExchangeCards with a wrong cards collection throws an ArgumentOutOfRangeException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExchangeCardsWithAWrongCombinationOfCardsShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.ExchangeCards, TurnType.Attack);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);

            // Act
            this.Game.ExchangeCards(new List<CardType>() { CardType.Artillery, CardType.Artillery, CardType.Cavalry });

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if Exchanges a right combination of cards calls the PlayerReceivesNewArmies Event.
        /// </summary>
        [TestMethod]
        public void ExchangeCardsWithARightCombinationOfCardsShouldCallPlayerReceivesNewArmies()
        {
            // Arange
            this.EventHelper.SetupCountForPlayerReceicesNewArmies(GameFase.ExchangeCards);
            this.SettingUpTillGameFase(GameFase.ExchangeCards, TurnType.Attack);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);

            // Act
            this.Game.ExchangeCards(new List<CardType>() { CardType.Artillery, CardType.Artillery, CardType.Artillery });

            // Assert
            Assert.AreEqual(1, this.EventHelper.PlayerReceivesNewArmiesCount);
        }

        /// <summary>
        /// Testing if exchangecards twice throws an exception. An <see cref="IPlayer"/> can have a maximum of 5 <see cref="CardType"/>
        /// so after exchanging 3 of them he does not own enough to exchange another 3.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExchangeCardsTwiceShouldThrowException()
        {
            // Arange
            this.EventHelper.SetupCountForPlayerReceicesNewArmies(GameFase.ExchangeCards);
            this.SettingUpTillGameFase(GameFase.ExchangeCards, TurnType.Attack);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.Game.ExchangeCards(new List<CardType>() { CardType.Artillery, CardType.Artillery, CardType.Artillery });

            // Act
            this.Game.ExchangeCards(new List<CardType>() { CardType.Artillery, CardType.Artillery, CardType.Artillery });

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }
    }
}
