using Monster_Trading_Cards_Game;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace Monster_Trading_Cards_Game
{
    public class SessionHandler : Handler, IHandler
    {
        private static readonly Dictionary<string, string> ActiveSessions = new();

        public override bool Handle(HttpSvrEventArgs e)
        {
            // Überprüfe auf den /sessions-Endpunkt mit POST-Methode
            if (e.Path.TrimEnd('/', ' ', '\t') == "/sessions" && e.Method == "POST")
            {
                return HandleSessionPost(e);
            }

            return false;
        }

        private bool HandleSessionPost(HttpSvrEventArgs e)
        {
            JsonObject? reply;
            int status = HttpStatusCode.BAD_REQUEST;

            try
            {
                JsonNode? json = JsonNode.Parse(e.Payload);
                if (json != null)
                {
                    string username = json["username"]!.ToString();
                    string password = json["password"]!.ToString();

                    var result = User.Logon(username, password);

                    if (result.Success)
                    {
                        return HandleSuccessfulLogin(e, result.Token, username);
                    }
                    else
                    {
                        status = HttpStatusCode.UNAUTHORIZED;
                        reply = CreateResponse(false, "Logon failed.");
                    }
                }
                else
                {
                    reply = CreateResponse(false, "Invalid request format.");
                }
            }
            catch (Exception)
            {
                reply = CreateResponse(false, "Invalid request.");
            }

            e.Reply(status, reply?.ToJsonString());
            return true;
        }

        private bool HandleSuccessfulLogin(HttpSvrEventArgs e, string token, string username)
        {
            ActiveSessions[token] = username;

            var reply = new JsonObject
            {
                ["success"] = true,
                ["message"] = "User logged in.",
                ["token"] = token
            };

            var headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {token}" }
            };

            e.Reply(HttpStatusCode.OK, reply.ToJsonString(), headers);
            return true;
        }

        private static JsonObject CreateResponse(bool success, string message)
        {
            return new JsonObject
            {
                ["success"] = success,
                ["message"] = message
            };
        }

        public static string? GetUsernameFromToken(string token)
        {
            return ActiveSessions.TryGetValue(token, out var username) ? username : null;
        }
    }
}
