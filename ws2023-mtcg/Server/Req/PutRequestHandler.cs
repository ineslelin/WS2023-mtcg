using Newtonsoft.Json;
using Npgsql.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ws2023_mtcg.Models;
using ws2023_mtcg.Server.Repository;
using ws2023_mtcg.Server.Res;

namespace ws2023_mtcg.Server.Req
{
    internal class PutRequestHandler
    {
        string req;
        string data;
        string response;

        TcpClient client;
        StreamReader reader;
        StreamWriter writer;

        public PutRequestHandler(TcpClient client, StreamReader reader, StreamWriter writer, string req)
        {
            if (req == null)
                throw new ArgumentNullException();

            this.client = client;
            this.reader = reader;
            this.writer = writer;
            ServerCommands serverCommands = new ServerCommands(client, writer, reader);

            this.req = req;
            this.data = "";

            string[] reqLines = req.Split("\n");
            string[] route = reqLines[0].Split(" ");

            if (route[1] == "/deck")
            {
                data = serverCommands.RetrieveData();
                HandleDeckRequest();
            }

            if (Regex.IsMatch(route[1], @"/users/[a-zA-Z]*"))
            {
                data = serverCommands.RetrieveData();
                HandleUserRequest(route[1]);
            }
        }

        private void HandleDeckRequest() 
        {
            string username = "";

            try
            {
                TokenValidator.CheckTokenExistence(req);
                string authHeader = TokenValidator.GetAuthHeader(req);
                username = TokenValidator.SplitToken(authHeader);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Access token is missing or invalid",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Unauthorized);

                return;
            }

            List<Cards>? deck = new List<Cards>();
            List<string> cardIds = new List<string>();

            try
            {
                cardIds = JsonConvert.DeserializeObject<List<string>>(data);

                foreach(var id in cardIds)
                {
                    deck.Add(new Cards { Id = id,
                                         Owner = username });
                }

                if (deck == null)
                    return;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Invalid JSON"
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            if(deck.Count() < 4 || deck.Count() > 4)
            {
                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "The provided deck did not include the required amount of cards"
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            StackRepository stackRepository = new StackRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

            try
            {
                for (int i = 0; i < deck.Count; i++)
                {
                    deck[i] = stackRepository.ReadById(deck[i].Id);

                    if (deck[i].Owner != username)
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "At least one of the provided cards does not belong to the user or is not available."
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Forbidden);

                return;
            }

            DeckRepository deckRepository = new DeckRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

            try
            {
                deckRepository.Delete(username);

                foreach(var d in deck)
                {
                    deckRepository.Create(d);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Couldn't create deck."
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            List<Card> cards = new List<Card>();

            foreach (var card in deck)
            {
                Card temp = new Card()
                {
                    Id = card.Id,
                    Name = card.Name,
                    Damage = card.Damage,
                };

                cards.Add(temp);
            }

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                deck = cards
            });

            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
        }

        private void HandleUserRequest(string route)
        {
            string username = "";

            try
            {
                TokenValidator.CheckTokenExistence(req);
                string authHeader = TokenValidator.GetAuthHeader(req);
                username = TokenValidator.SplitToken(authHeader);

                if (!route.Contains(username))
                {
                    if (!TokenValidator.CheckAdminToken(authHeader))
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Access token is missing or invalid",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Unauthorized);

                return;
            }

            UserRepository userRepository = new UserRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true"); 
            User tempUser = new User();

            try
            {
                tempUser = userRepository.Read(username);

                if (tempUser == null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "User not found",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.NotFound);

                return;
            }

            try
            {
                tempUser = JsonConvert.DeserializeObject<User>(data);

                if (tempUser == null)
                    return;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Invalid JSON",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            UserProfileRepository userProfileRepository = new UserProfileRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

            try
            {
                tempUser.Username = username;

                if (userProfileRepository.Read(tempUser.Username) == null)
                    userProfileRepository.Create(tempUser);
                else
                    userProfileRepository.Update(tempUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Couldn't update user data.",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                profile = new UserData
                {
                    Name = tempUser.Name,
                    Bio = tempUser.Bio,
                    Image = tempUser.Image
                }
            });

            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
        }
    }
}
