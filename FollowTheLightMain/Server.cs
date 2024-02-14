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

<<<<<<< HEAD
         
            _listener.Start();
            Console.WriteLine("Server started. Listening for requests...");
            _listener.BeginGetContext(HandleRequest, _listener);
=======
            Router(context);

            requestListener.BeginGetContext(HandleRequest, requestListener);
        }
    }

    void Router(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response; 

        switch (request.HttpMethod)
        {
            case ("GET"):
                switch (request.Url?.AbsolutePath)
                {
                    case ("/intro"):
                        IntroGet(response);
                        break;
                    case ("/game/player/1"):
                        GameOneGet(response, request);
                        break;
                    case ("/game/player/2"):
                        GameTwoGet(response);
                        break;
                    case ("/game/player/message"):
                        Radio message = new Radio(_db);
                        message.GameMessage(response);
                        break;
                    default:
                        NotFound(response);
                        break;
                }
                break;

            case ("POST"):
                switch (request.Url?.AbsolutePath)
                {
                    case ("/game/player/1"):
                        GameOnePost(request, response);
                        break;
                    case ("/game/player/2"):
                        GameTwoPost(request, response);
                        break;
                    case ("/player/register"):
                        Player registerPlayer = new Player();
                        registerPlayer.Register(request, response);
                        break;
                    case ("/player/status"):
                        Player playerStatus = new Player();
                        playerStatus.ViewStatus(request, response);
                        break;
                    case ("/game/player/chat"):
                        Radio chat = new Radio(_db);
                        chat.StoreChat(request, response);
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
        
        
        string resultIntro = string.Empty;
        Console.WriteLine("Printing out 'Intro' from storypoints to player...");

        const string qIntroGet = "select content from storypoints where storypoint_id = 1";
        var reader = _db.CreateCommand(qIntroGet).ExecuteReader();
        while (reader.Read())
        {
            resultIntro = reader.GetString(0);
>>>>>>> main
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
            string resultIntro = _db.GetStoryPointContent(1);
            SendResponse(response, resultIntro);
        }

        void GameOneGet(HttpListenerResponse response)
        {
            string resultStoryOne = _db.GetStoryPointContent(2);
            SendResponse(response, resultStoryOne);
        }

        void GameTwoGet(HttpListenerResponse response)
        {
            string resultStoryTwo = _db.GetStoryPointContent(3);
            SendResponse(response, resultStoryTwo);
        }

        void GameThreeGet(HttpListenerResponse response)
        {
            string resultStoryThree = _db.GetStoryPointContent(4);
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
                    answer += "You chose option A in Game One.\n";
                    break;
                case "B":
                case "b":
                    answer += "You chose option B in Game One.\n";
                    break;
                case "/game/player/chat":
                    Radio chat = new Radio(_db);
                    chat.StoreChat(req, res); // Corrected parameter names
                    break;
                default:
                    answer += "Invalid choice in Game One.\n";
                    break;
            }

            SendResponse(res, answer);
        }
        void GameTwoPost(HttpListenerRequest req, HttpListenerResponse res)
        {
           
        }
      
        void GameThreePost(HttpListenerRequest req, HttpListenerResponse res)
        {
            
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
