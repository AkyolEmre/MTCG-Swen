using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security;
using Monster_Trading_Cards_Game.Exceptions;

namespace Monster_Trading_Cards_Game {
    public sealed class User
    {
        private static readonly Dictionary<string, User> Users = new();
        private User() { }
        public string UserName { get; private set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
        public string? SessionToken { get; set; }

        public void Save(string token)
        {
            var auth = Token.Authenticate(token);
            if (!auth.Success) throw new AuthenticationException("Not authenticated.");
            if (auth.User!.UserName != UserName) throw new SecurityException("Trying to change other user's data.");
            // Save logic here
        }

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
