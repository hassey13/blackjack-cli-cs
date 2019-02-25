using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Turn
    {
        public Dealer TurnDealer { get; set; }
        public Deck TurnCardDeck { get; set; }
        public Deck TurnCardDeckDiscardPile { get; set; }

        public readonly List<Player> Players = new List<Player>();
        public readonly List<Hand> Hands = new List<Hand>();

        private bool _isStarted { get; set; }
        private bool _arePlayersLocked { get; set; }


        // ./ Add dealer
        // ./ Add all players 

        // ./ start hand
        // ./ marks _isStarted

        // sends a message to all players to create hands/ bets
        // once all players mark ready dealer, set _arePlayersLocked
        // if new deck show and bury card assign shuffle split card
        // starts dealing
        // Check Dealer BJ
        // if possible BJ with Ace showing offer insurance
        // Check Players BJ
        // Player Turns
        // Payout and take cards back
        // send cards to discard
        // Check Deck for shuffle
        // 

        public void AddPlayers(List<Player> players)
        {
            if (_isStarted)
            {
                throw new InvalidOperationException("Cannot add players when turn _isStarted.");
            }

            foreach (var player in players)
            {
                Players.Add(player);
                Logger.Write("Added " + player.Name);
            }
        }

        public void Start()
        {
            _isStarted = true;
            MessagePlayersToCreateHands();
            CreateDealerHand();
            DealHands();
        }

        public int ReturnHandCountAvailable()
        {
            return 1;
        }

        public int ReturnMinHandCount()
        {
            return 1;
        }

        public void MessagePlayersToCreateHands()
        {
            foreach (var player in Players)
            {
                Logger.Write(player.Name + ", how many hands will you be playing? (" + (ReturnHandCountAvailable()).ToString() + " hand availble)");
                PromptPlayerToCreateHands(player);
            }
        }

        public void PromptPlayerToCreateHands(Player player)
        {
            int numOfHands = Input.GetInputIntegerWithinMinMax("Enter a number: ", ReturnMinHandCount(), ReturnHandCountAvailable());
            for (int i = 0; i < numOfHands; i++)
            {
                CreatePlayerHand(player);
            }
        }

        public void CreatePlayerHand(Player player)
        {
            var hand = new Hand(player);
            Logger.Write("Hand created for " + player.Name);
            Hands.Add(hand);
        }

        public void CreateDealerHand()
        {
            var player = TurnDealer.DealerPlayer;
            var hand = new Hand(player);
            Logger.Write("Hand created for " + player.Name);
            Hands.Add(hand);
        }

        public void DealCardToHand(Hand hand)
        {
            var card = TurnCardDeck.DrawCard();
            hand.AddCard(card);
            Logger.Write("Dealing " + card.GetCardDescription() + " to " + hand.GetPlayerName());
        }

        public void DealHands()
        {
            var handsCount = Hands.Count;
            for (int i = 0; i < handsCount; i++)
            {
                var hand = Hands[i];
                DealCardToHand(hand);
            }
            for (int i = 0; i < handsCount; i++)
            {
                var hand = Hands[i];
                DealCardToHand(hand);
            }
        }

    }
}
