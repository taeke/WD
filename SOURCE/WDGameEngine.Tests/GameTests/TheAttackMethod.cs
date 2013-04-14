//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="TheAttackMethod.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    //// TODO : checks for country string empty and wrong. See ThePlaceNewArmiesMethod

    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using WDGameEngine.Enums;
    using WDGameEngine.EventArgs;

    /// <summary>
    /// The tests for the Attack Method.
    /// </summary>
    [TestClass]
    public class TheAttackMethod : GameBaseTests
    {
        /// <summary>
        /// Testing if calling Attack before calling StartGame throws an InvalidOperationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttackBeforeCallingStartGameShouldThrowException()
        {
            // Arange
            this.SettingUpTillNormalGameStart();

            // Act
            this.Game.Attack(string.Empty, string.Empty, 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if Calling Attack throws an InvalidOperationException if the currentGameFase not is Attack.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttackWhileNotInAttackFaseShouldThrowException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.PlaceNewArmies, TurnType.Attack);

            // Act
            this.Game.Attack("4", "8", 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Calling Attack with the attacking <see cref="Country"/> is Null should throw an ArgumentNullException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AttackWithAttackingCountryIsNullShouldThrowAnException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);

            // Act
            this.Game.Attack(null, "8", 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Calling Attack with the deffending <see cref="Country"/> is Null should throw an ArgumentNullException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AttackWithDeffendingCountryIsNullShouldThrowAnException()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);

            // Act
            this.Game.Attack("4", null, 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling Attack calls PlayerReceivesNewCard if the <see cref="IPlayer"/> instance wins.
        /// </summary>
        [TestMethod]
        public void AttackShouldCallPlayerReceivesNewCard()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);
            this.EventHelper.SetupCountForPlayerReceivesNewCard(GameFase.Attack);
            this.EventHelper.SettingUpAttackingAndDefendingDices(true);

            // Act
            this.Game.Attack("4", "8", 1);

            // Assert
            Assert.AreEqual(1, this.EventHelper.PlayerReceivesNewCardCount);
        }

        /// <summary>
        /// Testing if calling Attack twice calls PlayerReceivesNewCard onlt once even if the <see cref="IPlayer"/> instance wins twice.
        /// </summary>
        [TestMethod]
        public void AttackShouldCallCardsAddOnPlayerOnlyOnce()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);
            this.EventHelper.SetupCountForPlayerReceivesNewCard(GameFase.Attack);
            this.EventHelper.SettingUpAttackingAndDefendingDices(true);
            this.Game.Attack("4", "8", 1);

            // Act
            this.Game.Attack("5", "9", 1);

            // Assert
            Assert.AreEqual(1, this.EventHelper.PlayerReceivesNewCardCount);
        }

        /// <summary>
        /// Testing if Calling Attack and the Player wins the <see cref="Country"/> he gets an PlayerCountryArmiesChanged Event for the attack player.
        /// </summary>
        [TestMethod]
        public void AttackShouldCallPlayerCountryArmiesChangedForAttackerIfWin()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);
            this.EventHelper.SettingUpAttackingAndDefendingDices(true);
            bool callFound = false;
            this.Game.PlayerCountryArmiesChanged += delegate(object sender, PlayerCountryArmiesChangedEventArgs e)
            {
                Country deffendingCountry = this.WorldHelper.World.Countries.Find(c => c.Name == "8");
                Country country = this.WorldHelper.World.Countries.Find(w => w.Name == e.Country);
                callFound = callFound || (e.PlayerColor == this.CurrentPlayer.Color && country == deffendingCountry);
            };

            // Act
            this.Game.Attack("4", "8", 1);

            // Assert
            Assert.IsTrue(callFound);
        }

        /// <summary>
        /// Testing if calling Attack call PlayerCountryArmiesChanged for the Country where the attack is launched from.
        /// </summary>
        [TestMethod]
        public void AttackShouldCallPlayerCountryArmiesChangedOnAttack()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);
            this.EventHelper.SettingUpAttackingAndDefendingDices(true);
            bool callFound = false;
            this.Game.PlayerCountryArmiesChanged += delegate(object sender, PlayerCountryArmiesChangedEventArgs e)
            {
                Country country = this.WorldHelper.World.Countries.Find(w => w.Name == e.Country);
                callFound = callFound || (e.PlayerColor == this.CurrentPlayer.Color && country.Name == "4");
            };

            // Act
            this.Game.Attack("4", "8", 1);

            // Assert
            Assert.IsTrue(callFound);
        }

        /// <summary>
        /// Testing if calling attack throws an ArgumentException if the <see cref="Country"/> which attacks and the one thats beeing attacked are 
        /// not neighbours.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AttackShouldThrowExceptionIfNotNeighbours()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);
            this.EventHelper.SettingUpAttackingAndDefendingDices(true);

            // Act
            this.Game.Attack("4", "13", 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling Attack from a <see cref="Country"/> the <see cref="IPlayer"/> does not own throws an ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AttackShouldThrowExceptionIfNotAttackingCountryOwned()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);
            this.EventHelper.SettingUpAttackingAndDefendingDices(true);

            // Act
            this.Game.Attack("11", "7", 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling Attack and attacking a <see cref="Country"/> the <see cref="IPlayer"/> does own throws an ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AttackShouldThrowExceptionIfDeffendingCountryOwned()
        {
            // Arange
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);
            this.EventHelper.SettingUpAttackingAndDefendingDices(true);

            // Act
            this.Game.Attack("4", "5", 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if Calling attack will roll the dice on the attacking <see cref="IPlayer"/>
        /// </summary>
        [TestMethod]
        public void AttackShouldCallRollTheDiceOnAttacker()
        {
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);
            this.EventHelper.SettingUpAttackingAndDefendingDices(true);

            // Act
            this.Game.Attack("4", "8", 1);

            // Assert
            Country deffendingCountry = this.WorldHelper.World.Countries.Find(w => w.Name == "8");
            this.EventHelper.CurrentPlayerHelper.PlayerMock.Verify(p => p.RollTheDices(1));
        }

        /// <summary>
        /// Testing if Calling attack will roll the dice on the defending <see cref="IPlayer"/>
        /// </summary>
        [TestMethod]
        public void AttackShouldCallRollTheDiceOnDefender()
        {
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);
            this.EventHelper.SettingUpAttackingAndDefendingDices(true);

            // Act
            this.Game.Attack("4", "8", 1);

            // Assert
            Country deffendingCountry = this.WorldHelper.World.Countries.Find(w => w.Name == "8");
            this.EventHelper.OtherPlayerHelper.PlayerMock.Verify(p => p.RollTheDices(deffendingCountry));
        }

        /// <summary>
        /// Testing if calling Attack with too many armies will throw an ArgumentException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AttackShouldThrowExceptionIfCalledWithToManyArmies()
        {
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);
            this.EventHelper.SettingUpAttackingAndDefendingDices(true);

            // Act
            Country attackingCountry = this.CurrentPlayer.CountryNumberOfArmies.First(c => c.Key.Name == "5").Key;
            this.Game.Attack("5", "8", this.CurrentPlayer.CountryNumberOfArmies[attackingCountry] + 1);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }

        /// <summary>
        /// Testing if calling Attack with zero armies will call an ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AttackShouldThrowExceptionIfCalledWithZeroArmies()
        {
            this.SettingUpTillGameFase(GameFase.Attack, TurnType.Attack);
            this.EventHelper.SettingUpAttackingAndDefendingDices(true);

            // Act
            this.Game.Attack("5", "8", 0);

            // Assert
            // Assertion is done bij ExpectedException attribute.
        }
    }
}
