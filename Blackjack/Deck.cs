using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Deck
    {
        public int NumOfCardPacks { get; private set; }
        public readonly List<Card> CardsInDeckList = new List<Card>();

        public Deck(int cardPacksInDeck)
        {
            NumOfCardPacks = cardPacksInDeck;
            for (int i = 0; i < NumOfCardPacks; i++)
            {
                var pack = CreateCardPack();
                AddCardPackToDeck(pack);
            }
            ShuffleDeck();
        }

        static public List<Card> CreateCardPack()
        {
            var pack = new List<Card>();
            var cardsInPack = 52;
            var offset = 1;
            for (int i = offset; i < cardsInPack + offset; i++)
            {
                var suit = (CardSuits)(i % 4);
                var type = (CardTypes)(i % 13);
                var card = new Card(suit, type);
                pack.Add(card);
            }
            return pack;
        }

        public void AddCardPackToDeck(List<Card> pack)
        {
            var length = pack.Count;
            for (int i = 0; i < length; i++)
            {
                var card = pack[i];
                CardsInDeckList.Add(card);
            }
        }

        public void ShuffleDeck()
        {
            var list = CardsInDeckList;
            int n = list.Count;
            var rng = new Random();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public void PrintDeck()
        {
            var length = CardsInDeckList.Count;
            for (int i = 0; i < length; i++)
            {
                var card = CardsInDeckList[i];
                Logger.Write(card.GetCardDescription());
            }
            
        }
    }
}
