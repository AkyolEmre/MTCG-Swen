using System;
using System.Collections.Generic;

namespace Monster_Trading_Cards_Game.Models
{
    public class Battle
    {
        public User Player1 { get; }
        public User Player2 { get; }
        public User? Winner { get; private set; }
        private readonly List<Round> _rounds;
        private const int RoundsCount = 3;

        public Battle(User player1, User player2)
        {
            Player1 = player1;
            Player2 = player2;
            _rounds = new List<Round>();
        }

        public void Start()
        {
            Console.WriteLine($"Battle between {Player1.UserName} and {Player2.UserName} is about to start!");

            int player1Wins = 0, player2Wins = 0;

            for (int i = 0; i < RoundsCount; i++)
            {
                Console.WriteLine($"Commencing Round {i + 1}...");
                var round = new Round(Player1, Player2);
                round.Play();
                _rounds.Add(round);

                if (round.Winner == Player1) player1Wins++;
                else if (round.Winner == Player2) player2Wins++;
            }

            Winner = DetermineWinner(player1Wins, player2Wins);
        }

        private User? DetermineWinner(int player1Wins, int player2Wins)
        {
            if (player1Wins > player2Wins)
            {
                Console.WriteLine($"Victor: {Player1.UserName}");
                return Player1;
            }
            else if (player2Wins > player1Wins)
            {
                Console.WriteLine($"Victor: {Player2.UserName}");
                return Player2;
            }
            else
            {
                Console.WriteLine("The battle is a stalemate!");
                return null;
            }
        }
    }
}
