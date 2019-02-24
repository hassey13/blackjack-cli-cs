using System;

namespace Blackjack
{
    public class Logger
    {
        static public void Write(string message)
        {
            Console.WriteLine(message);
        }

        static public void Linebreak()
        {
            Console.WriteLine("--------------------");
            Console.WriteLine("");
        }
    }
}
