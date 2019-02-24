using System;

namespace Blackjack
{
    class BlackJack : IGameRules
    {
        public int ReturnDeckCount()
        {
            return 1;
        }

        public int ReturnHandValue(Hand hand)
        {
            throw new NotImplementedException();
        }
    }
}
