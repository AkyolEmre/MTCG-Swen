using System;
using System.Linq;

namespace Monster_Trading_Cards_Game.Models
{
    public class Round
    {
        public User Player1 { get; }
        public User Player2 { get; }
        public User? Winner { get; private set; }

        public Round(User player1, User player2)
        {
            Player1 = player1;
            Player2 = player2;
        }

        public void Play()
        {
            int player1TotalDamage = Player1.Deck.Sum(card => card.Damage);
            int player2TotalDamage = Player2.Deck.Sum(card => card.Damage);

            Console.WriteLine($"{Player1.UserName} total damage: {player1TotalDamage}");
            Console.WriteLine($"{Player2.UserName} total damage: {player2TotalDamage}");

            Winner = DetermineRoundWinner(player1TotalDamage, player2TotalDamage);
        }

        private User? DetermineRoundWinner(int player1TotalDamage, int player2TotalDamage)
        {
            if (player1TotalDamage > player2TotalDamage)
            {
                Console.WriteLine($"Round winner: {Player1.UserName}");
                return Player1;
            }
            else if (player2TotalDamage > player1TotalDamage)
            {
                Console.WriteLine($"Round winner: {Player2.UserName}");
                return Player2;
            }
            else
            {
                Console.WriteLine("The round is a draw.");
                return null;
            }
        }
    }
}
