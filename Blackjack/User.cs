using System;

namespace Blackjack
{
    class User
    {
        public string Name { get; private set; }
        public Player UserPlayer { get; private set; }

        public User(string name)
        {
            Name = name;
        }

        static public User Create(string name)
        {
            return new User(name);
        }

        static public User Login(string name, string pw)
        {
            throw new NotImplementedException();
            // return new User(name);
        }
    }
}
