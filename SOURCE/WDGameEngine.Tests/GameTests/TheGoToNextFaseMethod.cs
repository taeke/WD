//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="TheGoToNextFaseMethod.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using WDGameEngine.Enums;

    /// <summary>
    /// The tests for the GoToNextFase Method.
    /// </summary>
    [TestClass]
    public class TheGoToNextFaseMethod : GameBaseTests
    {
        /// <summary>
        /// Testing if calling GoToNextFase before calling StartGame throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GoToNextFaseBeforeCallingStartGameShouldThrowException()
        {
            // Arange
            this.SettingUpTillNormalGameStart();

            // Act
            this.Game.GoToNextFase();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        //// PlaceInitialArmies => ChooseTurnType

        /// <summary>
        /// Testing if GoToNextFase will trigger a NewArmiesReceived when you call GoToNextFase and the 
        /// currentGameFase changes to ChooseTurnType. Will be one call more then just StartGame.
        /// </summary>
        [TestMethod]
        public void TriggersNewArmiesReceivedWhenGoingToChooseTurnTypeFase()
        {
            // Arange
            this.EventHelper.SetupCountForPlayerReceicesNewArmies(GameFase.ChooseTurnType);

            // Act
            this.SettingUpTillGameFase(GameFase.ChooseTurnType, TurnType.Attack);

            // Assert we can't test for GameFase.ChoosTurnType because this gets set after the
            // receivesNewArmies event. So we have to count the first serie also which are the intial
            // armies and after they are placed on a Country the 0.
            Assert.AreEqual(7, this.EventHelper.PlayerReceivesNewArmiesCount);
        }

        //// ChooseTurnType => ExchangeCards
        
        /// <summary>
        /// Testing if GoToNextFase calls PlayerReceivesNewArmies when TurnType is GetArmiesForCountries
        /// </summary>
        [TestMethod]
        public void GoToNextFaseInChooseTurnTypeShouldCallPlayerReceivesNewArmiesIfGetArmiesForCountries()
        {
            // Arrange
            this.EventHelper.SetupCountForPlayerReceicesNewArmies(GameFase.ChooseTurnType);
            this.SettingUpTillGameFase(GameFase.ChooseTurnType, TurnType.Attack);
            this.Game.ChooseTurnType(TurnType.GetArmiesForCountries);

            // Act
            this.Game.GoToNextFase();

            // Assert PlaceInitialArmies for both twice => 4. Change current player place the initial armies both => 2 => Chooseturntype 
            // continents => 1 GetArmiesForCountries => 1 
            Assert.AreEqual(8, this.EventHelper.PlayerReceivesNewArmiesCount);
        }

        /// <summary>
        /// Testing if GoToNextFase Not calls PlayerReceivesNewArmies when TurnType is Attack
        /// </summary>
        [TestMethod]
        public void GoToNextFaseInChooseTurnTypeShouldNotCallPlayerReceivesNewArmiesIfAttack()
        {
            // Arrange
            this.EventHelper.SetupCountForPlayerReceicesNewArmies(GameFase.ChooseTurnType);
            this.SettingUpTillGameFase(GameFase.ChooseTurnType, TurnType.Attack);
            this.Game.ChooseTurnType(TurnType.Attack);

            // Act
            this.Game.GoToNextFase();

            // Assert PlaceInitialArmies for both twice => 4. Change current player place the initial armies both => 2 => Chooseturntype 
            // continents => 1 attack => 0 
            Assert.AreEqual(7, this.EventHelper.PlayerReceivesNewArmiesCount);
        }

        /// <summary>
        /// Testing if GoToNextFase throws an InvalidOperationException if the TurnType still is On NotChosen.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GoToNextFaseInChooseTurnTypeShouldThrowExceptionIfNotChosen()
        {
            // Arrange
            this.EventHelper.SetupCountForPlayerReceicesNewArmies(GameFase.ExchangeCards);
            this.SettingUpTillGameFase(GameFase.ChooseTurnType, TurnType.Attack);

            // Act
            this.Game.GoToNextFase();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        //// PlaceNewArmies => Attack
        
        /// <summary>
        /// Testing if GoToNextFase throws an InvalidOperationException when you go to Attack Fase but not
        /// all New armies are placed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GoToNextFaseThrowsExceptionWhenGoingToAttackFaseNotAllNewArmiesPlaced()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.PlaceNewArmies, TurnType.Attack);

            // Act
            this.Game.GoToNextFase();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if GoToNextFase returns Attack when in PlaceNewArmies <see cref="GameFace"/> and <see cref="TurnType"/> is Attack.
        /// </summary>
        [TestMethod]
        public void GoToNextFaseShouldGoToAttackWhenTurnTypeIsAttack()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.PlaceNewArmies, TurnType.Attack);
            this.Game.PlaceNewArmies(
                this.CurrentPlayer.NumberOfNewArmies,
                this.CurrentPlayer.CountryNumberOfArmies.First().Key.Name);

            // Act
            this.Game.GoToNextFase();

            // Assert if we can attack without exception we are in Attack fase
            this.EventHelper.SettingUpAttackingAndDefendingDices(true);
            this.Game.Attack("1", "2", 2);
        }

        //// PlaceNewArmies => ChooseTurnType

        /// <summary>
        /// Testing if GoToNextFase returns ChooseTurnType when in PlaceNewArmies <see cref="GameFace"/> and <see cref="TurnType"/> is GetArmiesForCountries.
        /// </summary>
        [TestMethod]
        public void GoToNextFaseShouldGoToChooseTurnTypeWhenTurnTypeIsGetArmiesForCountries()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.PlaceNewArmies, TurnType.GetArmiesForCountries);
            this.Game.PlaceNewArmies(
                this.CurrentPlayer.NumberOfNewArmies,
                this.CurrentPlayer.CountryNumberOfArmies.First().Key.Name);

            // Act
            this.Game.GoToNextFase();

            // Assert if we can ChooseTurnType without exception we are in ChooseTurnType fase.
            this.Game.ChooseTurnType(TurnType.GetArmiesForCountries);
        }

        /// <summary>
        /// Testing if GoToNextFase calls PlayerGetsTurn when in PlaceNewArmies <see cref="GameFace"/> and <see cref="TurnType"/> is GetArmiesForCountries.
        /// </summary>
        [TestMethod]
        public void GoToNextFaseShouldCallPlayerGetsTurnWhenTurnTypeIsGetArmiesForCountries()
        {
            // Arange
            this.EventHelper.SetupCountForPlayerGetsTurn(GameFase.PlaceNewArmies);
            this.SettingUpTillGameFase(GameFase.PlaceNewArmies, TurnType.GetArmiesForCountries);
            this.Game.PlaceNewArmies(
                this.CurrentPlayer.NumberOfNewArmies,
                this.CurrentPlayer.CountryNumberOfArmies.First().Key.Name);

            // Act
            this.Game.GoToNextFase();

            // Assert
            Assert.AreEqual(1, this.EventHelper.PlayerGetsTurnCount);
        }

        //// PlaceInitialArmies => PlaceInitialArmies

        /// <summary>
        /// Testing if calling GoToNextFase throws an InvalidOperationException when not all new armies are placed in 
        /// PlaceInitialArmies fase.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GoToNextFaseThrowsExceptionWhenLeavingPlaceInitialArmiesNotAllNewArmiesPlaced()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.PlaceInitialArmies, TurnType.Attack);

            // Act
            this.Game.GoToNextFase();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        //// Attack => MoveArmiesAfterAttack

        /// <summary>
        /// Testing if Calling GoToNextFase goes from Attack to MoveArmiesAfterAttack if the player won.
        /// </summary>
        [TestMethod]
        public void GoToNextFaseShouldGoFromAttackToMoveArmiesAfterAttackIfPlayerWon()
        {
            // Arrange and Act
            this.SettingUpTillGameFase(GameFase.MoveArmiesAfterAttack, TurnType.Attack);

            // Assert
            Assert.AreEqual(GameFase.MoveArmiesAfterAttack, this.EventHelper.CurrentGameFase);
        }

        //// Attack => MoveArmiesEndOfTurn

        /// <summary>
        /// Testing is calling GoToNextFase goes from Attack to MoveArmiesEndOfTurn if the player does not attack anymore. 
        /// So he lost his last attack or he just called GoToNextFase and got from MoveArmiesAfterAttack to Attack fase and
        /// did not attack again.
        /// </summary>
        [TestMethod]
        public void GoToNextFaseShouldGoFromAttackToMoveArmiesEndOfTurnIfPlayerDoesNotAttackAnymore()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);

            // Act
            this.Game.GoToNextFase();

            // Assert if we can MoveArmies without exception we are in MoveArmies fase.
            this.Game.MoveArmies("1", "5", 2);
        }

        //// ExchangeCards => PlaceNewArmies

        /// <summary>
        /// Testing if Calling GotToNextFase while in ExchangesCards fase  will go to PlaceNewArmies face after exchanging cards. The test
        /// for the situation without excahnging cards wil be done by SettingUpTillGameFase.
        /// </summary>
        [TestMethod]
        public void GoToNextFaseShouldGoFromExhangeCardsToPlaceNewArmies()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.ExchangeCards, TurnType.Attack);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.Game.ExchangeCards(new List<CardType>() { CardType.Artillery, CardType.Artillery, CardType.Artillery });

            // Act
            this.Game.GoToNextFase();

            // Assert if we can PlaceNewArmies without exception we are in PlaceNewArmies fase. 
            this.Game.PlaceNewArmies(1, "1");
        }

        /// <summary>
        /// Testing if calling GoToNextFase while the <see cref="IPlayer"/> holds to many <see cref="CardType"/> throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GoToNextFaseInExchangeCardsFaseWithToManyCardsThrowsException()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.ExchangeCards, TurnType.Attack);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);
            this.EventHelper.CurrentPlayerHelper.Cards.Add(CardType.Artillery);

            // Act
            this.Game.GoToNextFase();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        //// MoveArmiesEndOfTurn => ChooseTurnType

        /// <summary>
        /// Testing if Calling GoToNextFase in MoveArmiesEndOfTurnFase calls PlayerGetsTurn.
        /// </summary>
        [TestMethod]
        public void GoToNextFaseInMoveArmiesEndOfTurnFaseShouldCallPlayerGetsTurn()
        {
            // Arrange
            this.EventHelper.SetupCountForPlayerGetsTurn(GameFase.MoveArmiesEndOfTurn);
            this.SettingUpTillGameFase(GameFase.MoveArmiesEndOfTurn, TurnType.Attack);

            // Act
            this.Game.GoToNextFase();

            // Assert
            Assert.AreEqual(1, this.EventHelper.PlayerGetsTurnCount);
        }

        /// <summary>
        /// Testing if calling GoToNextFase in MoveArmiesEndOfTurnFase goes to ChooseTurnType fase.
        /// </summary>
        [TestMethod]
        public void GoToNextFaseInMoveArmiesEndOfTurnShouldGoToChooseTurnType()
        {
            // Arrange
            this.SettingUpTillGameFase(GameFase.MoveArmiesEndOfTurn, TurnType.Attack);

            // Act
            this.Game.GoToNextFase();

            // Assert if we can ChooseTurnType without exception we are in ChooseTurnType fase. 
            this.Game.ChooseTurnType(TurnType.GetArmiesForCountries);
        }

        /// <summary>
        /// Testing if calling GoToNextFase after the game is ended throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GoToNextFaseAfterGameIsEndedShouldThrowException()
        { 
            // Arrange
            this.SettingUpTillEndOfGame();

            // Act
            this.Game.GoToNextFase();

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        //// Testing if GoToNextFase in MoveArmiesAfterAttack goes to Attack fase is done by SettingUpTillGameFase.
    }
}
