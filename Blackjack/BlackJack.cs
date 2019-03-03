using System;
using System.Collections.Generic;

namespace Blackjack
{
    class BlackJackRules : IGameRules
    {
        public int ReturnDeckCount()
        {
            return 1;
        }

        public int CalcHandValue(Hand hand)
        {
            var cards = hand.Cards;
            int value = 0;
            int aceCount = 0;
            foreach (var card in cards)
            {
                if (card.IsAce())
                {
                    aceCount += 1;
                }
                else
                {
                    value += card.GetValue();
                }
            }
            for (int i = 0; i < aceCount; i++)
            {
                if (value + CardValueDictionary.GetValue("Ace") > 21)
                {
                    value += 1;
                }
                else
                {

                    value += CardValueDictionary.GetValue("Ace");
                }
            }
            return value;
        }

        public bool IsHandSoft(Hand hand)
        {
            var cards = hand.Cards;
            var isSoft = false;
            int value = 0;
            int aceCount = 0;
            foreach (var card in cards)
            {
                if (card.IsAce())
                {
                    aceCount += 1;
                }
                else
                {
                    value += card.GetValue();
                }
            }
            for (int i = 0; i < aceCount; i++)
            {
                if (value + CardValueDictionary.GetValue("Ace") > 21)
                {
                    value += 1;
                }
                else
                {
                    isSoft = true;
                    value += CardValueDictionary.GetValue("Ace");
                }
            }
            return isSoft;
        }

        public bool DidHandBust(Hand hand)
        {
            return CalcHandValue(hand) > 21;
        }

        public bool ShouldDealerHit(Hand hand)
        {
            var handValue = CalcHandValue(hand);
            return handValue < 17 || (handValue == 17 && IsHandSoft(hand));
        }

        public bool ShouldOfferInsurance(Hand hand)
        {
            var handCards = hand.Cards;
            if (handCards.Count != 2)
            {
                throw new InvalidOperationException("Dealer did not have proper amount of cards to check for Blackjack.");
            }
            var cardsTypesShowingThatWouldOfferInsurance = new List<string>();
            cardsTypesShowingThatWouldOfferInsurance.Add("Ace");

            var showingCard = handCards[0];
            var shouldOfferInsurance = false;
            foreach (var cardType in cardsTypesShowingThatWouldOfferInsurance)
            {
                if (showingCard.GetCardType().ToString() == cardType)
                {
                    shouldOfferInsurance = true;
                }
            }
            return shouldOfferInsurance;
        }

        public bool ShouldCheckDealerHandForBlackjack(Hand hand)
        {
            var handCards = hand.Cards;
            if (handCards.Count != 2)
            {
                throw new InvalidOperationException("Dealer did not have proper amount of cards to check for Blackjack.");
            }
            var cardsTypesShowingThatWouldEnableBlackjack = new List<string>()
            {
                "Ace",
                "King",
                "Queen",
                "Jack",
                "Ten"
            };
            var showingCard = handCards[0];
            var couldHaveBlackjack = false;
            foreach (var cardType in cardsTypesShowingThatWouldEnableBlackjack)
            {
                if (showingCard.GetCardType().ToString() == cardType)
                {
                    couldHaveBlackjack = true;
                }
            }
            return couldHaveBlackjack;
        }
    }
}
