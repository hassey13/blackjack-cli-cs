namespace Blackjack
{
    public class Card
    {
        public CardSuits _cardSuit { get; private set; }
        public CardTypes _cardType { get; private set; }

        public Card(CardSuits cardSuit, CardTypes cardType)
        {
            _cardSuit = cardSuit;
            _cardType = cardType;
        }

        public string GetCardDescription()
        {
            var prefix = UseAn() ? "an " : "a ";
            return prefix + this._cardType.ToString() + " of " + this.GetCardSuitIcon() + " " + this._cardSuit.ToString();
        }

        public string GetCardSuitIcon()
        {
            var icon = "";
            int suitEnumInt = (int)_cardSuit;
            switch (suitEnumInt)
            {
                case 0:
                    icon = "♥";
                    break;
                case 1:
                    icon = "♦";
                    break;
                case 2:
                    icon = "♣";
                    break;
                case 3:
                    icon = "♠";
                    break;
                default:
                    throw new ArgumentException("A valid suit enum must be supplied.");
            }
            return icon;
        }

        private bool UseAn()
        {
            var type = this._cardType;
            return (int)type == 0 || (int)type == 7;
        }
    }
}
