//-------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="GameFase.cs">
// Taeke van der Veen april 2013
// </copyright>
// Visual Studio Express 2012 for Windows Desktop
//-------------------------------------------------------------------------------------------------------------------------------------------------

namespace WDGameEngine.Enums
{
    using WDGameEngine.Interfaces;

    /// <summary>
    /// Each turn a <see cref="IPlayer"/> goes through a couple of fases. This enum defines these fases.
    /// </summary>
    public enum GameFase
    {
        /// <summary>
        /// Before a call to StartGame there is no real GameFase.
        /// </summary>
        None,

        /// <summary>
        /// When the <see cref="IGame"/> is started the first turn the <see cref="IPlayer"/> must place the new armies he receives at the start of the game 
        /// on one or more of his <see cref="Country"/>. There is allready 1 amrie on each <see cref="Country"/> a player receives at the start of the game.
        /// </summary>
        PlaceInitialArmies,

        /// <summary>
        /// The <see cref="IPlayer"/> has to decide which <see cref="TurnType"/> he wants to play this time. He can choose this only once during each turn.
        /// </summary>
        ChooseTurnType,

        /// <summary>
        /// The <see cref="IPlayer"/> may choose to Exchanges <see cref="CardType"/> for new Armies. Every turn every <see cref="IPlayer"/> goes through this
        /// fase even if he doesn't own a combination of <see cref="CardType"/> he can exchanges.
        /// </summary>
        ExchangeCards,

        /// <summary>
        /// The <see cref="IPlayer"/> has got new Armies and is in the process of placing them. He must place all his new Armies before he can go to the 
        /// next fase. Which GameFase that is depents on the <see cref="TurnType"/>.
        /// </summary>
        PlaceNewArmies,

        /// <summary>
        /// The <see cref="IPlayer"/> has decided he wants to attack. He can get in this fase multiple times during 1 turn and will go to MoveArmiesAfterAttack if the
        /// attack was succesfull or stays in this fase if the attack was not succesfull. He can decide to place another attack or go to the MoveArmiesEndOfTurn fase.
        /// </summary>
        Attack,

        /// <summary>
        /// The <see cref="IPlayer"/> has succesfully attacked a <see cref="Country"/> and has to decide how many armies he wants to Move to the new Country. He can get 
        /// in this fase multiple times during one turn.
        /// </summary>
        MoveArmiesAfterAttack,

        /// <summary>
        /// The <see cref="IPlayer"/> has indicated he wants to end his attacks and still may move armies from one of his <see cref="Country"/> to another.
        /// He can only move armies once during eacht turn in this fase.
        /// </summary>
        MoveArmiesEndOfTurn
    }
}
