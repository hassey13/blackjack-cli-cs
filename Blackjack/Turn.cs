using System;
using System.Collections.Generic;
using System.Threading;

namespace Blackjack
{
    public class Turn
    {
        public IGameRules GameRules { get; set; }
        public Dealer TurnDealer { get; set; }
        public Deck TurnCardDeck { get; set; }
        public List<Card> TurnCardDeckDiscardPile { get; set; }

        public readonly List<Player> Players = new List<Player>();
        public readonly List<Hand> Hands = new List<Hand>();

        private bool _isStarted { get; set; }
        private bool _arePlayersLocked { get; set; }


        // ./ Add dealer
        // ./ Add all players 

        // ./ start hand
        // ./ marks _isStarted

        // sends a message to all players to create hands/ bets
        // once all players mark ready dealer, set _arePlayersLocked
        // if new deck show and bury card assign shuffle split card

        // ./ starts dealing
        // ./ Check Dealer BJ
        // ./ if possible BJ with Ace showing offer insurance
        // ./ Check Players BJ
        // ./ Player Turns
        // ./ Dealer Turn
        // Payout
        // ./ take cards back
        // ./ send cards to discard
        // Validate all states cleared
        // Check Deck for shuffle
        // ./ End Turn

        public void Start()
        {
            _isStarted = true;
            MessagePlayersToCreateHands();
            CreateDealerHand();
            DealHands();
            if (GameRules.ShouldOfferInsurance(GetDealerHand()))
            {
                OfferEvenMoneyToPlayersWithBlackjackIfAvailable();
                OfferInsuranceToPlayers();
            }
            if (GameRules.ShouldCheckDealerHandForBlackjack(GetDealerHand()))
            {
                Logger.Write("Dealer is checking for Blackjack.");
                Thread.Sleep(1000);
                if (!DoesDealerHaveBlackjack())
                {
                    Logger.Write("Dealer does not have Blackjack.");
                    Logger.SkipLine();
                }
            }
            if (DoesDealerHaveBlackjack())
            {
                Logger.Write("Dealer has Blackjack!");
                GetDealerHand().PrintDealerHand(false);
                Thread.Sleep(1500);
                PayoutInsurance();
                ClearHandsAndHandlePayouts();
            }
            else
            {
                HandleAnyPlayerBlackjacks();
                PlayPlayerHands();
                PlayDealerHand();
                HandleAnyPayOuts();
                ValidateAllActionsCarriedOut();
            }
            EndTurn();
        }

        public void EndTurn()
        {
            // Not sure what might go here
        }

        public void AddPlayers(List<Player> players)
        {
            if (_isStarted)
            {
                throw new InvalidOperationException("Cannot add players when turn _isStarted.");
            }

            foreach (var player in players)
            {
                Players.Add(player);
                Logger.Write("Added " + player.Name);
            }
            Logger.SkipLine();
        }

        public int ReturnHandCountAvailable()
        {
            return 6;
        }

        public int ReturnMinHandCount()
        {
            // TODO: implement
            return 1;
        }

        public void HandleAnyPayOuts()
        {
            var dealerHand = GetDealerHand();
            var isLoopingThruPlayerHands = true;
            var i = 0;
            while (isLoopingThruPlayerHands)
            {
                var hand = Hands[i];
                if (hand.IsDealerHand())
                {
                    isLoopingThruPlayerHands = false;
                    break;
                }
                else
                {
                    if (hand.IsActive())
                    {
                        if (GameRules.DidHandBust(dealerHand) || (GameRules.CalcHandValue(hand) > GameRules.CalcHandValue(dealerHand)))
                        {
                            Logger.Write("You won! Congrats!");
                        }
                        else if (GameRules.CalcHandValue(hand) == GameRules.CalcHandValue(dealerHand))
                        {
                            Logger.Write("You drew!");
                        }
                        else
                        {
                            Logger.Write("You lost!");
                        }
                    }
                    i += 1;
                }
            }
        }

        public void ValidateAllActionsCarriedOut()
        {
            // check hands for cards
            // check no insurance unpaid
            // check dealer has no cards
        }

        public bool IsAPlayerHandActive()
        {
            var isAHandActive = false;
            var isLoopingThruPlayerHands = true;
            var i = 0;
            while (isLoopingThruPlayerHands)
            {
                var hand = Hands[i];
                if (hand.IsDealerHand())
                {
                    isLoopingThruPlayerHands = false;
                    break;
                }
                else
                {
                    if (hand.IsActive())
                    {
                        isAHandActive = true;
                    }
                    i += 1;
                }
            }
            return isAHandActive;
        }

        public void PlayDealerHand()
        {
            var dealerHand = GetDealerHand();
            Logger.Write("Dealer has:");
            dealerHand.PrintHand();
            if (IsAPlayerHandActive())
            {
                var continueLoop = true;
                while (continueLoop)
                {
                    if (GameRules.ShouldDealerHit(dealerHand))
                    {
                        DealCardToHand(dealerHand, false);
                        Thread.Sleep(1000); // Pause for effect
                    }
                    else
                    {
                        continueLoop = false;
                        if (GameRules.DidHandBust(dealerHand))
                        {
                            Logger.Write("Dealer busted.");
                        }
                    }
                }
            }
            else
            {
                Logger.Write("Dealer does not play hand since no player hands still in play.");
            }
        }

        public void OfferInsuranceToPlayers()
        {
            var isLoopingThruHand = true;
            var i = 0;
            while (isLoopingThruHand)
            {
                var hand = Hands[i];
                if (hand.IsDealerHand())
                {
                    isLoopingThruHand = false;
                    break;
                }
                else
                {
                    OfferPlayerWithHandInsurance(hand);
                    i += 1;
                }
            }
        }

        public void OfferEvenMoneyToPlayersWithBlackjackIfAvailable()
        {
            var isLoopingThruHand = true;
            var i = 0;
            while (isLoopingThruHand)
            {
                var hand = Hands[i];
                if (hand.IsDealerHand())
                {
                    isLoopingThruHand = false;
                    break;
                }
                else
                {
                    if (hand.HasBlackjack())
                    {
                        OfferPlayerWithBlackjackEvenMoney(hand);
                    }
                    i += 1;
                }
            }
            Logger.SkipLine();
        }

        public void OfferPlayerWithBlackjackEvenMoney(Hand hand)
        {
            var message = hand.GetPlayerName() + " do you want even money?";
            var didAcceptEvenMoney = Input.GetInputYN(message);
            if (didAcceptEvenMoney)
            {
                Logger.Write(hand.GetPlayerName() + " chose even money.");
                // TODO: Add payout
                ClearHand(hand);
            }
            else
            {
                Logger.Write(hand.GetPlayerName() + " rejected even money.");
            }
        }

        public void OfferPlayerWithHandInsurance(Hand hand)
        {
            // TODO: Implement
            hand.PrintHand();
            var message = hand.GetPlayerName() + " do you want insurance?";
            var didAcceptEvenMoney = Input.GetInputYN(message);
            if (didAcceptEvenMoney)
            {
                Logger.Write(hand.GetPlayerName() + " chose insurance.");
            }
            else
            {
                Logger.Write(hand.GetPlayerName() + " rejected insurance.");
            }
        }

        public void PlayPlayerHands()
        {
            var isUnplayedPlayerHands = true;
            var i = 0;
            while (isUnplayedPlayerHands)
            {
                var hand = Hands[i];
                Logger.Write("Playing hand for: " + hand.GetPlayerName());
                if (hand.IsDealerHand())
                {
                    isUnplayedPlayerHands = false;
                    break;
                }
                else
                {
                    if (hand.IsActive() && !GameRules.SkipPlayOfHand(hand))
                    {
                        PlayHand(hand);
                    }
                    i += 1;
                }
            }
        }

        public void HandleAnyPlayerBlackjacks()
        {
            var isLoopingThruHands = true;
            var i = 0;
            while (isLoopingThruHands)
            {
                var hand = Hands[i];
                if (hand.IsDealerHand())
                {
                    isLoopingThruHands = false;
                    break;
                }
                else
                {
                    if (hand.HasBlackjack())
                    {
                        Logger.Write(hand.GetPlayerName() + " congrats you have Blackjack!");
                        ClearHand(hand);
                    }
                    i += 1;
                }
            }
        }

        public void PlayHand(Hand hand)
        {
            PromptPlayerForHandAction(hand);
        }

        public void PromptPlayerForHandAction(Hand hand)
        {
            Logger.Write(hand.GetPlayerName() + " it is your turn.");
            Logger.SkipLine();
            Logger.Write("Dealer is showing:");
            GetDealerHand().PrintDealerHand(true);
            Logger.SkipLine();
            Logger.Write(hand.GetPlayerName() + " has:");
            hand.PrintHand();
            Logger.Write("Hand value: " + GameRules.CalcHandValue(hand));
            Logger.SkipLine();
            Logger.Write("The following actions are available:");
            PrintActionsListForHand(hand);
            var playerActionInput = Input.GetInputString("Please select an action:");
            HandlePlayerHandAction(hand, playerActionInput);
        }

        public void ClearHand(Hand hand)
        {
            var cards = hand.ClearAndReturnHand();
            foreach (var card in cards)
            {
                TurnCardDeckDiscardPile.Add(card);
            }
        }

        public void HandleActionHit(Hand hand)
        {
            Logger.Write(hand.GetPlayerName() + " chose to hit");
            DealCardToHand(hand, false);
            Logger.SkipLine();
            if (GameRules.DidHandBust(hand))
            {
                Logger.Write(hand.GetPlayerName() + " busted.");
                Logger.SkipLine();
                // TODO: take bet
                ClearHand(hand);
            }
            else if (GameRules.CalcHandValue(hand) == 21)
            {
                Logger.Write("Congrats! You got 21");
            }
            else
            {
                PromptPlayerForHandAction(hand);
            }
        }

        public void HandleActionStand(Hand hand)
        {
            Logger.Write(hand.GetPlayerName() + " chose to stand");
            Logger.SkipLine();
        }

        public int GetIndexOfHand(Hand hand)
        {
            var handIndex = -1;
            var loopIndex = 0;
            while (handIndex == -1 || loopIndex < Hands.Count)
            {
                var iterationHand = Hands[loopIndex];
                if (hand == iterationHand)
                {
                    handIndex = loopIndex;
                }
                loopIndex += 1;
            }
            if (handIndex == -1)
            {
                throw new Exception("Could not find hand to determine index.");
            }
            return handIndex;
        }

        public void HandleActionSplit(Hand hand)
        {
            // TODO: check if has enough money to split
            var splitCard = hand.RemoveAndReturnLastCard();
            var newHand = new Hand(hand.GetPlayer())
            {
                isSplitHand = true
            };
            newHand.AddCard(splitCard);
            var indexForNewHand = GetIndexOfHand(hand) + 1;
            Hands.Insert(indexForNewHand, newHand);
            Logger.Write(hand.GetPlayerName() + " chose to split");
            DealCardToHand(hand, false);
            if (splitCard.IsAce())
            {
                DealCardToHand(newHand, false);
                Logger.SkipLine();
                Logger.Write(hand.GetPlayerName() + " now has two hands with:");
                hand.PrintHand();
                newHand.PrintHand();
            }
            else
            {
                PromptPlayerForHandAction(hand);
            }
        }

        public void HandleActionDoubleDown(Hand hand)
        {
            // TODO: check if has money to double, ask for bet amount cannot exceed original bet size
            Logger.Write(hand.GetPlayerName() + " chose to double down");
            DealCardToHand(hand, false);
            if (GameRules.DidHandBust(hand))
            {
                Logger.Write(hand.GetPlayerName() + " busted.");
                ClearHand(hand);
            }
            else if (GameRules.CalcHandValue(hand) == 21)
            {
                Logger.Write("Congrats! You got 21");
            }
            Logger.SkipLine();
        }

        public void HandleActionSurrenderHand(Hand hand)
        {
            Logger.Write(hand.GetPlayerName() + " chose to surrender their hand");
            // TODO: take half bet and give half back
            ClearHand(hand);
        }

        public void HandleActionAskForAdvice(Hand hand)
        {
            Logger.Write(hand.GetPlayerName() + " is looking for advice on how to play this hand.");
            Logger.Write(TurnDealer.Name + " says \"" + hand.GetPlayerName() + ", you should " +  TurnDealer.GiveAdviceOnHand(hand) + ".\" ");
        }

        public void HandlePlayerHandAction(Hand hand, string action)
        {
            switch (action.ToLower())
            {
                case "hit":
                    HandleActionHit(hand);
                    break;
                case "stand":
                    HandleActionStand(hand);
                    break;
                case "split":
                    if (GameRules.SplitHandAvailable(hand))
                    {
                        HandleActionSplit(hand);
                    }
                    else
                    {
                        PromptPlayerForHandAction(hand);
                    }
                    break;
                case "double down":
                    HandleActionDoubleDown(hand);
                    break;
                case "surrender":
                    HandleActionSurrenderHand(hand);
                    break;
                case "ask advice":
                    HandleActionAskForAdvice(hand);
                    break;
                default:
                    PromptPlayerForHandAction(hand);
                    break;
            }
        }

        public void PrintActionsListForHand(Hand hand)
        {
            Logger.Write("Hit");
            Logger.Write("Stand");
            if (hand.IsDoubleDownAvailable())
            {
                Logger.Write("Double Down");
            }
            if (GameRules.SplitHandAvailable(hand))
            {
                Logger.Write("Split");
            }
            if (hand.IsSurrenderHandAvailable())
            {
                Logger.Write("Surrender Hand (Receive half of wager back)");
            }
            Logger.Write("Ask Advice");

        }

        public void MessagePlayersToCreateHands()
        {
            foreach (var player in Players)
            {
                var countHandsAvailable = ReturnHandCountAvailable();
                Logger.Write(player.Name + ", how many hands will you be playing? (" + countHandsAvailable.ToString() + " " + Helpers.DeterminePlurality(countHandsAvailable, "hand") + "availble)");
                PromptPlayerToCreateHands(player);
            }
        }

        public void PromptPlayerToCreateHands(Player player)
        {
            int numOfHands = Input.GetInputIntegerWithinMinMax("Enter a number: ", ReturnMinHandCount(), ReturnHandCountAvailable());
            for (int i = 0; i < numOfHands; i++)
            {
                CreatePlayerHand(player);
            }
        }

        public void CreatePlayerHand(Player player)
        {
            var hand = new Hand(player);
            Logger.Write("Hand created for " + player.Name);
            Hands.Add(hand);
        }

        public void CreateDealerHand()
        {
            var player = TurnDealer.DealerPlayer;
            var hand = new Hand(player);
            Logger.Write("Hand created for " + player.Name);
            Hands.Add(hand);
            Logger.Linebreak();
        }

        public void DealCardToHand(Hand hand, bool shouldHideDealerCard)
        {
            var card = TurnCardDeck.DrawCard();
            hand.AddCard(card);
            if (shouldHideDealerCard && hand.GetPlayerName() == "Dealer")
            {
                Logger.Write("Dealing " + "card" + " to " + hand.GetPlayerName());
                Thread.Sleep(500);
            }
            else
            {
                Logger.Write("Dealing " + card.GetCardDescription() + " to " + hand.GetPlayerName());
                Thread.Sleep(500);
            }
        }

        public void DealHands()
        {
            var handsCount = Hands.Count;
            for (int i = 0; i < handsCount; i++)
            {
                var hand = Hands[i];
                DealCardToHand(hand, false);
            }
            for (int i = 0; i < handsCount; i++)
            {
                var hand = Hands[i];
                DealCardToHand(hand, true);
            }
            Logger.Linebreak();
        }

        public void ClearHandsAndHandlePayouts()
        {
            var isLoopingThruPlayerHands = true;
            var i = 0;
            while (isLoopingThruPlayerHands)
            {
                var hand = Hands[i];
                if (hand.IsDealerHand())
                {
                    isLoopingThruPlayerHands = false;
                    ClearHand(hand);
                    break;
                }
                else
                {
                    if (hand.IsActive())
                    {
                        ClearHand(hand);
                    }
                    i += 1;
                }
            }
        }

        public void PayoutInsurance()
        {
            // Implement
            Logger.Write("Should have gotten that insurance...");
        }

        public bool DoesDealerHaveBlackjack()
        {
            return GetDealerHand().HasBlackjack();
        }

        public bool ShouldOfferInsurance()
        {
            var hand = GetDealerHand();
            var visibleDealerCard = hand.Cards[0];
            return visibleDealerCard.IsAce();
        }

        public Hand GetDealerHand()
        {
            // TODO: vague assumption
            return Hands[Hands.Count - 1];
        }

        public void PrintDealerHand()
        {
            Logger.Write("Dealer has: ");
            GetDealerHand().PrintHand();
        }

    }
}
