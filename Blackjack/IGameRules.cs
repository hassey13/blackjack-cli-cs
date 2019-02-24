namespace Blackjack
{
    public interface IGameRules
    {
        int ReturnDeckCount();
        int ReturnHandValue(Hand hand);
    }
}
