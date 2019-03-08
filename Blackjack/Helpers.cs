namespace Blackjack
{
    static public class Helpers
    {
        static public string DeterminePlurality(int count, string word)
        {
            return count == 1 ? word : word + 's';
        }
    }
}
