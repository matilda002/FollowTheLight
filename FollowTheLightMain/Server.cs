using System.Net;
using Npgsql;
namespace FollowTheLightMain;

public class Server
{
    private readonly NpgsqlDataSource _db;
    public Server(NpgsqlDataSource db)
    {
        _db = db;
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
        
        PostPuzzle postPuzzle = new(_db);
        Player player = new();
        Radio radio = new(_db);
        GetGame get = new(_db);
        PostGame post = new(_db);

        switch (request.HttpMethod)
        {
            case "GET":
                switch (request.Url?.AbsolutePath)
                {
                    case "/chat":
                        radio.GameMessage(response);
                        break;

                    case "/intro":
                        get.Intro(response);
                        break;
                    case "/game/player/1":
                        get.GameOne(response);
                        break;
                    case "/game/player/2":
                        get.GameTwo(response);
                        break;
                    case "/game/player/3":
                        get.PuzzleOneP1(response);
                        break;
                    case "/game/player/4":
                        get.GameThree(response);
                        break;
                    case "/game/player/5":
                        get.GameFour(response);
                        break;
                    case "/game/player/6":
                        get.PuzzleTwoP1(response);
                        break;
                    case "/game/player/7":
                        get.GameFive(response);
                        break;
                    case "/game/player/8":
                        get.GameSix(response);
                        break;
                    case "/game/player/9":
                        get.PuzzleThreeP1(response);
                        break;
                    case "/game/player/10":
                        get.End(response);
                        break;
                    case "/game/player2/1":
                        get.GameSeven(response);
                        break;
                    case "/game/player2/2":
                        get.GameEight(response);
                        break;
                    case "/game/player2/3":
                        get.PuzzleOneP2(response);
                        break;
                    case "/game/player2/4":
                        get.GameNine(response);
                        break;
                    case "/game/player2/5":
                        get.GameTen(response);
                        break;
                    case "/game/player2/6":
                        get.PuzzleTwoP2(response);
                        break;
                    case "/game/player2/7":
                        get.GameEleven(response);
                        break;
                    case "/game/player2/8":
                        get.GameTwelve(response);
                        break;
                    case "/game/player2/9":
                        get.PuzzleThreeP2(response);
                        break;
                    case "/game/player2/10":
                        get.End(response);
                        break;
                    default:
                        NotFound(response);
                        break;
                }
                break;

            case "POST":
                switch (request.Url?.AbsolutePath)
                {
                    case "/register":
                        player.Register(request, response);
                        break;
                    case "/player/1/chat":
                        radio.SendMessageOne(request, response);
                        break;
                    case "/player/2/chat":
                        radio.SendMessageTwo(request, response);
                        break;

                    case "/game/player/1":
                        post.GameOne(request, response);
                        break;
                    case "/game/player/2":
                        post.GameTwo(request, response);
                        break;
                    case "/game/player/3":
                        postPuzzle.PuzzleOneP1(request, response);
                        break;
                    case "/game/player/4":
                        post.GameThree(request, response);
                        break;
                    case "/game/player/5":
                        post.GameFour(request, response);
                        break;
                    case "/game/player/6":
                        postPuzzle.PuzzleTwoP1(request, response);
                        break;
                    case "/game/player/7":
                        post.GameFive(request, response);
                        break;
                    case "/game/player/8":
                        post.GameSix(request, response);
                        break;
                    case "/game/player/9":
                        postPuzzle.PuzzleThreeP1(request, response);
                        break;
                    case "/game/player/10":
                        post.Ending(request, response);
                        break;
                    case "/game/player2/1":
                        post.GameSeven(request, response);
                        break;
                    case "/game/player2/2":
                        post.GameEight(request, response);
                        break;
                    case "/game/player2/3":
                        postPuzzle.PuzzleOneP2(request, response);
                        break;
                    case "/game/player2/4":
                        post.GameNine(request, response);
                        break;
                    case "/game/player2/5":
                        post.GameTen(request, response);
                        break;
                    case "/game/player2/6":
                        postPuzzle.PuzzleTwoP2(request, response);
                        break;
                    case "/game/player2/7":
                        post.GameEleven(request, response);
                        break;
                    case "/game/player2/8":
                        post.GameTwelve(request, response);
                        break;
                    case "/game/player2/9":
                        postPuzzle.PuzzleThreeP2(request, response);
                        break;
                    case "/game/player2/10":
                        post.Ending(request, response);
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
    void NotFound(HttpListenerResponse res)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;
        res.Close();
    }
}