using System;
using System.Collections.Generic;

namespace Monster_Trading_Cards_Game.Models
{
    /// <summary>
    /// Repräsentiert einen Kampf zwischen zwei Spielern.
    /// </summary>
    public class Battle
    {
        public User Player1 { get; private set; }
        public User Player2 { get; private set; }
        public User? Winner { get; private set; }

        private List<Round> Rounds { get; set; }

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="Battle"/>-Klasse.
        /// </summary>
        /// <param name="player1">Der erste Spieler.</param>
        /// <param name="player2">Der zweite Spieler.</param>
        public Battle(User player1, User player2)
        {
            Player1 = player1;
            Player2 = player2;
            Rounds = new List<Round>();
        }

        /// <summary>
        /// Startet den Kampf zwischen den beiden Spielern.
        /// </summary>
        public void Start()
        {
            Console.WriteLine($"Kampf gestartet zwischen {Player1.UserName} und {Player2.UserName}!");

            int player1Wins = 0;
            int player2Wins = 0;

            // Beispiel: Durchführung von 3 Runden
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"Starte Runde {i + 1}...");
                Round round = new Round(Player1, Player2);
                round.Play();
                Rounds.Add(round);

                if (round.Winner == Player1)
                {
                    player1Wins++;
                }
                else if (round.Winner == Player2)
                {
                    player2Wins++;
                }
            }

            // Bestimme den Gesamtsieger
            if (player1Wins > player2Wins)
            {
                Winner = Player1;
                Console.WriteLine($"Der Gewinner des Kampfes ist: {Player1.UserName}");
            }
            else if (player2Wins > player1Wins)
            {
                Winner = Player2;
                Console.WriteLine($"Der Gewinner des Kampfes ist: {Player2.UserName}");
            }
            else
            {
                Winner = null;
                Console.WriteLine("Der Kampf endet unentschieden!");
            }
        }
    }
}