//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="Game.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studie Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine
{
    //// TODO : Check if the IPlayer instance stil has some countries otherwise skip and decided its end of the game if only one player left.

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
        private IConfig config;

        /// <summary>
        /// Initializes a Game instance.
        /// </summary>
        /// <param name="world"> An <see cref="IWorld"/> instance.</param>
        /// <param name="playerColors"> The list of Colors for the <see cref="IPlayer"/>.</param>
        /// <param name="playerFactory"> A factory for generating <see cref="IPlayer"/> instances. </param>
        /// <param name="config"> Configuration for the game. </param>
        /// <param name="random"> A class for generating random or pseudo random numbers. </param>
        public Game(IWorld world, List<Color> playerColors, Func<Color, IPlayer> playerFactory, IConfig config, Randomize random)
        {
            if (world == null)
            {
                throw new ArgumentNullException();
            }

            if (playerColors == null)
            {
                throw new ArgumentNullException();
            }

            if (playerFactory == null)
            {
                throw new ArgumentNullException();
            }

            if (config == null)
            {
                throw new ArgumentNullException();
            }

            if (random == null)
            {
                throw new ArgumentNullException();
            }

            this.world = world;
            this.config = config;

            if (playerColors.Count < config.MinimumNumberPlayers)
            {
                throw new ArgumentException();
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
        /// <returns> <inheritDoc/> </returns>
        public Color AddPlayer()
        {
            if (this.isStarted)
            {
                throw new InvalidOperationException();
            }

            if (this.playerColors.Count == this.players.Count)
            {
                throw new InvalidOperationException();
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
                throw new InvalidOperationException();
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
                throw new InvalidOperationException();
            }

            if (this.currentGameFase != GameFase.ChooseTurnType)
            {
                throw new InvalidOperationException();
            }

            if (!Enum.IsDefined(typeof(TurnType), turnType))
            {
                throw new ArgumentOutOfRangeException();
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
                throw new InvalidOperationException();
            }

            if (this.currentGameFase != GameFase.ExchangeCards)
            {
                throw new InvalidOperationException();
            }

            if (cards == null)
            {
                throw new ArgumentNullException();
            }

            if (cards.Count != 3)
            {
                throw new ArgumentOutOfRangeException();
            }

            int distinct = cards.Distinct().Count();
            if (!(distinct == 1) && !(distinct == cards.Count()))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (distinct == 3 && cards.Intersect(this.currentPlayer.Cards).Count() != 3)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (distinct == 1 && this.currentPlayer.Cards.FindAll(c => c == cards.First()).Count() != 3)
            {
                throw new ArgumentOutOfRangeException();
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
        /// <param name="country"> <inheritDoc/>. </param>
        public void PlaceNewArmies(int count, string countryName)
        {
            if (!this.isStarted)
            {
                throw new InvalidOperationException();
            }

            if (countryName == null)
            {
                throw new ArgumentNullException();
            }

            Country country;
            try
            {
                country = this.world.Countries.First(w => w.Name == countryName);
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }

            if (this.currentGameFase != GameFase.PlaceNewArmies && this.currentGameFase != GameFase.PlaceInitialArmies)
            {
                throw new InvalidOperationException();
            }

            if (country == null)
            {
                throw new ArgumentException();
            }

            if (count < 1)
            {
                throw new ArgumentException();
            }

            if (count > this.currentPlayer.NumberOfNewArmies)
            {
                throw new ArgumentException();
            }

            if (this.FindPlayerOwningCountry(country) != this.currentPlayer)
            {
                throw new ArgumentException();
            }

            this.AddNewArmiesForPlayer(this.currentPlayer, -count);
            this.AddArmiesOnCountry(country, count, true);
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        /// <param name="attackingCountry"> <inheritDoc/>. </param>
        /// <param name="defendingCountry"> <inheritDoc/>. </param>
        /// <param name="count"> <inheritDoc/>. </param>
        public void Attack(string attackingCountryName, string defendingCountryName, int count)
        {
            if (!this.isStarted)
            {
                throw new InvalidOperationException();
            }

            if (attackingCountryName == null)
            {
                throw new ArgumentNullException();
            }

            Country attackingCountry;
            try
            {
                attackingCountry = this.world.Countries.First(w => w.Name == attackingCountryName);
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }

            if (defendingCountryName == null)
            {
                throw new ArgumentNullException();
            }

            Country defendingCountry;
            try
            {
                defendingCountry = this.world.Countries.First(w => w.Name == defendingCountryName);
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }

            if (this.currentGameFase != GameFase.Attack)
            {
                throw new InvalidOperationException();
            }

            if (attackingCountry == null)
            {
                throw new ArgumentException();
            }

            if (defendingCountry == null)
            {
                throw new ArgumentException();
            }

            if (count < 1)
            {
                throw new ArgumentException();
            }

            if (this.currentPlayer.CountryNumberOfArmies.ContainsKey(defendingCountry))
            {
                throw new ArgumentException();
            }

            if (!this.currentPlayer.CountryNumberOfArmies.ContainsKey(attackingCountry))
            {
                throw new ArgumentException();
            }

            if (!attackingCountry.Neighbours.Contains(defendingCountry))
            {
                throw new ArgumentException();
            }

            if ((count > 2 ? 3 : count) > this.currentPlayer.CountryNumberOfArmies[attackingCountry] - count)
            {
                throw new ArgumentException();
            }

            this.FillAttackInfo(attackingCountry, defendingCountry, count);
            KeyValuePair<IPlayer, int> winner = this.Fight(defendingCountry, count);
            this.lastAttackInfo.AttackerIsWinner = winner.Key == this.currentPlayer;
            int aditonalArmies = (winner.Key == this.currentPlayer) && (winner.Value < (count > 2 ? 3 : count)) ? (count > 2 ? 3 : count) - winner.Value : 0;
            this.NewCardIfAttackerWinsAndNotReceivedACard(winner);
            this.SetArmiesDefendingCountry(winner, defendingCountry, aditonalArmies);
            this.ChangeCountryNumberArmies(attackingCountry, this.currentPlayer, this.currentPlayer.CountryNumberOfArmies[attackingCountry] - count - aditonalArmies);
        }

        /// <summary>
        /// <inheritDoc/>
        /// </summary>
        /// <param name="from"> <inheritDoc/>. </param>
        /// <param name="to"> <inheritDoc/>. </param>
        /// <param name="count"> <inheritDoc/>. </param>
        public void MoveArmies(string fromName, string toName, int count)
        {
            if (!this.isStarted)
            {
                throw new InvalidOperationException();
            }

            if (fromName == null)
            {
                throw new ArgumentNullException();
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
                throw new ArgumentNullException();
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
                throw new InvalidOperationException();
            }

            if (count < 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (from == null)
            {
                throw new ArgumentNullException();
            }

            if (to == null)
            {
                throw new ArgumentNullException();
            }

            if (from == to)
            {
                throw new ArgumentException();
            }

            if (!this.currentPlayer.CountryNumberOfArmies.ContainsKey(to))
            {
                throw new ArgumentException();
            }

            if (!this.currentPlayer.CountryNumberOfArmies.ContainsKey(from))
            {
                throw new ArgumentException();
            }

            if (this.currentPlayer.CountryNumberOfArmies[from] <= count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (this.currentGameFase == GameFase.MoveArmiesAfterAttack &&
                ((from != this.lastAttackInfo.AttackingCountry &&
                 from != this.lastAttackInfo.DefendingCountry) ||
                 (to != this.lastAttackInfo.AttackingCountry &&
                  to != this.lastAttackInfo.DefendingCountry)))
            {
                throw new ArgumentException();
            }

            if (this.currentGameFase == GameFase.MoveArmiesAfterAttack &&
                this.lastAttackInfo.DefendingCountry == from &&
                this.currentPlayer.CountryNumberOfArmies[from] - count < (this.lastAttackInfo.AttackingArmies > 2 ? 3 : this.lastAttackInfo.AttackingArmies))
            {
                throw new ArgumentException();
            }

            if (this.movedArmiesThisGameFase)
            {
                throw new InvalidOperationException();
            }

            if (!from.Neighbours.Contains(to))
            {
                throw new ArgumentException();
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
            switch (this.currentGameFase)
            {
                case GameFase.None:
                    throw new InvalidOperationException();
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
                throw new InvalidOperationException();
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
        /// <param name="localArmies"> The number of armies used in the attack by the attackingCountry. </param>
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
        /// Change the number of armies on a <see cref="County"/> for a <see cref="IPlayer"/>. It will remove the <see cref="Country"/>
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
        /// Change the current <see cref="IPlayer"/>. It will reset all state for the turn. It will also call OnPlayerGetsTurn.
        /// </summary>
        private void ChangeCurrentPlayer()
        {
            if (this.currentPlayer == null)
            {
                this.currentPlayer = this.players[0];
            }
            else
            {
                int index = this.currentPlayer == this.players.Last() ? 0 : this.players.IndexOf(this.currentPlayer) + 1;
                this.currentPlayer = this.players[index];
            }

            this.currentPlayerGotCardThisTurn = false;
            this.currentPlayer.TurnType = TurnType.NotChosen;
            if (this.currentGameFase == GameFase.ChooseTurnType)
            {
                this.AddNewArmiesForPlayer(this.currentPlayer, this.NewArmiesForOwningContinents());
            }

            this.OnPlayerGetsTurn(new PlayerEventArgs(this.currentPlayer.Color));
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
                throw new InvalidOperationException();
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
                throw new InvalidOperationException();
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
                throw new InvalidOperationException();
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
                throw new InvalidOperationException();
            }

            switch (this.currentPlayer.TurnType)
            {
                case TurnType.NotChosen:
                    throw new InvalidOperationException();
                case TurnType.Attack:
                    this.currentGameFase = GameFase.Attack;
                    break;
                case TurnType.GetArmiesForCountries:
                    this.currentGameFase = GameFase.ChooseTurnType;
                    this.ChangeCurrentPlayer();
                    break;
                default:
                    throw new InvalidOperationException();
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
        /// If the attacking <see cref="IPlayer"/> wins call change CountryNumberArmies to 0 armies for the defending <see cref="IPlayer"/>
        /// and call ChangeCountryNumberArmies for the attacking <see cref="IPlayer"/> on the defending <see cref="Country"/> with the
        /// new amount of armies. Else only for the defending player with the new amount of armies.
        /// </summary>
        /// <param name="winner"> The <see cref="IPlayer"/> who won the fight and the amount of armies left after the fight. </param>
        /// <param name="defendingCountry"> The <see cref="Country"/> which was attacked. </param>
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
    }
}
