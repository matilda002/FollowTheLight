using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Npgsql;

namespace FollowTheLightMain
{
    public class Server
    {
        private readonly HttpListener _listener;
        private readonly DatabaseHelper _db;

        public Server(NpgsqlDataSource db)
        {
            _listener = new HttpListener();
            _db = new DatabaseHelper(db);
            _listener.Prefixes.Add("http://localhost:3000/");

            _listener.Start();
            Console.WriteLine("Server started. Listening for requests...");
            _listener.BeginGetContext(HandleRequest, _listener);
        }

 
        public void HandleRequest(IAsyncResult result)
        {
            try
            {
                HttpListenerContext context = _listener.EndGetContext(result);       
                Router(context);
                _listener.BeginGetContext(HandleRequest, _listener);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
            }
        }
        void Router(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            switch (request.HttpMethod)
            {
                case "GET":
                    switch (request.Url?.AbsolutePath)
                    {
                        case "/intro":
                            IntroGet(response);
                            break;
                        case "/game/player/1":
                            GameOneGet(response);
                            break;
                        case "/game/player/2":
                            GameTwoGet(response);
                            break;
                        case "/game/player/3":
                            GameThreeGet(response);
                            break;
                        case "/game/player/4":
                            GameThreeGet(response);
                            break;
                        case "/game/player/5":
                            GameThreeGet(response);
                            break;
                        case "/game/player/6":
                            GameThreeGet(response);
                            break;
                        case "/game/player/message":
                            Radio message = new Radio(_db);
                            message.GameMessage(response);
                            break;
                        default:
                            NotFound(response);
                            break;
                    }
                    break;
                case "POST":
                    switch (request.Url?.AbsolutePath)
                    {
                        case "/game/player/1":
                            GameOnePost(request, response);
                            break;
                        case "/game/player/2":
                            GameTwoPost(request, response);
                            break;
                        case "/game/player/3":
                            GameThreePost(request, response);
                            break;
                        case "/game/player/4":
                            GameThreePost(request, response);
                            break;
                        case "/game/player/5":
                            GameThreePost(request, response);
                            break;
                        case "/game/player/6":
                            GameThreePost(request, response);
                            break;
                        case "/player/register":
                            Player registerPlayer = new Player();
                            registerPlayer.RegisterPost(request, response);
                            break;
                        case "/player/login":
                            Player playerLogin = new Player();
                            playerLogin.LoginPost(request, response);
                            break;
                        default:
                            NotFound(response);
                            break;
                    }
                    break;
                default:
                    NotFound(response);
                    break;
            }
        }

        void IntroGet(HttpListenerResponse response)
        {
            string resultIntro = _db.GetStoryPointContent(0);
            SendResponse(response, resultIntro);
        }

        void GameOneGet(HttpListenerResponse response)
        {
            string resultStoryOne = _db.GetStoryPointContent(1);
            SendResponse(response, resultStoryOne);
        }

        void GameTwoGet(HttpListenerResponse response)
        {
            string resultStoryTwo = _db.GetStoryPointContent(2);
            SendResponse(response, resultStoryTwo);
        }

        void GameThreeGet(HttpListenerResponse response)
        {
            string resultStoryThree = _db.GetStoryPointContent(3);
            SendResponse(response, resultStoryThree);
        }
        void GameFourGet(HttpListenerResponse response)
        {
            string resultStoryThree = _db.GetStoryPointContent(4);
            SendResponse(response, resultStoryThree);
        }
        void GameFiveGet(HttpListenerResponse response)
        {
            string resultStoryThree = _db.GetStoryPointContent(5);
            SendResponse(response, resultStoryThree);
        }
        void GameSixGet(HttpListenerResponse response)
        {
            string resultStoryThree = _db.GetStoryPointContent(6);
            SendResponse(response, resultStoryThree);
        }

        void GameOnePost(HttpListenerRequest req, HttpListenerResponse res)
        {
            StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            string answer = string.Empty;

            switch (body)
            {
                case "A":
                case "a":
                    answer += "Option A chosen in the first scenario. You found a torch and noticed that the cave wall has “Fear not the dead” written creepily in blood.\n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in the first scenario.\n";
                    break;
                case "C":
                case "c":
                    answer += "Option C chosen in the first scenario. You feel a cold hand grab your ankle and drag you into the dark. You lose a health point.\n";
                    break;
                case "D":
                case "d":
                    answer += "Option D chosen in the first scenario. You waste a match and attract unwanted attention. You lose a health point.\n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.StoreChat(req, res);
                    break;
                default:
                    answer += "Invalid choice in the first scenario.\n";
                    break;
            }

            SendResponse(res, answer);
        }
        void GameTwoPost(HttpListenerRequest req, HttpListenerResponse res)
        {
            StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            string answer = string.Empty;

            switch (body)
            {
                case "A":
                case "a":
                    answer += "Option A chosen in the second scenario. The paper has a drawing of a spider and a note. Maybe the other player can use it, the note has written “Webby loves leg scratches”.\r\n\n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in the second scenario.\n";
                    break;
                case "C":
                case "c":
                    answer += "Option C chosen in the second scenario. You destroy a valuable piece of information and the flame burns fast and you get burnt. You lose a health point.\n";
                    break;
                case "D":
                case "d":
                    answer += "Option D chosen in the second scenario. You choke on the paper, stupid. You lose a health point.\n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.StoreChat(req, res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
        }
      
        void GameThreePost(HttpListenerRequest req, HttpListenerResponse res)
        {
            StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            string answer = string.Empty;

            switch (body)
            {
                case "A":
                case "a":
                    answer += "Option A chosen in the third scenario. The frog is poisonous and you feel a sharp pain in your hand. You lose a health point.\n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in the third scenario. The frog is disgusted and bites your lip, and you get poisoned by the bite. You lose a health point.\n";
                    break;
                case "C":
                case "c":
                    answer += "Option C chosen in the third scenario.\n";
                    break;
                case "D":
                case "d":
                    answer += "Option D chosen in the third scenario. The frog jumps away and reveals a note that seemed to be under it. You read “The float is reliable”...\n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.StoreChat(req, res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
        }

        void GameFourPost(HttpListenerRequest req, HttpListenerResponse res)
        {
            StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            string answer = string.Empty;

            switch (body)
            {
                case "A":
                case "a":
                    answer += "Option A chosen in the forth scenario. You manage to balance and reach the top. As you climb, you notice that the symbols are a combination of numbers - xxxx and a sentence engraved: “Honor the Gods…”. Maybe they will come in handy later.\n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in the forth scenario. The rope is infested with maggots and worms. They bite and burrow into your skin. You lose a health point.\n";
                    break;
                case "C":
                case "c":
                    answer += "Option C chosen in the forth scenario. The wall is coated with a slimy substance leaking out of the cracks that burns your flesh. You feel a searing pain in your hands and feet. You lose a health point.\n";
                    break;
                case "D":
                case "d":
                    answer += "Option D chosen in the forth scenario. You look around for another way. Maybe you’ll discover a hidden passage or a secret door. \n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.StoreChat(req, res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
        }

        void GameFivePost(HttpListenerRequest req, HttpListenerResponse res)
        {
            StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            string answer = string.Empty;

            switch (body)
            {
                case "A":
                case "a":
                    answer += "Option A chosen in the fifth scenario. Removing some stones reveals a hidden passage. You see a faint light at the end of it.\n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in the fifth scenario. \n";
                    break;
                case "C":
                case "c":
                    answer += "Option C chosen in the fifth scenario. You cause another collapse and get crushed by the stones. You lose a health point.\n";
                    break;
                case "D":
                case "d":
                    answer += "Option D chosen in the fifth scenario.  You hear a hissing sound and realize the stones are now covered with snakes. You lose a health point.\n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.StoreChat(req, res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
        }

        void GameSixPost(HttpListenerRequest req, HttpListenerResponse res)
        {
            StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            string answer = string.Empty;

            switch (body)
            {
                case "A":
                case "a":
                    answer += "Option A chosen in the sisxth scenario. By quietly standing still, the danger does not sense your presence. When passing you, it is sinisterly almost inaudible repeating “Honor the Gods, Honor the Gods…” \n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in the sixth scenario. You trip over a skull and fall to the ground. The figure catches up to you and slashes you with its claws. You lose a health point.\n";
                    break;
                case "C":
                case "c":
                    answer += "Option C chosen in the sixth scenario. You grab a bone and swing it at the figure. It breaks the bone and grabs you by the neck and mangle you. [ YOU DIED ] \n";
                    break;
                case "D":
                case "d":
                    answer += "Option D chosen in the sixth scenario. You go back to previous scenario. \n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.StoreChat(req, res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
        }


        void SendResponse(HttpListenerResponse response, string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.ContentType = "text/plain";
            response.StatusCode = (int)HttpStatusCode.OK;

            using (Stream output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
            response.Close();
        }

       
        void NotFound(HttpListenerResponse res)
        {
            res.StatusCode = (int)HttpStatusCode.NotFound;
            res.Close();
        }

 
        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}
