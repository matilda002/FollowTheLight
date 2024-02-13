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

        public Server(DatabaseHelper db)
        {
            _listener = new HttpListener();
            _db = db;
            _listener.Prefixes.Add("http://localhost:3000/");
        }

        public void Start()
        {
            _listener.Start();
            Console.WriteLine("Server started. Listening for requests...");

            ThreadPool.QueueUserWorkItem((o) =>
            {
                try
                {
                    while (_listener.IsListening)
                    {
                        HttpListenerContext context = _listener.GetContext();
                        ThreadPool.QueueUserWorkItem((c) => HandleRequest(context));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception: {e.Message}");
                }
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

        public void HandleRequest(object state)
        {
            HttpListenerContext context = (HttpListenerContext)state;
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

        private void IntroGet(HttpListenerResponse response)
        {
            string resultIntro = _db.GetStoryPointContent(1); 
            SendResponse(response, resultIntro);
        }

        private void GameOneGet(HttpListenerResponse response)
        {
            string resultStoryOne = _db.GetStoryPointContent(2);
            SendResponse(response, resultStoryOne);
        }

        private void GameTwoGet(HttpListenerResponse response)
        {
            string resultStoryTwo = _db.GetStoryPointContent(3); 
        }

        private void GameThreeGet(HttpListenerResponse response)
        {
            string resultStoryThree = _db.GetStoryPointContent(4); 
            SendResponse(response, resultStoryThree);
        }

        private void GameOnePost(HttpListenerRequest req, HttpListenerResponse res)
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
                        answer += "You chose option B in Game One.\n"; // Add scenario-specific logic here
                        break;
                    default:
                        answer += "Invalid choice in Game One.\n"; // Add scenario-specific logic here
                        break;
                }

                // Send user choice as response
                SendResponse(res, answer);
            }


        private void GameTwoPost(HttpListenerRequest req, HttpListenerResponse res)
        {
            // Handle POST request for GameTwo
        }

        private void GameThreePost(HttpListenerRequest req, HttpListenerResponse res)
        {
            // Handle POST request for GameThree
        }

        private void SendResponse(HttpListenerResponse response, string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.ContentType = "text/plain";
            response.StatusCode = (int)HttpStatusCode.OK;

            foreach (byte b in buffer)
            {
                response.OutputStream.WriteByte(b);
                Thread.Sleep(35);
            }
            response.OutputStream.Close();
        }

        private void NotFound(HttpListenerResponse res)
        {
            res.StatusCode = (int)HttpStatusCode.NotFound;
            res.Close();
        }
    }
}
