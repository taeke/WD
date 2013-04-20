//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Game.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using WDGameEngine.Enums;
    using WDGameEngine.EventArgs;
    using WDGameEngine.Interfaces;

    /// <summary>
    /// <inheritDoc/>
    /// </summary>
    public class Game : IGame
    {
        private const int NewArmiesForThreeDifferentCardTypes = 10;

        /// <summary>
        /// The list of <see cref="IPlayer"/> playing the game.
        /// </summary>
        private List<IPlayer> players = new List<IPlayer>();

        /// <summary>
        /// <see cref="IPlayer"/> who's turn it is.
        /// </summary>
        private IPlayer currentPlayer = null;

        /// <summary>
        /// The current <see cref="GameFase"/> for the turn off the currentPlayer.
        /// </summary>
        private GameFase currentGameFase;

        /// <summary>
        /// Is the game started?
        /// </summary>
        private bool isStarted = false;

        /// <summary>
        /// Is the game ended?
        /// </summary>
        private bool isEnded = false;

        /// <summary>
        /// Did the currentPlayer receive a <see cref="CardType"/> during this turn.
        /// </summary>
        private bool currentPlayerGotCardThisTurn = false;

        /// <summary>
        /// The <see cref="IPlayer"/> may only move armies once in each <see cref="GameFase"/>.
        /// </summary>
        private bool movedArmiesThisGameFase = false;

        /// <summary>
        /// Information about the last attack.
        /// </summary>
        private LastAttackInfo lastAttackInfo = new LastAttackInfo();

        /// <summary>
        /// The collection of <see cref="Continent"/> and <see cref="Country"/>.
        /// </summary>
        private IWorld world;

        /// <summary>
        /// The collection of colors for the <see cref="IPlayer"/>.
        /// </summary>
        private List<Color> playerColors;

        /// <summary>
        /// A factory for creating new <see cref="IPlayer"/> instances.
        /// </summary>
        private Func<Color, IPlayer> playerFactory;

        /// <summary>
        /// We need servel random numbers for the game.
        /// </summary>
        private Randomize random;

        /// <summary>
        /// How to configure the Game.
        /// </summary>
        private Config config;

        /// <summary>
        /// Initializes a Game instance.
        /// </summary>
        /// <param name="world"> An <see cref="IWorld"/> instance.</param>
        /// <param name="playerColors"> The list of Colors for the <see cref="IPlayer"/>.</param>
        /// <param name="playerFactory"> A factory for generating <see cref="IPlayer"/> instances. </param>
        /// <param name="config"> Configuration for the game. </param>
        /// <param name="random"> A class for generating random or pseudo random numbers. </param>
        public Game(IWorld world, List<Color> playerColors, Func<Color, IPlayer> playerFactory, Config config, Randomize random)
        {
            if (world == null)
            {
                throw new ArgumentNullException("world");
            }

            if (playerColors == null)
            {
                throw new ArgumentNullException("playerColors");
            }

            if (playerFactory == null)
            {
                throw new ArgumentNullException("playerFactory");
            }

            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (random == null)
            {
                throw new ArgumentNullException("random");
            }

            this.world = world;
            this.config = config;

            if (playerColors.Count < config.MinimumNumberPlayers)
            {
                throw new ArgumentException(Strings.PLAYER_COLORS_COUNT_SMALLER_MIN_PLAYERS);
            }

            this.playerColors = playerColors;
            this.playerFactory = playerFactory;
            this.random = random;
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerGetsTurn;

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public event EventHandler<PlayerCountryArmiesChangedEventArgs> PlayerCountryArmiesChanged;

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public event EventHandler<PlayerReceivesNewArmiesEventArgs> PlayerReceivesNewArmies;

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public event EventHandler<PlayerReceivesNewCardEventArgs> PlayerReceivesNewCard;

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public event EventHandler<PlayerEventArgs> PlayerHasWon;

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        /// <returns> <inheritDoc/> </returns>
        public Color AddPlayer()
        {
            if (this.isStarted)
            {
                throw new InvalidOperationException(Strings.CANT_ADDPLAYER_IF_GAME_STARTED);
            }

            if (this.playerColors.Count == this.players.Count)
            {
                throw new InvalidOperationException(Strings.CANT_ADD_MORE_PLAYERS_AS_COLORS);
            }

            IPlayer newPlayer = this.playerFactory(this.playerColors[this.players.Count]);
            this.players.Add(newPlayer);
            return newPlayer.Color;
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public void StartGame()
        {
            if (this.players.Count < this.config.MinimumNumberPlayers)
            {
                throw new InvalidOperationException(Strings.CANT_START_WITH_LESS_MIN_PLAYERS);
            }

            if (this.isStarted)
            {
                throw new InvalidOperationException(Strings.GAME_ALREADY_STARTED);
            }

            this.isStarted = true;
            this.currentGameFase = GameFase.PlaceInitialArmies;
            this.SetupWorld();
            this.DivideInitialArmies();
            this.ChangeCurrentPlayer();
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        /// <param name="turnType"> <inheritDoc/>. </param>
        public void ChooseTurnType(TurnType turnType)
        {
            if (!this.isStarted)
            {
                throw new InvalidOperationException(Strings.CANT_CHOOSETURNTYPE_GAME_NOT_STARTED);
            }

            if (this.currentGameFase != GameFase.ChooseTurnType)
            {
                throw new InvalidOperationException(Strings.CANT_CHOOSETURNTYPE_NOT_IN_FASE);
            }

            if (!Enum.IsDefined(typeof(TurnType), turnType))
            {
                throw new ArgumentOutOfRangeException("turnType");
            }

            this.currentPlayer.TurnType = turnType;
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        /// <param name="cards"> <inheritDoc/>. </param>
        public void ExchangeCards(List<CardType> cards)
        {
            if (!this.isStarted)
            {
                throw new InvalidOperationException(Strings.CANT_EXCHANGECARDS_GAME_NOT_STARTED);
            }

            if (this.currentGameFase != GameFase.ExchangeCards)
            {
                throw new InvalidOperationException(Strings.CANT_EXCHANGECARDS_NOT_IN_FASE);
            }

            if (cards == null)
            {
                throw new ArgumentNullException("cards");
            }

            if (cards.Count != 3)
            {
                throw new ArgumentOutOfRangeException("cards");
            }

            int distinct = cards.Distinct().Count();
            if (!(distinct == 1) && !(distinct == cards.Count()))
            {
                throw new ArgumentOutOfRangeException("cards");
            }

            if (distinct == 3 && cards.Intersect(this.currentPlayer.Cards).Count() != 3)
            {
                throw new ArgumentOutOfRangeException("cards");
            }

            if (distinct == 1 && this.currentPlayer.Cards.FindAll(c => c == cards.First()).Count() != 3)
            {
                throw new ArgumentOutOfRangeException("cards");
            }

            this.AddNewArmiesForPlayer(this.currentPlayer, distinct == 1 ? (int)cards.First() : Game.NewArmiesForThreeDifferentCardTypes);
            foreach (CardType card in cards)
            {
                this.currentPlayer.Cards.Remove(this.currentPlayer.Cards.First(c => c == card));
            }
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        /// <param name="count"> <inheritDoc/>. </param>
        /// <param name="countryName"> <inheritDoc/>. </param>
        public void PlaceNewArmies(int count, string countryName)
        {
            if (!this.isStarted)
            {
                throw new InvalidOperationException(Strings.CANT_PLACENEWARMIES_GAME_NOT_STARTED);
            }

            if (countryName == null)
            {
                throw new ArgumentNullException("countryName");
            }

            Country country;
            try
            {
                country = this.world.Countries.First(w => w.Name == countryName);
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException(Strings.NOT_A_VALID_COUNTRY_NAME);
            }

            if (this.currentGameFase != GameFase.PlaceNewArmies && this.currentGameFase != GameFase.PlaceInitialArmies)
            {
                throw new InvalidOperationException(Strings.CANT_PLACENEWARMIES_NOT_IN_FASE);
            }

            if (count < 1)
            {
                throw new ArgumentException(Strings.MUST_PLACE_AT_LEAST_ONE_ARMIE);
            }

            if (count > this.currentPlayer.NumberOfNewArmies)
            {
                throw new ArgumentException(Strings.CANT_PLACE_MORE_AS_OWNED);
            }

            if (this.FindPlayerOwningCountry(country) != this.currentPlayer)
            {
                throw new ArgumentException(Strings.PLACENEWARMIES_DONT_OWN_COUNTRY);
            }

            this.AddNewArmiesForPlayer(this.currentPlayer, -count);
            this.AddArmiesOnCountry(country, count, true);
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        /// <param name="attackingCountryName"> <inheritDoc/>. </param>
        /// <param name="defendingCountryName"> <inheritDoc/>. </param>
        /// <param name="count"> <inheritDoc/>. </param>
        public void Attack(string attackingCountryName, string defendingCountryName, int count)
        {
            if (!this.isStarted)
            {
                throw new InvalidOperationException(Strings.CANT_ATTACK_GAME_NOT_STARTED);
            }

            if (attackingCountryName == null)
            {
                throw new ArgumentNullException("attackingCountryName");
            }

            Country attackingCountry;
            try
            {
                attackingCountry = this.world.Countries.First(w => w.Name == attackingCountryName);
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException(Strings.NOT_A_VALID_COUNTRY_NAME);
            }

            if (defendingCountryName == null)
            {
                throw new ArgumentNullException("defendingCountryName");
            }

            Country defendingCountry;
            try
            {
                defendingCountry = this.world.Countries.First(w => w.Name == defendingCountryName);
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException(Strings.NOT_A_VALID_COUNTRY_NAME);
            }

            if (this.currentGameFase != GameFase.Attack)
            {
                throw new InvalidOperationException(Strings.CANT_ATTACK_NOT_IN_FASE);
            }

            if (count < 1)
            {
                throw new ArgumentException(Strings.MUST_ATTACK_AT_LEAST_ONE_ARMIE);
            }

            if (this.currentPlayer.CountryNumberOfArmies.ContainsKey(defendingCountry))
            {
                throw new ArgumentException(Strings.ATTACK_OWN_COUNTRY);
            }

            if (!this.currentPlayer.CountryNumberOfArmies.ContainsKey(attackingCountry))
            {
                throw new ArgumentException(Strings.ATTACK_DONT_OWN_COUNTRY);
            }

            if (!attackingCountry.Neighbours.Contains(defendingCountry))
            {
                throw new ArgumentException(Strings.ATTACK_ONLY_NEIGHBOUR);
            }

            if ((count > 2 ? 3 : count) > this.currentPlayer.CountryNumberOfArmies[attackingCountry] - count)
            {
                throw new ArgumentException(Strings.ATTACK_WITH_TO_MANY_ARMIES);
            }

            this.FillAttackInfo(attackingCountry, defendingCountry, count);
            KeyValuePair<IPlayer, int> winner = this.Fight(defendingCountry, count);
            this.lastAttackInfo.AttackerIsWinner = winner.Key == this.currentPlayer;
            int aditonalArmies = (winner.Key == this.currentPlayer) && (winner.Value < (count > 2 ? 3 : count)) ? (count > 2 ? 3 : count) - winner.Value : 0;
            this.NewCardIfAttackerWinsAndNotReceivedACard(winner);
            this.SetArmiesDefendingCountry(winner, defendingCountry, aditonalArmies);
            this.ChangeCountryNumberArmies(attackingCountry, this.currentPlayer, this.currentPlayer.CountryNumberOfArmies[attackingCountry] - count - aditonalArmies);
            this.CheckAttackHasWonGame();
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        /// <param name="fromName"> <inheritDoc/>. </param>
        /// <param name="toName"> <inheritDoc/>. </param>
        /// <param name="count"> <inheritDoc/>. </param>
        public void MoveArmies(string fromName, string toName, int count)
        {
            if (!this.isStarted)
            {
                throw new InvalidOperationException(Strings.CANT_MOVEARMIES_GAME_NOT_STARTED);
            }

            if (fromName == null)
            {
                throw new ArgumentNullException("fromName");
            }

            Country from;
            try
            {
                from = this.world.Countries.First(w => w.Name == fromName);
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }

            if (toName == null)
            {
                throw new ArgumentNullException("toName");
            }

            Country to;
            try
            {
                to = this.world.Countries.First(w => w.Name == toName);
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }

            if (this.currentGameFase != GameFase.MoveArmiesAfterAttack && this.currentGameFase != GameFase.MoveArmiesEndOfTurn)
            {
                throw new InvalidOperationException(Strings.CANT_MOVEARMIES_NOT_IN_FASE);
            }

            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(Strings.MUST_MOVE_AT_LEAST_ONE_ARMIE);
            }

            if (from == to)
            {
                throw new ArgumentException(Strings.FROM_AND_TO_CANT_BE_THE_SAME);
            }

            if (!this.currentPlayer.CountryNumberOfArmies.ContainsKey(to))
            {
                throw new ArgumentException(Strings.MOVEARMIES_DONT_OWN_COUNTRY);
            }

            if (!this.currentPlayer.CountryNumberOfArmies.ContainsKey(from))
            {
                throw new ArgumentException(Strings.MOVEARMIES_DONT_OWN_COUNTRY);
            }

            if (this.currentPlayer.CountryNumberOfArmies[from] <= count)
            {
                throw new ArgumentOutOfRangeException(Strings.CANT_MOVE_ALL_OR_MORE_ARMIES);
            }

            if (this.currentGameFase == GameFase.MoveArmiesAfterAttack &&
                ((from != this.lastAttackInfo.AttackingCountry &&
                 from != this.lastAttackInfo.DefendingCountry) ||
                 (to != this.lastAttackInfo.AttackingCountry &&
                  to != this.lastAttackInfo.DefendingCountry)))
            {
                throw new ArgumentException(Strings.CANT_MOVE_FROM_OTHER_COUNTRIES);
            }

            if (this.currentGameFase == GameFase.MoveArmiesAfterAttack &&
                this.lastAttackInfo.DefendingCountry == from &&
                this.currentPlayer.CountryNumberOfArmies[from] - count < (this.lastAttackInfo.AttackingArmies > 2 ? 3 : this.lastAttackInfo.AttackingArmies))
            {
                throw new ArgumentException(Strings.MUST_LEAVE_ATTACKING_COUNTRIES);
            }

            if (this.movedArmiesThisGameFase)
            {
                throw new InvalidOperationException(Strings.MOVE_ONLY_ONCE_IN_A_TURN);
            }

            if (!from.Neighbours.Contains(to))
            {
                throw new ArgumentException(Strings.MOVE_ONLY_NEIGHBOUR);
            }

            this.movedArmiesThisGameFase = true;
            this.ChangeCountryNumberArmies(from, this.currentPlayer, this.currentPlayer.CountryNumberOfArmies[from] - count);
            this.ChangeCountryNumberArmies(to, this.currentPlayer, this.currentPlayer.CountryNumberOfArmies[to] + count);
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        public void GoToNextFase()
        {
            this.movedArmiesThisGameFase = false;
            if (this.isEnded)
            {
                throw new InvalidOperationException(Strings.CANT_GOTONEXTFASE_GAME_IS_ENDED);
            }

            switch (this.currentGameFase)
            {
                case GameFase.None:
                    throw new InvalidOperationException(Strings.CANT_GOTONEXTFASE_GAME_NOT_STARTED);
                case GameFase.PlaceInitialArmies:
                    this.PlaceInitialArmiesFace();
                    break;
                case GameFase.ChooseTurnType:
                    this.ChooseTurnTypeFace();
                    break;
                case GameFase.ExchangeCards:
                    this.ExchangeCardsFase();
                    break;
                case GameFase.PlaceNewArmies:
                    this.PlaceNewArmiesFase();
                    break;
                case GameFase.Attack:
                    this.AttackFase();
                    break;
                case GameFase.MoveArmiesAfterAttack:
                    this.MoveArmiesAfterAttackFase();
                    break;
                case GameFase.MoveArmiesEndOfTurn:
                    this.MoveArmiesEndOfTurnFase();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Call the PlayerGetsTurn event.
        /// </summary>
        /// <param name="e"> The <see cref="PlayerEventArgs"/> instance. </param>
        protected virtual void OnPlayerGetsTurn(PlayerEventArgs e)
        {
            if (this.PlayerGetsTurn != null)
            {
                this.PlayerGetsTurn(this, e);
            }
        }

        /// <summary>
        /// Call the PlayerCountryArmiesChanged event.
        /// </summary>
        /// <param name="e"> The <see cref="PlayerCountryArmiesChangedEventArgs"/> instance. </param>
        protected virtual void OnPlayerCountryArmiesChanged(PlayerCountryArmiesChangedEventArgs e)
        {
            if (this.PlayerCountryArmiesChanged != null)
            {
                this.PlayerCountryArmiesChanged(this, e);
            }
        }

        /// <summary>
        /// Call the PlayerCountryArmiesChanged event.
        /// </summary>
        /// <param name="e"> The <see cref="PlayerCountryArmiesChangedEventArgs"/> instance. </param>
        protected virtual void OnPlayerReceivesNewCard(PlayerReceivesNewCardEventArgs e)
        {
            if (this.PlayerReceivesNewCard != null)
            {
                this.PlayerReceivesNewCard(this, e);
            }
        }

        /// <summary>
        /// Call the PlayerHasWon event.
        /// </summary>
        /// <param name="e"> The <see cref="PlayerEventArgs"/> instance. </param>
        protected virtual void OnPlayerHasWon(PlayerEventArgs e)
        {
            if (this.PlayerHasWon != null)
            {
                this.PlayerHasWon(this, e);
            }
        }

        /// <summary>
        /// Call the PlayerReceivesNewArmies event.
        /// </summary>
        /// <param name="e"> The <see cref="PlayerReceivesNewArmiesEventArgs"/> instance. </param>
        protected virtual void OnPlayerReceivesNewArmies(PlayerReceivesNewArmiesEventArgs e)
        {
            if (this.PlayerReceivesNewArmies != null)
            {
                this.PlayerReceivesNewArmies(this, e);
            }
        }

        /// <summary>
        /// Divides the armies accros the players at the start of the game.
        /// </summary>
        private void DivideInitialArmies()
        {
            foreach (IPlayer player in this.players)
            {
                this.AddNewArmiesForPlayer(player, this.config.NumberOfPlayersIntialArmies[this.players.Count] - player.CountryNumberOfArmies.Count);
            }
        }

        /// <summary>
        /// Create the <see cref="World"/> for this Game and devide the countries over the players.
        /// </summary>
        private void SetupWorld()
        {
            this.world.LoadContinentsAndCountries();
            if (this.world.Countries == null ||
                this.world.Countries.Count < this.players.Count ||
                this.world.Continents == null ||
                this.world.Continents.Count == 0)
            {
                throw new InvalidOperationException(Strings.NOT_LOADED_WORLD_CORRECT);
            }

            this.DivideCountries();
        }

        /// <summary>
        /// Divide the <see cref="Country"/> list over the <see cref="IPlayer"/> every player gets the same amount of 
        /// <see cref="Country"/> but which <see cref="Country"/> each player gets is random.
        /// </summary>
        private void DivideCountries()
        {
            int playerIndex = 0;
            List<int> indexList = Enumerable.Range(0, this.world.Countries.Count).ToList();
            for (int i = 0; i < this.world.Countries.Count; i++)
            {
                int index = this.random.Next(indexList.Count - 1);
                this.ChangeCountryNumberArmies(this.world.Countries[indexList[index]], this.players[playerIndex], 1);
                indexList.RemoveAt(index);
                playerIndex = playerIndex < this.players.Count - 1 ? playerIndex + 1 : 0;
            }
        }

        /// <summary>
        /// This method will return the player who won the <see cref="Country"/> and the number of armies he got left on it after the attack.
        /// </summary>
        /// <param name="country"> The <see cref="Country"/> defending. </param>
        /// <param name="armies"> The number of armies used in the attack by the attackingCountry. </param>
        private KeyValuePair<IPlayer, int> Fight(Country country, int armies)
        {
            int attackerArmies = armies;
            int defenderArmies = this.GetArmiesOnCountry(country);
            IPlayer defendingPlayer = this.FindPlayerOwningCountry(country);
            while (defenderArmies > 0 && attackerArmies > 0)
            {
                List<int> dicesDefender = defendingPlayer.RollTheDices(country);
                List<int> dicesAttacker = this.currentPlayer.RollTheDices(attackerArmies);
                int indexDice = 0;
                while (indexDice < dicesAttacker.Count && indexDice < dicesDefender.Count && attackerArmies > 0 && defenderArmies > 0)
                {
                    bool defenderWon = dicesDefender[indexDice] >= dicesAttacker[indexDice];
                    defenderArmies = defenderWon ? defenderArmies : defenderArmies - 1;
                    attackerArmies = defenderWon ? attackerArmies - 1 : attackerArmies;
                    indexDice++;
                }
            }

            return defenderArmies == 0 ?
                new KeyValuePair<IPlayer, int>(this.currentPlayer, attackerArmies) :
                new KeyValuePair<IPlayer, int>(defendingPlayer, defenderArmies);
        }

        /// <summary>
        /// Give the <see cref="IPlayer"/> a new CardType if this.currentPlayerGotCardThisTurn is false.
        /// it will change this.currentPlayerGotCardThisTurn to true and this will only happen if
        /// attackerHasWon is true.
        /// </summary>
        /// <param name="attackerHasWon"> The <see cref="IPlayer"/> has won. </param>
        private void SetNewCard(bool attackerHasWon)
        {
            if (attackerHasWon && !this.currentPlayerGotCardThisTurn)
            {
                this.currentPlayerGotCardThisTurn = true;
                this.currentPlayer.Cards.Add(CardType.Artillery);
            }
        }

        /// <summary>
        /// Change the number of armies on a <see cref="Country"/> for a <see cref="IPlayer"/>. It will remove the <see cref="Country"/>
        /// by this <see cref="IPlayer"/> if the number of armies is 0. And will add the <see cref="Country"/> if count > 0 and
        /// the <see cref="IPlayer"/> doesn't own it yet. It will also call OnPlayerCountryArmiesChanged if something really changed.
        /// </summary>
        /// <param name="country"> The <see cref="Country"/> to add the armies to. </param>
        /// <param name="player"> The <see cref="IPlayer"/> which owns, gets or loses the <see cref="Country"/>. </param>
        /// <param name="count"> The number of armies to be placed on the <see cref="Country"/> if it is 0 the <see cref="IPlayer"/> 
        /// loses the <see cref="Country"/>. </param>
        private void ChangeCountryNumberArmies(Country country, IPlayer player, int count)
        {
            if (count == 0)
            {
                if (player.CountryNumberOfArmies.ContainsKey(country))
                {
                    player.CountryNumberOfArmies.Remove(country);
                }
            }
            else
            {
                if (!player.CountryNumberOfArmies.ContainsKey(country))
                {
                    player.CountryNumberOfArmies.Add(country, 0);
                }

                if (player.CountryNumberOfArmies[country] != count)
                {
                    player.CountryNumberOfArmies[country] = count;
                    this.OnPlayerCountryArmiesChanged(new PlayerCountryArmiesChangedEventArgs(player.Color, country.Name, count));
                }
            }
        }

        /// <summary>
        /// Change the current <see cref="IPlayer"/> if there is another player left. It will reset all state for the turn. It will also call OnPlayerGetsTurn 
        /// or OnPlayerHasWon.
        /// </summary>
        private void ChangeCurrentPlayer()
        {
            List<IPlayer> playersToChooseFrom = this.players.Where(p => p.CountryNumberOfArmies.Count > 0).ToList();
            this.currentPlayer = this.GetNextPlayer(playersToChooseFrom);
            this.currentPlayerGotCardThisTurn = false;
            this.currentPlayer.TurnType = TurnType.NotChosen;
            if (this.currentGameFase == GameFase.ChooseTurnType)
            {
                this.AddNewArmiesForPlayer(this.currentPlayer, this.NewArmiesForOwningContinents());
            }

            this.OnPlayerGetsTurn(new PlayerEventArgs(this.currentPlayer.Color));
        }

        /// <summary>
        /// Picks the next <see cref="IPlayer"/> from a list of <see cref="IPlayer"/> to choose from.
        /// </summary>
        /// <param name="playersToChooseFrom"> List of <see cref="IPlayer"/> to choose from. </param>
        private IPlayer GetNextPlayer(List<IPlayer> playersToChooseFrom)
        {
            if (this.currentPlayer == null)
            {
                return this.players[0];
            }
            else
            {
                int index = this.currentPlayer == playersToChooseFrom.Last() ? 0 : playersToChooseFrom.IndexOf(this.currentPlayer) + 1;
                return playersToChooseFrom[index];
            }
        }

        /// <summary>
        /// Every <see cref="Country"/> is owned by a <see cref="IPlayer"/>. This method will return a <see cref="IPlayer"/> who
        /// owns the <see cref="Country"/>
        /// </summary>
        /// <param name="country"> The <see cref="Country"/> you want to find the Owner for</param>
        /// <returns> The <see cref="IPlayer"/> who owns the <see cref="Country"/>. </returns>
        private IPlayer FindPlayerOwningCountry(Country country)
        {
            return this.players.Find(p => p.CountryNumberOfArmies.ContainsKey(country));
        }

        /// <summary>
        /// Will find the amount of armies on a <see cref="Country"/> without needing to specify the <see cref="IPlayer"/> 
        /// owning the <see cref="Country"/>.
        /// </summary>
        /// <param name="country"> The <see cref="Country"/> for which to find the amount of armies. </param>
        /// <returns> The amount of armies on the <see cref="Country"/>. </returns>
        private int GetArmiesOnCountry(Country country)
        {
            IPlayer player = this.FindPlayerOwningCountry(country);
            return player.CountryNumberOfArmies[country];
        }

        /// <summary>
        /// Will set the amount of armies on a <see cref="Country"/> without needing to specify the <see cref="IPlayer"/> 
        /// owning the <see cref="Country"/>.
        /// </summary>
        /// <param name="country"> The <see cref="Country"/> for which to set the amount of armies. </param>
        /// <param name="count"> The amount of armies to set on the <see cref="Country"/>.</param>
        /// <param name="callChangeEvent"> Should it also call the PlayerCountryArmiesChanged event. </param>
        private void AddArmiesOnCountry(Country country, int count, bool callChangeEvent)
        {
            IPlayer player = this.FindPlayerOwningCountry(country);
            player.CountryNumberOfArmies[country] = player.CountryNumberOfArmies[country] + count;
            if (callChangeEvent)
            {
                this.OnPlayerCountryArmiesChanged(new PlayerCountryArmiesChangedEventArgs(player.Color, country.Name, player.CountryNumberOfArmies[country]));
            }
        }

        /// <summary>
        /// Calculates how many armies the current <see cref="IPlayer"/> will receive for owning continents.
        /// </summary>
        /// <returns> The number of new armies. </returns>
        private int NewArmiesForOwningContinents()
        {
            int result = 0;
            foreach (Continent continent in this.world.Continents)
            {
                Country countryNotOwnedOnContinent = continent.Countries.Find(c => !this.currentPlayer.CountryNumberOfArmies.ContainsKey(c));
                if (countryNotOwnedOnContinent == null)
                {
                    result = result + continent.NewArmiesForOwning;
                }
            }

            return result;
        }

        /// <summary>
        /// Handeling GoToNextFase when in PlaceInitialArmies <see cref="GameFase"/>.
        /// Will also call ChangeCurrentPlayer
        /// </summary>
        private void PlaceInitialArmiesFace()
        {
            if (this.currentPlayer.NumberOfNewArmies > 0)
            {
                throw new InvalidOperationException(Strings.PLACE_ALL_NEW_ARMIES);
            }

            if (this.currentPlayer == this.players.Last())
            {
                this.currentGameFase = GameFase.ChooseTurnType;
            }

            this.ChangeCurrentPlayer();
        }

        /// <summary>
        /// Handeling GoToNextFase when in ChooseTurnType <see cref="GameFase"/>.
        /// Will call AddNewArmiesForPlayer when the <see cref="IPlayer"/> chose <see cref="TurnType"/> GetArmiesForCountries.
        /// </summary>
        private void ChooseTurnTypeFace()
        {
            if (this.currentPlayer.TurnType == TurnType.NotChosen)
            {
                throw new InvalidOperationException(Strings.MUST_CHOOSETURNTYPE);
            }

            this.currentGameFase = GameFase.ExchangeCards;
            if (this.currentPlayer.TurnType == TurnType.GetArmiesForCountries)
            {
                this.AddNewArmiesForPlayer(this.currentPlayer, (int)this.currentPlayer.CountryNumberOfArmies.Count / 3);
            }
        }

        /// <summary>
        /// Handeling GoToNextFase when in ExchangeCards <see cref="GameFase"/>.
        /// </summary>
        private void ExchangeCardsFase()
        {
            if (this.currentPlayer.Cards.Count() == this.config.MaximumCards)
            {
                throw new InvalidOperationException(Strings.HOLD_TO_MANY_CARDS);
            }

            this.currentGameFase = GameFase.PlaceNewArmies;
        }

        /// <summary>
        /// Handeling GoToNextFase when in PlaceNewArmies <see cref="GameFase"/>.
        /// Will also call ChangeCurrentPlayer is the currentPlayer chose <see cref="TurnType"/> GetArmiesForCountries.
        /// </summary>
        private void PlaceNewArmiesFase()
        {
            if (this.currentPlayer.NumberOfNewArmies != 0)
            {
                throw new InvalidOperationException(Strings.PLACE_ALL_NEW_ARMIES);
            }

            switch (this.currentPlayer.TurnType)
            {
                case TurnType.NotChosen:
                    throw new InvalidOperationException(Strings.WRONG_TURNTYPE);
                case TurnType.Attack:
                    this.currentGameFase = GameFase.Attack;
                    break;
                case TurnType.GetArmiesForCountries:
                    this.currentGameFase = GameFase.ChooseTurnType;
                    this.ChangeCurrentPlayer();
                    break;
                default:
                    throw new InvalidOperationException(Strings.WRONG_TURNTYPE);
            }
        }

        /// <summary>
        /// Handeling GoToNextFase when in Attack <see cref="GameFase"/>.
        /// Resets this.lastAttackInfo.AttackerIsWinner = false.
        /// </summary>
        private void AttackFase()
        {
            if (this.lastAttackInfo.AttackerIsWinner)
            {
                this.currentGameFase = GameFase.MoveArmiesAfterAttack;
                this.lastAttackInfo.AttackerIsWinner = false;
            }
            else
            {
                this.currentGameFase = GameFase.MoveArmiesEndOfTurn;
            }
        }

        /// <summary>
        /// Handeling GoToNextFase when in MoveArmiesEndOfTurn <see cref="GameFase"/>.
        /// Will also call ChangeCurrentPlayer
        /// </summary>
        private void MoveArmiesEndOfTurnFase()
        {
            this.currentGameFase = GameFase.ChooseTurnType;
            this.ChangeCurrentPlayer();
        }

        /// <summary>
        /// Handeling GoToNextFase when in MoveArmiesAfterAttack <see cref="GameFase"/>.
        /// </summary>
        private void MoveArmiesAfterAttackFase()
        {
            this.currentGameFase = GameFase.Attack;
        }

        /// <summary>
        /// Give <see cref="IPlayer"/> a new <see cref="CardType"/> if he has won and did not receive a new <see cref="CardType"/> allready.
        /// </summary>
        /// <param name="winner"> The <see cref="IPlayer"/> who won the fight and the amount of armies left after the fight. </param>
        private void NewCardIfAttackerWinsAndNotReceivedACard(KeyValuePair<IPlayer, int> winner)
        {
            if (winner.Key == this.currentPlayer && !this.currentPlayerGotCardThisTurn)
            {
                this.currentPlayerGotCardThisTurn = true;
                CardType card = (CardType)this.random.Next(2);
                this.currentPlayer.Cards.Add(card);
                this.OnPlayerReceivesNewCard(new PlayerReceivesNewCardEventArgs(this.currentPlayer.Color, card));
            }
        }

        /// <summary>
        /// If the attacking <see cref="IPlayer"/> wins call ChangeCountryNumberArmies to 0 armies for the defending <see cref="IPlayer"/>
        /// and call ChangeCountryNumberArmies for the attacking <see cref="IPlayer"/> on the defending <see cref="Country"/> with the
        /// new amount of armies. Else only for the defending player with the new amount of armies.
        /// </summary>
        /// <param name="winner"> The <see cref="IPlayer"/> who won the fight and the amount of armies left after the fight. </param>
        /// <param name="defendingCountry"> The <see cref="Country"/> which was attacked. </param>
        /// <param name="aditionalArmies"> The additional armies which will be moved to the new Country if the attacker had won. A winning
        /// <see cref="IPlayer"/> must move at least 3 armies to the new <see cref="Country"/> if he attacks with more then or with precise three
        /// armies or else two or only one if he attacks with two armies or one armie. </param>
        private void SetArmiesDefendingCountry(KeyValuePair<IPlayer, int> winner, Country defendingCountry, int aditionalArmies)
        {
            IPlayer defendingPlayer = this.FindPlayerOwningCountry(defendingCountry);
            if (winner.Key == this.currentPlayer)
            {
                this.ChangeCountryNumberArmies(defendingCountry, defendingPlayer, 0);
                this.ChangeCountryNumberArmies(defendingCountry, this.currentPlayer, winner.Value + aditionalArmies);
            }
            else
            {
                this.ChangeCountryNumberArmies(defendingCountry, defendingPlayer, winner.Value);
            }
        }

        /// <summary>
        /// Add new armies on the player and call the PlayerReceivesNewArmies event.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="newArmies"></param>
        private void AddNewArmiesForPlayer(IPlayer player, int newArmies)
        {
            player.NumberOfNewArmies = player.NumberOfNewArmies + newArmies;
            this.OnPlayerReceivesNewArmies(new PlayerReceivesNewArmiesEventArgs(player.Color, player.NumberOfNewArmies));
        }

        /// <summary>
        /// Fill information about the attack.
        /// </summary>
        /// <param name="attackingCountry"> The attacking <see cref="Country"/></param>
        /// <param name="defendingCountry"> The defending <see cref="Country"/></param>
        /// <param name="count"> The amount of armies used in the attack. </param>
        private void FillAttackInfo(Country attackingCountry, Country defendingCountry, int count)
        {
            this.lastAttackInfo.AttackingCountry = attackingCountry;
            this.lastAttackInfo.DefendingCountry = defendingCountry;
            this.lastAttackInfo.AttackingArmies = count;
        }

        /// <summary>
        /// Check if there is only 1 <see cref="IPlayer"/> left with <see cref="Country"/> if this is the case he has won
        /// and this method will call the PlayerHasWon event.
        /// </summary>
        private void CheckAttackHasWonGame()
        {
            this.isEnded = this.players.Where(p => p.CountryNumberOfArmies.Count > 0).Count() == 1;
            if (this.isEnded)
            {
                this.OnPlayerHasWon(new PlayerEventArgs(this.currentPlayer.Color));
            }
        }
    }
}
