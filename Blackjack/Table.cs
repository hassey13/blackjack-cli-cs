using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Table
    {
        public Dealer TableDealer { get; private set; }
        public IGameRules GameMode { get; set; }
        public Deck CardDeck { get; private set; }
        public readonly List<Player> Players = new List<Player>();

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
    }
}
