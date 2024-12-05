using Monster_Trading_Cards_Game;
using Monster_Trading_Cards_Game.Exceptions;
using System;
using System.Text.Json.Nodes;

namespace Monster_Trading_Cards_Game
{
    /// <summary>Handler für Benutzer-spezifische Anfragen.</summary>
    public class UserHandler : Handler, IHandler
    {
        /// <summary>Bearbeitet eingehende HTTP-Anfragen.</summary>
        /// <param name="e">Ereignisargumente.</param>
        public override bool Handle(HttpSvrEventArgs e)
        {
            JsonObject? reply = null;
            int status = HttpStatusCode.BAD_REQUEST;

            string trimmedPath = e.Path.TrimEnd('/', ' ', '\t');

            if (trimmedPath == "/users" && e.Method == "POST")
            {
                return HandleUserCreation(e);
            }
            else if (e.Path == "/users/me" && e.Method == "GET")
            {
                return HandleGetCurrentUser(e);
            }
            else if (e.Path.StartsWith("/users"))
            {
                return HandleUserSpecificRequests(e);
            }

            return false;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Private Methoden                                                                                                 //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Bearbeitet das Erstellen eines neuen Benutzers.</summary>
        private bool HandleUserCreation(HttpSvrEventArgs e)
        {
            JsonObject? reply = null;
            int status = HttpStatusCode.BAD_REQUEST;

            try
            {
                JsonNode? json = JsonNode.Parse(e.Payload);
                if (json != null)
                {
                    User.Create(
                        (string)json["username"]!,
                        (string)json["password"]!,
                        (string?)json["fullname"] ?? "",
                        (string?)json["email"] ?? ""
                    );

                    status = HttpStatusCode.OK;
                    reply = new JsonObject
                    {
                        ["success"] = true,
                        ["message"] = "User created."
                    };
                }
            }
            catch (UserException ex)
            {
                reply = new JsonObject
                {
                    ["success"] = false,
                    ["message"] = ex.Message
                };
            }
            catch
            {
                reply = new JsonObject
                {
                    ["success"] = false,
                    ["message"] = "Invalid request."
                };
            }

            e.Reply(status, reply?.ToJsonString());
            return true;
        }

        /// <summary>Gibt Informationen über den aktuellen Benutzer zurück.</summary>
        private bool HandleGetCurrentUser(HttpSvrEventArgs e)
        {
            var auth = Token.Authenticate(e);
            JsonObject? reply;
            int status;

            if (auth.Success)
            {
                status = HttpStatusCode.OK;
                reply = new JsonObject
                {
                    ["success"] = true,
                    ["username"] = auth.User!.UserName,
                    ["fullname"] = auth.User.FullName,
                    ["email"] = auth.User.EMail
                };
            }
            else
            {
                status = HttpStatusCode.UNAUTHORIZED;
                reply = new JsonObject
                {
                    ["success"] = false,
                    ["message"] = "Unauthorized."
                };
            }

            e.Reply(status, reply.ToJsonString());
            return true;
        }

        /// <summary>Bearbeitet spezifische Benutzeranfragen basierend auf dem Pfad und der Methode.</summary>
        private bool HandleUserSpecificRequests(HttpSvrEventArgs e)
        {
            if (e.Method == "GET" && e.Path == "/users")
            {
                return HandleGetAllUsers(e);
            }
            else if (e.Method == "GET" && e.Path.StartsWith("/users/"))
            {
                return HandleGetUser(e);
            }
            else if (e.Method == "POST" && e.Path.EndsWith("/stack/add-package"))
            {
                return HandleAddPackage(e);
            }
            else if (e.Method == "POST" && e.Path.EndsWith("/deck/choose"))
            {
                return HandleChooseDeck(e);
            }

            return false;
        }

        /// <summary>Gibt alle Benutzer zurück.</summary>
        private bool HandleGetAllUsers(HttpSvrEventArgs e)
        {
            var auth = Token.Authenticate(e);
            JsonObject? reply;
            int status;

            if (auth.Success)
            {
                JsonArray usersArray = new();
                foreach (var user in User.GetAllUsers())
                {
                    usersArray.Add(new JsonObject
                    {
                        ["username"] = user.UserName,
                        ["fullname"] = user.FullName,
                        ["email"] = user.EMail
                    });
                }

                status = HttpStatusCode.OK;
                reply = new JsonObject
                {
                    ["success"] = true,
                    ["users"] = usersArray
                };
            }
            else
            {
                status = HttpStatusCode.UNAUTHORIZED;
                reply = new JsonObject
                {
                    ["success"] = false,
                    ["message"] = "Unauthorized."
                };
            }

            e.Reply(status, reply.ToJsonString());
            return true;
        }

        /// <summary>Gibt einen spezifischen Benutzer basierend auf dem Pfad zurück.</summary>
        private bool HandleGetUser(HttpSvrEventArgs e)
        {
            string requestedUser = e.Path.Substring("/users/".Length);

            if (User.Exists(requestedUser))
            {
                User user = User.Get(requestedUser)!;
                JsonObject reply = new()
                {
                    ["success"] = true,
                    ["username"] = user.UserName,
                    ["fullname"] = user.FullName,
                    ["email"] = user.EMail
                };

                e.Reply(HttpStatusCode.OK, reply.ToJsonString());
                return true;
            }

            JsonObject notFoundReply = new()
            {
                ["success"] = false,
                ["message"] = "User not found."
            };

            e.Reply(HttpStatusCode.NOT_FOUND, notFoundReply.ToJsonString());
            return true;
        }

        /// <summary>Fügt ein Paket zum Stapel eines Benutzers hinzu.</summary>
        private bool HandleAddPackage(HttpSvrEventArgs e)
        {
            return HandleAuthenticatedRequest(e, "Package added to user's stack.");
        }

        /// <summary>Wählt ein neues Deck für den Benutzer aus.</summary>
        private bool HandleChooseDeck(HttpSvrEventArgs e)
        {
            return HandleAuthenticatedRequest(e, "Deck selected from user's stack.");
        }

        /// <summary>Hilfsmethode zur Verarbeitung authentifizierter Anfragen.</summary>
        private bool HandleAuthenticatedRequest(HttpSvrEventArgs e, string successMessage)
        {
            var auth = Token.Authenticate(e);
            JsonObject reply;
            int status;

            if (auth.Success)
            {
                status = HttpStatusCode.OK;
                reply = new JsonObject
                {
                    ["success"] = true,
                    ["message"] = successMessage
                };
            }
            else
            {
                status = HttpStatusCode.UNAUTHORIZED;
                reply = new JsonObject
                {
                    ["success"] = false,
                    ["message"] = "Unauthorized."
                };
            }

            e.Reply(status, reply.ToJsonString());
            return true;
        }
    }
}