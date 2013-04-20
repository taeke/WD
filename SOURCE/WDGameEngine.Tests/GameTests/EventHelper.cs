//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="EventHelper.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Tests.GameTests
{
    using System.Collections.Generic;
    using System.Linq;
    using WDGameEngine.Enums;
    using WDGameEngine.EventArgs;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// The <see cref="Game"/> class uses a couple of Events to inform the outside world of his state. This class is to test
    /// these state changes, to update the mocked <see cref="IPlayer"/> instances and to start actions depending on these events.
    /// </summary>
    public class EventHelper
    {
        /// <summary>
        /// The current <see cref="Game"/> instance.
        /// </summary>
        private Game game;

        /// <summary>
        /// The current list of <see cref="IPlayer"/> instances.
        /// </summary>
        private List<PlayerHelper> playerHelpers;

        /// <summary>
        /// The current IWorld instance.
        /// </summary>
        private IWorld world;

        /// <summary>
        /// Backing field for CurrentGameFase.
        /// </summary>
        private GameFase currentGameFase;

        /// <summary>
        /// Backing field for CurrentPlayerHelper.
        /// </summary>
        private PlayerHelper currentPlayerHelper;

        /// <summary>
        /// Backing field for OtherPlayerHelper.
        /// </summary>
        private PlayerHelper otherPlayerHelper;

        /// <summary>
        /// Backing field for PlayerReceivesNewArmiesCount.
        /// </summary>
        private int playerReceivesNewArmiesCount = 0;

        /// <summary>
        /// Backing field for PlayerCountryArmiesChangedCount.
        /// </summary>
        private int playerCountryArmiesChangedCount = 0;

        /// <summary>
        /// Backing field for PlayerGetsTurnCount.
        /// </summary>
        private int playerGetsTurnCount = 0;

        /// <summary>
        /// Backing field for PlayerReceivesNewCardCount.
        /// </summary>
        private int playerReceivesNewCardCount = 0;

        /// <summary>
        /// Backing field for PlayerHasWonCount
        /// </summary>
        private int playerHasWonCount = 0;

        /// <summary>
        /// Creates a instances of this Helper class and creates the setups.
        /// </summary>
        /// <param name="game"> The current <see cref="Game"/> instance. </param>
        /// <param name="playerHelpers"> The current list of <see cref="PlayerHelper"/> instances. </param>
        /// <param name="world"> The current <see cref="IWorld"/> instance. </param>
        public EventHelper(Game game, List<PlayerHelper> playerHelpers, IWorld world)
        {
            this.world = world;
            this.game = game;
            this.playerHelpers = playerHelpers;
            this.game.PlayerReceivesNewArmies += this.Game_PlayerReceivesNewArmies;
            this.game.PlayerCountryArmiesChanged += this.Game_PlayerCountryArmiesChanged;
            this.game.PlayerGetsTurn += this.Game_PlayerGetsTurn;
            this.currentGameFase = GameFase.None;
        }

        /// <summary>
        /// The <see cref="IPlayer"/> instance from the last PlayerGetsTurn call.
        /// </summary>
        public PlayerHelper CurrentPlayerHelper
        {
            get
            {
                return this.currentPlayerHelper;
            }
        }

        /// <summary>
        /// For the tests we use only two <see cref="IPlayer"/> instances. So this is THE other <see cref="IPlayer"/> 
        /// the opposite of CurrentPlayerHelper.
        /// </summary>
        public PlayerHelper OtherPlayerHelper
        {
            get
            {
                return this.otherPlayerHelper;
            }
        }

        /// <summary>
        /// We don't have access to the internal <see cref="GameFase"/>GameFase of the <see cref="Game"/> instance but the GoToNextFase will change
        /// the internal <see cref="GameFase"/> so this is used to keep track of this and needs to be updated right before every GoToNextFase call.
        /// </summary>
        public GameFase CurrentGameFase
        {
            get
            {
                return this.currentGameFase;
            }

            set
            {
                this.currentGameFase = value;
            }
        }

        /// <summary>
        /// How many times did <see cref="IGame"/> instance call the PlayerReceivesNewArmies Event.
        /// </summary>
        public int PlayerReceivesNewArmiesCount
        {
            get
            {
                return this.playerReceivesNewArmiesCount;
            }
        }

        /// <summary>
        /// How many times did <see cref="IGame"/> instance call the PlayerCountryArmiesChanged Event.
        /// </summary>
        public int PlayerCountryArmiesChangedCount
        {
            get
            {
                return this.playerCountryArmiesChangedCount;
            }
        }

        /// <summary>
        /// How many times did <see cref="IGame"/> instance call the PlayerGetsTurn Event.
        /// </summary>
        public int PlayerGetsTurnCount
        {
            get
            {
                return this.playerGetsTurnCount;
            }
        }

        /// <summary>
        /// How many times did <see cref="IGame"/> instance call the PlayerReceivesNewCard Event.
        /// </summary>
        public int PlayerReceivesNewCardCount
        {
            get
            {
                return this.playerReceivesNewCardCount;
            }
        }

        /// <summary>
        /// How many times did <see cref="IGame"/> instance call the PlayerHasWon Event.
        /// </summary>
        public int PlayerHasWonCount
        {
            get
            {
                return this.playerHasWonCount;
            }
        }

        /// <summary>
        /// Hook up the GotoChooseTurnType event.
        /// </summary>
        public void SetGotoChooseTurnType()
        {
            this.game.PlayerGetsTurn += this.GotoChooseTurnType;
        }

        /// <summary>
        /// We have to handle all the PlayerGetsTurn events for all the <see cref="IPlayer"/> instances to go to the 
        /// ChooseTurnType GameFase of one of the <see cref="IPlayer"/> instances.
        /// </summary>
        /// <param name="sender"> The <see cref="Game"/> object calling the event.</param>
        /// <param name="e"> The PlayerEventArgs instance. </param>
        public void GotoChooseTurnType(object sender, EventArgs.PlayerEventArgs e)
        {
            // Can't be shure currentPlayer is set yet.
            this.currentGameFase = GameFase.ChooseTurnType; 
            PlayerHelper playerHelper = this.playerHelpers.Find(p => p.Player.Color == e.PlayerColor);

            // By checking for 32 we are sure these are not the new armies for continents amount but the initialarmie so we are still in
            // ChooseTurnType
            if (playerHelper.Player.NumberOfNewArmies == 36)
            {
                string countryName = playerHelper.CountryNumberOfArmies.FirstOrDefault(c => c.Key.Name == "1").Key != null ? "1" : "6";
                this.game.PlaceNewArmies(24, countryName);
                countryName = playerHelper.CountryNumberOfArmies.FirstOrDefault(c => c.Key.Name == "5").Key != null ? "5" : "2";
                this.game.PlaceNewArmies(12, countryName);
                this.game.GoToNextFase();
            }
        }

        /// <summary>
        /// Hookup the PlayerGetsTurn Event event to keep the PlayerHelper up to date.
        /// </summary>
        /// <param name="sender"> The <see cref="Game"/> object calling the event. </param>
        /// <param name="e"> The <see cref="PlayerEventArgs"/> instance. </param>
        public void Game_PlayerGetsTurn(object sender, EventArgs.PlayerEventArgs e)
        {
            this.currentPlayerHelper = this.playerHelpers.Find(p => p.Player.Color == e.PlayerColor);
            this.otherPlayerHelper = this.playerHelpers.Find(p => p.Player.Color != e.PlayerColor);
        }

        /// <summary>
        /// Hookup the PlayerCountryArmiesChanged Event event to keep the PlayerHelper up to date.
        /// </summary>
        /// <param name="sender"> The <see cref="Game"/> object calling the event. </param>
        /// <param name="e"> The <see cref="PlayerCountryArmiesChangedEventArgs"/> instance. </param>
        public void Game_PlayerCountryArmiesChanged(object sender, EventArgs.PlayerCountryArmiesChangedEventArgs e)
        {
            Country country = this.world.Countries.Find(w => w.Name == e.Country);
            if (e.Armies == 0)
            {
                this.playerHelpers.Find(p => p.Player.Color == e.PlayerColor).CountryNumberOfArmies.Remove(country);
            }
            else
            {
                this.playerHelpers.Find(p => p.Player.Color == e.PlayerColor).CountryNumberOfArmies[country] = e.Armies;
            }
        }

        /// <summary>
        /// Hookup the PlayerReceivesNewArmies event to keep the PlayerHelper up to date.
        /// </summary>
        /// <param name="sender"> The <see cref="Game"/> object calling the event. </param>
        /// <param name="e"> The <see cref="PlayerReceivesNewArmiesEventArgs"/> instance. </param>
        public void Game_PlayerReceivesNewArmies(object sender, EventArgs.PlayerReceivesNewArmiesEventArgs e)
        {
            this.playerHelpers.Find(p => p.Player.Color == e.PlayerColor).NumberOfNewArmies = e.Armies;
        }

        /// <summary>
        /// Keep track of the amount of times the PlayerCountryArmiesChanged Event is called in a specfic <see cref="GameFase"/>.
        /// </summary>
        /// <param name="gameFase"> The <see cref="GameFase"/> for which we want to now the Event count. </param>
        public void SetupCountForPlayerCountryArmiesChanged(GameFase gameFase)
        {
            this.game.PlayerCountryArmiesChanged += delegate(object sender, PlayerCountryArmiesChangedEventArgs e)
            {
                if (this.currentGameFase == gameFase)
                {
                    this.playerCountryArmiesChangedCount++;
                }
            };
        }

        /// <summary>
        /// Keep track of the amount of times the PlayerGetsTurn Event is called in a specfic <see cref="GameFase"/>.
        /// </summary>
        /// <param name="gameFase"> The <see cref="GameFase"/> for which we want to now the Event count. </param>
        public void SetupCountForPlayerGetsTurn(GameFase gameFase)
        {
            this.game.PlayerGetsTurn += delegate(object sender, PlayerEventArgs e)
            {
                if (this.currentGameFase == gameFase)
                {
                    this.playerGetsTurnCount++;
                }
            };
        }

        /// <summary>
        /// Keep track of the amount of times the PlayerReceivesNewCard Event is called in a specfic <see cref="GameFase"/>.
        /// </summary>
        /// <param name="gameFase"> The <see cref="GameFase"/> for which we want to now the Event count. </param>
        public void SetupCountForPlayerReceivesNewCard(GameFase gameFase)
        {
            this.game.PlayerReceivesNewCard += delegate(object sender, PlayerReceivesNewCardEventArgs e)
            {
                if (this.currentGameFase == gameFase)
                {
                    this.playerReceivesNewCardCount++;
                }
            };
        }

        /// <summary>
        /// Keep track of the amount of times the PlayerHasWon Event is called in a specfic <see cref="GameFase"/>.
        /// </summary>
        /// <param name="gameFase"> The <see cref="GameFase"/> for which we want to now the Event count. </param>
        public void SetupCountForPlayerHasWon(GameFase gameFase)
        {
            this.game.PlayerHasWon += delegate(object sender, PlayerEventArgs e)
            {
                if (this.currentGameFase == gameFase)
                {
                    this.playerHasWonCount++;
                }
            };
        }

        /// <summary>
        /// Keep track of the amount of times the PlayerReceivesNewArmies Event is called in a specfic <see cref="GameFase"/>.
        /// </summary>
        /// <param name="gameFase"> The <see cref="GameFase"/> for which we want to now the Event count. </param>
        public void SetupCountForPlayerReceicesNewArmies(GameFase gameFase)
        {
            this.game.PlayerReceivesNewArmies += delegate(object sender, PlayerReceivesNewArmiesEventArgs e)
            {
                if (this.currentGameFase == gameFase)
                {
                    this.playerReceivesNewArmiesCount++;
                }
            };
        }

        /// <summary>
        /// For the tests we want to control the rolling of the dices and decide which player wins and which loses.
        /// </summary>
        /// <param name="attackerWins"> Should the attacker get the winning dices? </param>
        public void SettingUpAttackingAndDefendingDices(bool attackerWins)
        {
            int attacker = this.playerHelpers[0] == this.currentPlayerHelper ? 0 : 1;
            int defender = this.playerHelpers[0] == this.currentPlayerHelper ? 1 : 0;
            this.playerHelpers[attacker].SetDices(attackerWins ? 6 : 1, true);
            this.playerHelpers[defender].SetDices(attackerWins ? 1 : 6, false);
        }
    }
}
