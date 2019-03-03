using System;
using System.Threading;

namespace Blackjack
{
    public class Logger
    {
        static public void Write(string message)
        {
            Thread.Sleep(500);
            Console.WriteLine(message);
        }

        static public void Linebreak()
        {
            Console.WriteLine("--------------------");
            Console.WriteLine("");
        }

        static public void SkipLine()
        {
            Console.WriteLine("");
        }
    }
}
