namespace Blackjack
{
    public interface IGameRules
    {
        int ReturnDeckCount();
        int CalcHandValue(Hand hand);
        bool ShouldDealerHit(Hand hand);
        bool DidHandBust(Hand hand);
        bool ShouldOfferInsurance(Hand hand);
        bool ShouldCheckDealerHandForBlackjack(Hand hand);
    }
}
