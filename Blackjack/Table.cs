using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Table
    {
        public Dealer TableDealer { get; private set; }
        public IGameRules GameMode { get; set; }

        public Deck CardDeck { get; private set; }
        public List<Card> CardDeckDiscardPile = new List<Card>();

        public readonly List<Player> Players = new List<Player>();
        public Player PlayerToSplitDeck { get; set; }

        public readonly List<Turn> TurnHistory = new List<Turn>();

        public void AssignGameMode(IGameRules gameMode)
        {
            GameMode = gameMode;
        }

        public void AssignDeck(Deck deck)
        {
            CardDeck = deck;
        }

        public void AssignDealer(Dealer dealer)
        {
            if (!(GameMode is IGameRules))
                throw new InvalidOperationException("Must assign a GameMode to table before assigning a Dealer");

            TableDealer = dealer;
            TableDealer.AssignGameMode(GameMode);
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public void AddTurnToTurnHistory(Turn turn)
        {
            TurnHistory.Add(turn);
        }

        public void StartTurn()
        {
            Logger.Write("Starting Turn.");
            var turn = new Turn()
            {
                GameRules = GameMode,
                TurnDealer = TableDealer,
                TurnCardDeck = CardDeck,
                TurnCardDeckDiscardPile = CardDeckDiscardPile
            };
            Logger.Write("Adding Players");
            turn.AddPlayers(Players);
            turn.Start();
        }

        public void PrintPlayers()
        {
            var PlayerCount = Players.Count;
            var PlayerNames = new String[PlayerCount];
            for (int i = 0; i < PlayerCount; i++)
            {
                var player = Players[i];
                PlayerNames[i] = player.Name;
            }
            Logger.Write(String.Join(", ", PlayerNames));
        }
    }
}
