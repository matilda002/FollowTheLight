using System.Net;
using System.Text;
using Npgsql;
namespace FollowTheLightMain

{
    public class Server
    {
        private readonly NpgsqlDataSource _db;
        private readonly DatabaseHelper dbHelper; // rename to _dbHelper

        public Server(NpgsqlDataSource db)
        {
            _db = db;
            dbHelper = new(db);
        }

        public void HandleRequest(IAsyncResult result)
        {
            if (result.AsyncState is HttpListener requestListener)
            {
                HttpListenerContext context = requestListener.EndGetContext(result);

                Router(context);

                requestListener.BeginGetContext(HandleRequest, requestListener);
            }
        }
        public void Router(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            PostPuzzle puzzle = new (_db);

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
                            PuzzleOneP1(response);
                            break;
                        case "/game/player/4":
                            GameThreeGet(response);
                            break;
                        case "/game/player/5":
                            GameFourGet(response);
                            break;
                        case "/game/player/6": 
                            PuzzleTwoP1(response);
                            break; 
                        case "/game/player/7":
                            GameFiveGet(response);
                            break;
                        case "/game/player/8":
                            GameSixGet(response);
                            break;
                        case "/game/player/9": 
                            PuzzleThreeP1(response);
                            break; 
                        case "/game/player/10":
                            EndGet(response);
                            break; 
                        case "/game/player2/1":
                            GameSevenGet(response);
                            break;
                        case "/game/player2/2":
                            GameEightGet(response);
                            break;
                        case "/game/player2/3":
                            PuzzleOneP2(response);
                            break; 
                        case "/game/player2/4":
                            GameNineGet(response);
                            break;
                        case "/game/player2/5":
                            GameTenGet(response);
                            break;
                        case "/game/player2/6":
                            PuzzleTwoP2(response);
                            break; 
                        case "/game/player2/7":
                            GameElevenGet(response);
                            break;
                        case "/game/player2/8":
                            GameTwelveGet(response);
                            break;
                        case "/game/player2/9":
                            PuzzleThreeP2(response);
                            break; 
                        case "/game/player2/10":
                            EndGet(response);
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
                            puzzle.PuzzleOneP1Post(request, response);
                            break;
                        case "/game/player/4":
                            GameThreePost(request, response);
                            break;
                        case "/game/player/5":
                            GameFourPost(request, response);
                            break;
                        case "/game/player/6":
                            puzzle.PuzzleTwoP1Post(request, response);
                            break;
                        case "/game/player/7":
                            GameFivePost(request, response);
                            break;
                        case "/game/player/8":
                            GameSixPost(request, response);
                            break;
                        case "/game/player/9":
                            puzzle.PuzzleThreeP1Post(request, response);
                            break;
                        case "/game/player2/1":
                            GameSevenPost(request, response);
                            break;
                        case "/game/player2/2":
                            GameEightPost(request, response);
                            break;
                        case "/game/player2/3":
                            puzzle.PuzzleOneP2Post(request, response);
                            break;
                        case "/game/player2/4":
                            GameNinePost(request, response);
                            break;
                        case "/game/player2/5":
                            GameTenPost(request, response);
                            break;
                        case "/game/player2/6":
                            puzzle.PuzzleTwoP2Post(request, response);
                            break;
                        case "/game/player2/7":
                            GameElevenPost(request, response);
                            break;
                        case "/game/player2/8":
                            GameTwelvePost(request, response);
                            break;
                        case "/game/player2/9":
                            puzzle.PuzzleThreeP2Post(request, response);
                            break;
                        case ("/player/register"):
                            Player registerPlayer = new Player();
                            registerPlayer.Register(request, response);
                            break;
                        case ("/player/status"):
                            Player playerStatus = new Player();
                            playerStatus.ViewStatus(request, response);
                            break;
                        case ("/player/1/chat"):
                            Radio chatOne = new Radio(_db);
                            chatOne.SendMessageOne(request, response);
                            break;
                        case ("/player/2/chat"):
                            Radio chatTwo = new Radio(_db);
                            chatTwo.SendMessageTwo(request, response);
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
            string res = dbHelper.GetStoryPointContent(1);
            SendResponse(response, res);
        }
        void EndGet(HttpListenerResponse response)
        {
            string res = dbHelper.GetStoryPointContent(20);
            SendResponse(response, res);
        }

        void GameOneGet(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(2);
            SendResponse(response, result);
        }
        void GameTwoGet(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(3);
            SendResponse(response, result);
        }
        void GameThreeGet(HttpListenerResponse response)
        {   
            string result = dbHelper.GetStoryPointContent(4);
            string resultImg = dbHelper.GetImgContent(8);
            SendResponse(response, result + resultImg);
        }
        void GameFourGet(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(5);
            SendResponse(response, result);
        }
        void GameFiveGet(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(6);
            SendResponse(response, result);
        }
        void GameSixGet(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(7);
            SendResponse(response, result);
        }
        void GameSevenGet(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(8);
            SendResponse(response, result);
        }
        void GameEightGet(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(9);
            SendResponse(response, result);
        }
        void GameNineGet(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(10);
            SendResponse(response, result);
        }
        void GameTenGet(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(11);
            SendResponse(response, result);
        }
        void GameElevenGet(HttpListenerResponse response)
        {
            string resultStoryThree = dbHelper.GetStoryPointContent(12);
            SendResponse(response, resultStoryThree);
        }
        void GameTwelveGet(HttpListenerResponse response)
        {
            string resultStoryThree = dbHelper.GetStoryPointContent(13);
            SendResponse(response, resultStoryThree);
        }

        // Puzzles Get
        void PuzzleOneP1(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(14);
            string resultImg = dbHelper.GetImgContent(10);
            SendResponse(response, result + resultImg);
        }
        void PuzzleTwoP1(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(16);
            string resultImg = dbHelper.GetImgContent(11);
            SendResponse(response, result + resultImg);
        } 
        void PuzzleThreeP1(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(18);
            string resultImg = dbHelper.GetImgContent(13);
            SendResponse(response, result + resultImg);
        } 
        void PuzzleOneP2(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(15);
            string resultImg = dbHelper.GetImgContent(9);
            SendResponse(response, result + resultImg);
        }
        void PuzzleTwoP2(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(17);
            string resultImg = dbHelper.GetImgContent(12);
            SendResponse(response, result + resultImg);
        } 
        void PuzzleThreeP2(HttpListenerResponse response)
        {
            string result = dbHelper.GetStoryPointContent(19);
            string resultImg = dbHelper.GetImgContent(14);
            SendResponse(response, result + resultImg);
        } 
        
        // Player One Storyline
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
                    chat.GameMessage(res);
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
                    chat.GameMessage(res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
            SendResponse(res, answer);
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
                    chat.GameMessage(res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
            SendResponse(res, answer);
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
                    chat.GameMessage(res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
            SendResponse(res, answer);
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
                    chat.GameMessage(res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
            SendResponse(res, answer);
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
                    chat.GameMessage(res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
            SendResponse(res, answer);
        }

        // Player Two Storyline
        void GameSevenPost(HttpListenerRequest req, HttpListenerResponse res) // Scenario One
        {
            StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            string answer = string.Empty;

            switch (body)
            {
                case "A":
                case "a":
                    answer += "Option A chosen in the first scenario. Taking a closer look, you see the corpse has been mutilated and eaten by something. There is a note on the corpse’s chest pocket. The note has written “Beware of the red frog. It is poisonous but poking it will make it go away”.. Maybe the other player can use it to avoid the dangers that might unfold. \n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in the first scenario.\n";
                    break;
                case "C":
                case "c":
                    answer += "Option C chosen in the first scenario. You disturb a swarm of flesh-eating insects that attack you. You lose a health point.\r\n \n";
                    break;
                case "D":
                case "d":
                    answer += "Option D chosen in the first scenario. You trip over a wire and trigger a trap that shoots arrows made of human bones at you. You lose a health point. \n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.GameMessage(res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
            SendResponse(res, answer);
        }
        void GameEightPost(HttpListenerRequest req, HttpListenerResponse res) // Scenario Two
        {
            StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            string answer = string.Empty;

            switch (body)
            {
                case "A":
                case "a":
                    answer += "Option A chosen in the second scenario. The spider seems pleased with the scratches and purrs softly. It pulls a string on the web which opens a secret passage to continue. passing the spider it screeches and whispers “If it can not hear you, it can not see you” before disappearing into the depth. \n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in the second scenario. The spider gets angry and spits venom at you. You lose a health point.\n";
                    break;
                case "C":
                case "c":
                    answer += "Option C chosen in the second scenario. The spider ignores you and wraps you in its web. You lose a health point.\n";
                    break;
                case "D":
                case "d":
                    answer += "Option D chosen in the second scenario. \n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.GameMessage(res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
            SendResponse(res, answer);
        }
        void GameNinePost(HttpListenerRequest req, HttpListenerResponse res) // Scenario Three
        {
            StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            string answer = string.Empty;

            switch (body)
            {
                case "A":
                case "a":
                    answer += "Option A chosen in the third scenario. \n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in the third scenario. You hide behind a pile of bones and see a shadowy figure pass by. It looks like a tall repulsing creature with horns and a wretched tail. You notice a ladder engraved on his back.\n";
                    break;
                case "C":
                case "c":
                    answer += "Option C chosen in the third scenario. You provoke the creature and it attacks you. You lose a health point. \n";
                    break;
                case "D":
                case "d":
                    answer += "Option D chosen in the third scenario. You get too close to the creature and it bites you. You lose a health point.\r\n\n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.GameMessage(res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
            SendResponse(res, answer);
        }
        void GameTenPost(HttpListenerRequest req, HttpListenerResponse res)
        {
            StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            string answer = string.Empty;

            switch (body)
            {
                case "A":
                case "a":
                    answer += "Option A chosen in the fourth scenario.The raft is sturdy despite looking half done and takes you safely to the other side where you spot a withered sign saying: “Stop running and Follow The Light”... \n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in the fourth scenario. Embarking the boat you notice little too late that it has holes. The boat starts to leak and sink rapidly. You try to swim back to the shore, but you feel something grab your leg and pull you down. You scream, but no one can hear you. You drowned. [ YOU DIED ] \n";
                    break;
                case "C":
                case "c":
                    answer += "Option C chosen in the fourth scenario. Swept away by the current, you hit your head on a rock and almost lose consciousness. You lose a health point. You wake up back on the shore.\n";
                    break;
                case "D":
                case "d":
                    answer += "Option D chosen in the fourth scenario. The waterfall is not a gentle shower, but a raging torrent. It slams you against the rocks and pushes you back to the shore. You are bruised, battered, and soaked. You lose a health point and your dignity. That was stupid.\n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.GameMessage(res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
            SendResponse(res, answer);
        }// Scenario Four
        void GameElevenPost(HttpListenerRequest req, HttpListenerResponse res)// Scenario Five
        {
            StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            string answer = string.Empty;

            switch (body)
            {
                case "A":
                case "a":
                    answer += "Option A chosen in the fifth scenario. By quietly standing still, the danger does not sense your presence. When passing you, it is sinisterly almost inaudible repeating “Honor the Gods, Honor the Gods…” \n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in the fifth scenario. You trip over a skull and fall to the ground. The figure catches up to you and slashes you with its claws. You lose a health point.\n";
                    break;
                case "C":
                case "c":
                    answer += "Option C chosen in the fifth scenario. You grab a bone and swing it at the figure. It breaks the bone and grabs you by the neck and mangle you. [ YOU DIED ] \n";
                    break;
                case "D":
                case "d":
                    answer += "Option D chosen in the fifth scenario. \n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.GameMessage(res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
            SendResponse(res, answer);
        }
        void GameTwelvePost(HttpListenerRequest req, HttpListenerResponse res)// Scenario Six
        {
            StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            string answer = string.Empty;

            switch (body)
            {
                case "A":
                case "a":
                    answer += "Option A chosen in the sixth scenario. You light the candle, feeling a surge of gratitude from the gods. The wall behind you begins to rumble and quake. You turn around and see a large crack forming on the surface. A faint light seeps through the gap. You have discovered a hidden chamber that was guarded by the gods.\n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in the sixth scenario. Ah yes, you have angered the Gods. Wretched screams close in and something bites you and leaves a blood wound. You lose a health point.\n";
                    break;
                case "C":
                case "c":
                    answer += "Option C chosen in the sixth scenario. Ah yes, you have angered the Gods. You feel a excruciating pain around your hand and drop the candle. You lose a health point.\n";
                    break;
                case "D":
                case "d":
                    answer += "Option D chosen in the sixth scenario. \n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.GameMessage(res);
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }
            SendResponse(res, answer);
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
            foreach (byte b in buffer)
            {
                response.OutputStream.WriteByte(b);
                Thread.Sleep(35);
            }
            response.StatusCode = (int)HttpStatusCode.Created;
            response.Close();
        }
        void NotFound(HttpListenerResponse res)
        {
            res.StatusCode = (int)HttpStatusCode.NotFound;
            res.Close();
        }
    }
}