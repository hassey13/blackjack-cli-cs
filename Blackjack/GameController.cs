using System;

namespace Blackjack
{
    class GameController
    {
        public bool IsGameRunning { get; private set; }
        public Table GameTable { get; private set; }

        public GameController()
        {
            IsGameRunning = false;
        }

        public void StartGame()
        {
            IsGameRunning = true;
            while (IsGameRunning)
            {
                DisplayGameMenu();
            }
        }

        public void StopGame()
        {
            Logger.Write("Exitting Game...");
            IsGameRunning = false;
        }

        public void AddTable(Table table)
        {
            GameTable = table;
        }

        public void PrintGameMenu()
        {
            Logger.Linebreak();
            Logger.Write("Current players at table:");
            GameTable.PrintPlayers();
            Logger.Linebreak();
            Logger.Write("Main Menu");
            Logger.Write("Available options are:");
            Logger.Write("(1) Play a hand");
            Logger.Write("(2) Add player");
            Logger.Write("(q) Quit");
            Logger.Write("");
        }

        public void DisplayGameMenu()
        {
            PrintGameMenu();
            HandleGameMenuInput();
        }

        public void HandleGameMenuInput()
        {
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    HandleStartTurn();
                    break;
                case "2":
                    HandleAddPlayer();
                    break;
                case "q":
                    StopGame();
                    break;
                default:
                    DisplayGameMenu();
                    break;
            }
        }

        public void HandleStartTurn()
        {
            var dealer = GameTable.TableDealer;
            var players = GameTable.Players;
            GameTable.StartTurn();
        }

        public void HandleAddPlayer()
        {
            throw new NotImplementedException();
        }
    }
}
