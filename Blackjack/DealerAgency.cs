using System.Collections.Generic;

namespace Blackjack
{
    class DealerAgency
    {
        private readonly List<Dealer> _dealersList = new List<Dealer>();

        public DealerAgency()
        {
            var alannaDealer = new Dealer()
            {
                Name = "Alanna",
                SkillLevel = 3
            };
            AddDealer(alannaDealer);

            var jeevesDealer = new Dealer()
            {
                Name = "Jeeves",
                SkillLevel = 10
            };
            AddDealer(jeevesDealer);
        }

        public void AddDealer(Dealer dealer)
        {
            _dealersList.Add(dealer);
        }

        public Dealer LoanDealer()
        {
            var loanDealer = _dealersList[0];
            _dealersList.RemoveAt(0);
            return loanDealer;
        }
    }
}
