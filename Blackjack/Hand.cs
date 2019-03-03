using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Hand
    {
        public readonly List<Card> Cards = new List<Card>();
        private Player _player { get; set; }

        public Hand(Player player)
        {
            _player = player;
        }

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

        public bool IsActive()
        {
            return Cards.Count > 0;
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
            Logger.SkipLine();
        }
        public void PrintDealerHand(bool hideSecondCard)
        {
            Logger.Write("Hand:");
            for (int i = 0; i < Cards.Count; i++)
            {
                var card = Cards[i];
                if (i == 1 && hideSecondCard)
                {
                    Logger.Write("Card is face down");
                }
                else
                {
                    Logger.Write(card.GetCardDescription());
                }

            }
            Logger.SkipLine();
        }

        public bool IsDealerHand()
        {
            return GetPlayerName() == "Dealer";
        }

        public string GetPlayerName()
        {
            return _player.Name;
        }

        public bool HasBlackjack()
        {
            if (Cards.Count != 2)
            {
                return false;
            }
            return (Cards[0].IsAce() && Cards[1].IsTenJQK()) || (Cards[0].IsTenJQK() && Cards[1].IsAce());
        }

        public bool IsDoubleDownAvailable()
        {
            return !HasUserPlayedAction();
        }

        public bool IsSplitAvailable()
        {
            return !HasUserPlayedAction() && Cards[0].GetCardType() == Cards[1].GetCardType();
        }

        public bool IsSurrenderHandAvailable()
        {
            return !HasUserPlayedAction();
        }

        public bool HasUserPlayedAction()
        {
            return Cards.Count != 2;
        }
    }
}
