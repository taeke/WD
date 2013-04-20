//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="IGame.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using WDGameEngine.Enums;
    using WDGameEngine.EventArgs;
    using WDGameEngine.Interfaces;
    
    /// <summary>
    /// Has a collection of <see cref="IPlayer"/>. Has a currentPlayer and methods that work on the currentPlayer and
    /// has events which communicate changes on the collection of <see cref="IPlayer"/>.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// Handler for a <see cref="IPlayer"/> that gets the turn.
        /// </summary>
        event EventHandler<PlayerEventArgs> PlayerGetsTurn;

        /// <summary>
        /// Handler for a change on the <see cref="Country"/> for a IPlayer. 
        /// </summary>
        event EventHandler<PlayerCountryArmiesChangedEventArgs> PlayerCountryArmiesChanged;

        /// <summary>
        /// Handler for notifying a <see cref="IPlayer"/> amount of new Armies changed. New armies are armies not yet placed on a <see cref="Country"/>
        /// </summary>
        event EventHandler<PlayerReceivesNewArmiesEventArgs> PlayerReceivesNewArmies;

        /// <summary>
        /// Handler for notifying a <see cref="IPlayer"/> got a new <see cref="CardType"/>.
        /// </summary>
        event EventHandler<PlayerReceivesNewCardEventArgs> PlayerReceivesNewCard;

        /// <summary>
        /// Handler for notifying a <see cref="IPlayer"/> has won the game.
        /// </summary>
        event EventHandler<PlayerEventArgs> PlayerHasWon;

       /// <summary>
        /// Add's a new <see cref="IPlayer"/> to the IGame.
        /// </summary>
        /// <returns> Returns the <see cref="Color"/> the <see cref="IPlayer"/> got assigned. </returns>
        Color AddPlayer();

        /// <summary>
        /// After all the <see cref="IPlayer"/> are added the game can start with this method.
        /// </summary>
        void StartGame();

        /// <summary>
        /// Each turn the currentPlayer must decided which kind of <see cref="TurnType"/> he wants to play.
        /// </summary>
        /// <param name="turnType"> The  <see cref="TurnType"/> the currentPlayer choses. </param>
        void ChooseTurnType(TurnType turnType);

        /// <summary>
        /// Each turn the currentPlayer can decided to exchanges a <see cref="CardType"/> collection for armies if he has a valid combination.
        /// </summary>
        /// <param name="cards"> The list of <see cref="CardType"/> the <see cref="IPlayer"/> wants to exchanges for armies. </param>
        void ExchangeCards(List<CardType> cards);

        /// <summary>
        /// The currentPlayer wants to place new armies on a <see cref="Country"/> he owns.
        /// </summary>
        /// <param name="count"> Number of <see cref="Country"/> the <see cref="IPlayer"/> wants to place on his <see cref="Country"/>.</param>
        /// <param name="countryName"> The <see cref="Country"/> the <see cref="IPlayer"/> want to place his armies on. </param>
        void PlaceNewArmies(int count, string countryName);

        /// <summary>
        /// The currentPlayer wants to attack a <see cref="Country"/> of another <see cref="IPlayer"/>.  
        /// </summary>
        /// <param name="attackingCountryName"> The <see cref="Country"/> the currentPlayer wants to launch his attack from. </param>
        /// <param name="defendingCountryName"> The <see cref="Country"/> the currentPlayer wants to attack. </param>
        /// <param name="count"> The number of armies the currentPlayer wants to use for the attack. </param>
        void Attack(string attackingCountryName, string defendingCountryName, int count);

        /// <summary>
        /// The currentPlayer can move armies from one <see cref="Country"/> to another <see cref="Country"/> he owns.
        /// </summary>
        /// <param name="fromName"> The <see cref="Country"/> to move the armies from. </param>
        /// <param name="toName"> The <see cref="Country"/> to move the armies to. </param>
        /// <param name="count"> The number of armies to move. </param>
        void MoveArmies(string fromName, string toName, int count);
        
        /// <summary>
        /// The currentPlayer wants to go to the next <see cref="GameFase"/>.
        /// </summary>
        void GoToNextFase();
    }
}
