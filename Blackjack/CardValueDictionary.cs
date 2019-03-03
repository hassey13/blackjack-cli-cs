using System;
using System.Collections.Generic;

namespace Blackjack
{
    public static class CardValueDictionary
    {
            static Dictionary<string, int> _dict = new Dictionary<string, int>()
            {
                { "Ace", 11 },
                { "Two", 2 },
                { "Three", 3 },
                { "Four", 4 },
                { "Five", 5 },
                { "Six", 6 },
                { "Seven", 7 },
                { "Eight", 8 },
                { "Nine", 9 },
                { "Ten", 10 },
                { "Jack", 10 },
                { "Queen", 10 },
                { "King", 10 }
            };

        public static int GetValue(string cardType)
        {
            if (_dict.TryGetValue(cardType, out int value))
            {
                return value;
            }
            else
            {
                throw new Exception("Could not find value for cardType");
            }
        }
    }
}
