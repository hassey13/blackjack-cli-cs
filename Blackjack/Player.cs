namespace Blackjack
{
    public class Player
    {
        public string Name { get; private set; }
        public readonly Hand PlayerHand = new Hand();

        public Player(string name)
        {
            Name = name;
        }
    }
}
