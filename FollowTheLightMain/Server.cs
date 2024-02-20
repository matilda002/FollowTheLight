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
        GameGet get = new(_db);
        GamePost post = new();

        switch (request.HttpMethod)
        {
            case "GET":
                switch (request.Url?.AbsolutePath)
                {
                    case "/player":
                        get.Game(response);
                        break;
                    case "/game/player/message":
                        radio.GameMessage(response);
                        break;
                    default:
                        NotFound(response);
                        break;
                }
                break;

            case "POST":
                switch (request.Url?.AbsolutePath)
                {
                    case "/player":
                        post.Game(request, response);
                        break;
                    case ("/player/register"):
                        player.Register(request, response);
                        break;
                    case ("/player/status"):
                        player.ViewStatus(request, response);
                        break;
                    case ("/player/1/chat"):
                        radio.SendMessageOne(request, response);
                        break;
                    case ("/player/2/chat"):
                        radio.SendMessageTwo(request, response);
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