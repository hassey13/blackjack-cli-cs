namespace Blackjack
{
    public interface IGameRules
    {
        int ReturnDeckCount();
        int CalcCardValue(Card card);
        int CalcHandValue(Hand hand);
        bool ShouldDealerHit(Hand hand);
        bool DidHandBust(Hand hand);
        bool ShouldOfferInsurance(Hand hand);
        bool ShouldCheckDealerHandForBlackjack(Hand hand);
        bool SkipPlayOfHand(Hand hand);
        bool SplitHandAvailable(Hand hand);
    }
}
