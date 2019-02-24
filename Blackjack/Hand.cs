using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Hand
    {
        public readonly List<Card> Cards = new List<Card>();
        private Player _player { get; set; }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public Card ReturnLastCard()
        {
            if (Cards.Count == 0)
                throw new InvalidOperationException("Cannot return a card if none exist in hand.");
            return Cards[Cards.Count - 1];
        }

        public void RemoveLastCard()
        {
            Cards.RemoveAt(Cards.Count - 1);
        }

        public Card RemoveAndReturnLastCard()
        {
            var lastCard = ReturnLastCard();
            RemoveLastCard();
            return lastCard;
        }

        public void ClearHand()
        {
            Cards.RemoveAll(card => true);
        }

        public List<Card> ClearAndReturnHand()
        {
            var tempHandToReturn = new List<Card>(Cards);
            ClearHand();
            return tempHandToReturn;
        }

        public void PrintHand()
        {
            Logger.Write("Hand:");
            foreach (var card in Cards)
            {
                Logger.Write(card.GetCardDescription());
            }
        }
    }
}
