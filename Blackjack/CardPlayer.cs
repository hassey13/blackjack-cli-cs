using System.Collections.Generic;

namespace Blackjack
{
    class CardPlayer
    {
        public readonly List<Hand> Hands = new List<Hand>();

        public void ReceiveCard(Card card, Hand hand)
        {
            hand.AddCard(card);
        }

        public void AddHand(Hand hand)
        {
            Hands.Add(hand);
        }

        public void RemoveHand(Hand hand)
        {

        }

        public List<Card> RemoveHandAndReturnCards(Hand hand)
        {
            var cards = hand.ClearAndReturnHand();
            Hands.RemoveAll(handInHands => handInHands == hand);
            return cards; 
        }

    }
}
