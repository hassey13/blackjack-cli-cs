using System;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var gameController = new GameController();
            var dealerAgency = new DealerAgency();
            var gameRules = new BlackJackRules();

            Logger.Write("Let\'s Play Blackjack!");
            Logger.Write("By: Eric Hasselbring");
            Logger.Linebreak();

            var firstTimePlayingMessage = "First Time Playing? (Yes/No)";
            var isFirstTime = Input.GetInputYN(firstTimePlayingMessage);
            Logger.Write("Great! " + (isFirstTime ? "You are going to have so much fun!" : "Welcome back!"));

            var getNameMessage = "What is your name?";
            var userInputName = Input.GetInputString(getNameMessage);
            Logger.Write("Hi " + userInputName + "! Thanks for playing!");
            var user = User.Create(userInputName);
            var player = new Player(user.Name);

            var table = new Table();
            table.AssignDeck(new Deck(gameRules.ReturnDeckCount()));
            table.AssignGameMode(gameRules);
            var dealer = dealerAgency.LoanDealer();
            table.AssignDealer(dealer);
            dealer.AssignTable(table);

            table.AddPlayer(player);
            Logger.Write(player.Name + " sat down at the table to play.");
            dealer.IntroduceSelf();

            gameController.AddTable(table);
            gameController.StartGame();

            Console.WriteLine("The End...");
            Console.ReadLine();
        }
    }
}
