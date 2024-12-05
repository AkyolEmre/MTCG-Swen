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
            if (e.Path.TrimEnd('/', ' ', '\t') == "/sessions" && e.Method == "POST")
            {
                return HandleSessionPost(e);
            }
            return false;
        }

        private bool HandleSessionPost(HttpSvrEventArgs e)
        {
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

                    e.Reply(HttpStatusCode.UNAUTHORIZED, CreateResponse(false, "Logon failed.").ToJsonString());
                }
                else
                {
                    e.Reply(HttpStatusCode.BAD_REQUEST, CreateResponse(false, "Invalid request format.").ToJsonString());
                }
            }
            catch
            {
                e.Reply(HttpStatusCode.BAD_REQUEST, CreateResponse(false, "Invalid request.").ToJsonString());
            }
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

            e.Reply(HttpStatusCode.OK, reply.ToJsonString(), new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {token}" }
            });
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
