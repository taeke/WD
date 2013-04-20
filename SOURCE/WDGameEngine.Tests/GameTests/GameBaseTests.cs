//-------------------------------------------------------------------------------------------------
// <copyright file="GameBaseTests.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//--------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using WDGameEngine.Enums;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// Unit tests for the <see cref="Game"/> class. This is a Base class which will setup things for all the tests
    /// and has some methods for setting up things which are used by a lot of the tests. Every Method in the <see cref="Game"/>
    /// class will have his own test class which inherits from this class. Except for the constructor which will stand on his own
    /// and will not inherit from this class.
    /// </summary>
    [TestClass]
    public class GameBaseTests
    {
        /// <summary>
        /// Backing field for PlayerHelpers.
        /// </summary>
        private List<PlayerHelper> playerHelpers;

        /// <summary>
        /// Backing field for EventHelper.
        /// </summary>
        private EventHelper eventHelper;

        /// <summary>
        /// Backing field for WorldHelper.
        /// </summary>
        private WorldHelper worldHelper;

        /// <summary>
        /// Backing field for ConfigHelper.
        /// </summary>
        private Config config;

        /// <summary>
        /// The instance of <see cref="RandomHelper"/>.
        /// </summary>
        private RandomHelper randomHelper;

        /// <summary>
        /// Backing field for Game.
        /// </summary>
        private Game game;

        /// <summary>
        /// The instance of the class under test.
        /// </summary>
        protected Game Game
        {
            get
            {
                return this.game;
            }
        }

        /// <summary>
        /// We use two players for the tests.
        /// </summary>
        protected List<PlayerHelper> PlayerHelpers
        {
            get
            {
                return this.playerHelpers;
            }
        }

        /// <summary>
        /// The instance of <see cref="EventHelper"/>.
        /// </summary>
        protected EventHelper EventHelper
        {
            get
            {
                return this.eventHelper;
            }
        }

        /// <summary>
        /// The instance of <see cref="WorldHelper"/>.
        /// </summary>
        protected WorldHelper WorldHelper
        {
            get
            {
                return this.worldHelper;
            }
        }

        /// <summary>
        /// Direct acces to this.EventHelper.CurrentPlayerHelper.Player because it is used a lot.
        /// </summary>
        protected IPlayer CurrentPlayer
        {
            get
            {
                return this.EventHelper.CurrentPlayerHelper.Player;
            }
        }

        /// <summary>
        /// Initializing for every test.
        /// </summary>
        [TestInitialize]
        public void GameInitialize()
        {
            this.worldHelper = new WorldHelper();
            this.playerHelpers = new List<PlayerHelper>();
            this.config = new Config();
            this.config.MinimumNumberPlayers = 2;
            Dictionary<int, int> numberOfPlayersIntialArmies = new Dictionary<int, int>();
            numberOfPlayersIntialArmies.Add(2, 40);
            this.config.NumberOfPlayersIntialArmies = numberOfPlayersIntialArmies;
            this.config.MaximumCards = 5;
            this.randomHelper = new RandomHelper();
            this.game = this.BuildTestGame();
            this.eventHelper = new EventHelper(this.game, this.playerHelpers, this.worldHelper.World);
        }

        /// <summary>
        /// For some tests we need to bring the <see cref="Game"/> instance in a specific state (<see cref="GameFase"/>
        /// before  we can start the real test.
        /// </summary>
        /// <param name="gameFase"> The <see cref="GameFase"/> where we want to go to. </param>
        /// <param name="turnType"> The <see cref="TurnType"/> which should be used. </param>
        protected void SettingUpTillGameFase(GameFase gameFase, TurnType turnType)
        {
            this.SettingUpTillNormalGameStart();
            if (gameFase == GameFase.None)
            {
                return;
            }

            if (gameFase == GameFase.PlaceInitialArmies)
            {
                this.eventHelper.CurrentGameFase = GameFase.PlaceInitialArmies;
                this.game.StartGame();
                return;
            }

            this.eventHelper.SetGotoChooseTurnType();
            this.eventHelper.CurrentGameFase = GameFase.ChooseTurnType;
            this.game.StartGame();
            if (gameFase == GameFase.ChooseTurnType)
            {
                return;
            }

            this.game.ChooseTurnType(turnType);
            this.game.GoToNextFase();
            this.eventHelper.CurrentGameFase = GameFase.ExchangeCards;
            if (gameFase == GameFase.ExchangeCards)
            {
                return;
            }

            this.eventHelper.CurrentGameFase = GameFase.PlaceNewArmies;
            this.game.GoToNextFase();
            if (gameFase == GameFase.PlaceNewArmies)
            {
                return;
            }

            this.game.PlaceNewArmies(this.CurrentPlayer.NumberOfNewArmies, "1");
            this.eventHelper.CurrentGameFase = GameFase.Attack;
            this.game.GoToNextFase();
            if (gameFase == GameFase.Attack)
            {
                return;
            }

            this.EventHelper.SettingUpAttackingAndDefendingDices(true);
            this.game.Attack("1", "2", 24);
            this.eventHelper.CurrentGameFase = GameFase.MoveArmiesAfterAttack;
            this.game.GoToNextFase();
            if (gameFase == GameFase.MoveArmiesAfterAttack)
            {
                return;
            }

            this.eventHelper.CurrentGameFase = GameFase.Attack;
            this.game.GoToNextFase(); // Back to Attack
            this.eventHelper.CurrentGameFase = GameFase.MoveArmiesEndOfTurn;
            this.game.GoToNextFase();
            if (gameFase == GameFase.MoveArmiesEndOfTurn)
            {
                return;
            }
        }

        /// <summary>
        /// For some tests we need to bring the <see cref="Game"/> instance in the end of game state
        /// before  we can start the real test.
        /// </summary>
        protected void SettingUpTillEndOfGame()
        {
            this.SettingUpTillGameFase(GameFase.MoveArmiesAfterAttack, TurnType.Attack);
            this.game.GoToNextFase(); // Attack
            this.eventHelper.CurrentGameFase = GameFase.Attack;
            Country country;
            country = this.eventHelper.CurrentPlayerHelper.CountryNumberOfArmies.First(c => c.Key.Name == "2").Key;
            this.game.Attack("2", "3", this.CurrentPlayer.CountryNumberOfArmies[country] - 3);
            this.game.GoToNextFase(); // MoveArmiesAfterAttack
            this.game.GoToNextFase(); // Attack
            country = this.eventHelper.CurrentPlayerHelper.CountryNumberOfArmies.First(c => c.Key.Name == "3").Key;
            this.game.Attack("3", "7", this.CurrentPlayer.CountryNumberOfArmies[country] - 3);
            this.game.GoToNextFase(); // MoveArmiesAfterAttack
            this.game.GoToNextFase(); // Attack
            country = this.eventHelper.CurrentPlayerHelper.CountryNumberOfArmies.First(c => c.Key.Name == "7").Key;
            this.game.Attack("7", "6", this.CurrentPlayer.CountryNumberOfArmies[country] - 3);
        }

        /// <summary>
        /// Adding <see cref="IPlayer"/> instances.
        /// </summary>
        protected void SettingUpTillNormalGameStart()
        {
            this.game.AddPlayer();
            this.game.AddPlayer();
        }

        /// <summary>
        /// Creating the <see cref="Game"/> instance for the tests and giving it all the Mocks it needs.
        /// </summary>
        /// <returns> A new <see cref="Game"/> instances. </returns>
        protected Game BuildTestGame()
        {
            return new Game(
                this.worldHelper.World,
                new List<Color> { Color.Red, Color.Blue, Color.Green },
                c =>
                {
                    PlayerHelper playerHelper = new PlayerHelper(c);
                    this.playerHelpers.Add(playerHelper);
                    return playerHelper.Player;
                },
                this.config,
                this.randomHelper.Random);
        }
    }
}
