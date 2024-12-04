using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security;
using Monster_Trading_Cards_Game.Exceptions;

namespace Monster_Trading_Cards_Game.Models
{
    public sealed class User
    {
        private static readonly Dictionary<string, User> Users = new();

        private User() { }

        public string UserName { get; private set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
        public int Coins { get; set; } = 20;
        public List<Card> Stack { get; set; } = new();
        public List<Card> Deck { get; set; } = new();
        public string? SessionToken { get; set; }

        private static readonly List<string> CardNames = new()
        {
            "Goblins", "Dragons", "Wizzard", "Knights", "Orks", "Kraken", "FireElves", "Lion", "DogMike", "Rocklee",
            "Tetsu", "Amaterasu", "Bankai", "Raijin", "Susanoo", "FighterKevin"
        };

        public void Save(string token)
        {
            var auth = Token.Authenticate(token);
            if (!auth.Success) throw new AuthenticationException("Not authenticated.");
            if (auth.User!.UserName != UserName) throw new SecurityException("Trying to change other user's data.");
            // Save logic here
        }

        public void AddPackage()
        {
            if (Coins < 5) throw new Exception("Insufficient coins. Need at least 5 coins.");

            Coins -= 5;
            var rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                var cardName = CardNames[rand.Next(CardNames.Count)];
                Stack.Add(CreateCard(cardName));
            }
        }

        public void ChooseDeck()
        {
            Deck = Stack.OrderByDescending(card => card.Damage)
                        .ThenBy(card => card.CardElementType == ElementType.Water ? 1 : card.CardElementType == ElementType.Fire ? 2 : 3)
                        .ThenByDescending(card => card.GetType().Name)
                        .Take(4).ToList();
        }

        private static Card CreateCard(string cardName) => cardName switch
        {
            "Dragons" or "FireElves" or "Kraken" or "Lion" => new MonsterCard(cardName),
            "Wizzard" or "Tetsu" or "Amaterasu" or "Bankai" => new SpellCard(cardName),
            _ => new NormalCard(cardName)
        };

        public static void Create(string userName, string password, string fullName = "", string eMail = "")
        {
            if (Users.ContainsKey(userName)) throw new UserException("User name already exists.");
            var user = new User
            {
                UserName = userName,
                FullName = fullName,
                EMail = eMail
            };
            Users.Add(userName, user);
        }

        public static (bool Success, string Token) Logon(string userName, string password)
        {
            if (!Users.TryGetValue(userName, out var user)) return (false, string.Empty);
            var token = Token._CreateTokenFor(user);
            user.SessionToken = token;
            return (true, token);
        }

        public static bool Exists(string userName) => Users.ContainsKey(userName);

        public static User? Get(string userName) => Users.TryGetValue(userName, out var user) ? user : null;

        public static IEnumerable<User> GetAllUsers() => Users.Values;
    }
}
