using System;

namespace Blackjack
{
    class Input
    {
        static public bool GetInputYN(string message)
        {
            Logger.Write(message);
            Logger.Write("");
            var input = Console.ReadLine();
            if(input == "Yes" || input == "No" || input == "yes" || input == "y" || input == "no" || input == "n")
            {
                return input == "Yes" || input == "yes" || input == "y";
            }
            else
            {
                Logger.Write("Input was not a valid option. Please Try Again.");
                Logger.Linebreak();
                return GetInputYN(message);
            }
        }

        static public string GetInputString(string message)
        {
            Logger.Write(message);
            Logger.Write("");
            var input = Console.ReadLine();
            if (input.Length > 0)
            {
                return input;
            }
            else
            {
                Logger.Write("Enter Something Please!! Please Try Again.");
                Logger.Linebreak();
                return GetInputString(message);
            }
        }
    }
}
