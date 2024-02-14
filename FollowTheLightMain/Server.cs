using Npgsql;
using System.Net;
using System.Text;
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
                        Player registerPlayer = new Player(_db);
                        registerPlayer.Register(request, response);
                        break;
                    case ("/player/status"):
                        Player playerStatus = new Player(_db);
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
        }

        byte[] buffer = Encoding.UTF8.GetBytes(resultIntro);
        response.ContentType = "text/plain";
        response.StatusCode = (int)HttpStatusCode.OK;

        foreach (byte b in buffer)
        {
            response.OutputStream.WriteByte(b);
            Thread.Sleep(35);
        }
        response.OutputStream.Close();
    }

    void GameOneGet(HttpListenerResponse response, HttpListenerRequest request)
    {
        StreamReader reader1 = new(request.InputStream, request.ContentEncoding);
        string username = reader1.ReadToEnd();
        
        string queryToDb = "update players set current_storypoint = @current_storypoint where username = @username";
        var cmd = _db.CreateCommand(queryToDb);
        cmd.Parameters.AddWithValue("@current_storypoint", 2);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.ExecuteNonQuery(); 
        
        string resultStoryOne = string.Empty;
        Console.WriteLine("Printing out 'Story One' from storypoints to player...");

        const string qStoryOne = "select content from storypoints where storypoint_id = 2";
        var reader = _db.CreateCommand(qStoryOne).ExecuteReader();
        while (reader.Read())
        {
            resultStoryOne = reader.GetString(0);
        }

        byte[] buffer = Encoding.UTF8.GetBytes(resultStoryOne);
        response.ContentType = "text/plain";
        response.StatusCode = (int)HttpStatusCode.OK;

        foreach (byte b in buffer)
        {
            response.OutputStream.WriteByte(b);
            Thread.Sleep(35);
        }
        response.OutputStream.Close();
    }
    void GameTwoGet(HttpListenerResponse response)
    {
        string resultStoryTwo = string.Empty;
        Console.WriteLine("Printing out 'Story Two' from storypoints to player...");

        const string qStoryTwo = "select content from storypoints where storypoint_id = 3";
        var reader = _db.CreateCommand(qStoryTwo).ExecuteReader();
        while (reader.Read())
        {
            resultStoryTwo = reader.GetString(0);
        }

        byte[] buffer = Encoding.UTF8.GetBytes(resultStoryTwo);
        response.ContentType = "text/plain";
        response.StatusCode = (int)HttpStatusCode.OK;

        foreach (byte b in buffer)
        {
            response.OutputStream.WriteByte(b);
            Thread.Sleep(35);
        }
        response.OutputStream.Close();
    }


    void GameOnePost(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd();
        string answer = string.Empty;

        switch (body)
        {
            case ("A"):
            case ("a"):
                answer +=
                    "You find teeth looks like it's human. You turn back around, take the left tunnel and find a torch.\n";
                break;
            case ("B"):
            case ("b"):
                answer +=
                    "You find a torch, might be useful later\n";
                break;
            default:
                answer +=
                    "That option does not exist\n";
                break;
        }

        //user choice
        byte[] buffer = Encoding.UTF8.GetBytes(answer);
        res.ContentType = "text/plain";
        res.StatusCode = (int)HttpStatusCode.OK;
        foreach (byte b in buffer)
        {
            res.OutputStream.WriteByte(b);
            Thread.Sleep(35);
        }

        res.StatusCode = (int)HttpStatusCode.Created;
        res.Close();
    }

    void GameTwoPost(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd();
        string answer = string.Empty;

        switch (body)
        {
            case ("A"):
            case ("a"):
                answer +=
                    "You find teeth looks like it's human. You turn back around, take the left tunnel and find a torch.\n";
                break;
            case ("B"):
            case ("b"):
                answer +=
                    "You find a torch, might be useful later\n";
                break;
            default:
                answer +=
                    "That option does not exist\n";
                break;
        }

        //user choice
        byte[] buffer = Encoding.UTF8.GetBytes(answer);
        res.ContentType = "text/plain";
        res.StatusCode = (int)HttpStatusCode.OK;
        foreach (byte b in buffer)
        {
            res.OutputStream.WriteByte(b);
            Thread.Sleep(35);
        }

        res.StatusCode = (int)HttpStatusCode.Created;
        res.Close();
    }

    public void StoreChat(HttpListenerRequest req, HttpListenerResponse res)
    {
        try
        {

            StreamReader reader = new(req.InputStream, req.ContentEncoding);
            string chat = reader.ReadToEnd();

            string answer = "Your message has been sent";

            byte[] buffer = Encoding.UTF8.GetBytes(answer);
            res.ContentType = "text/plain";
            res.StatusCode = (int)HttpStatusCode.OK;
            res.OutputStream.Write(buffer, 0, buffer.Length);
            res.Close();


            using (var cmd = _db.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS radio(value TEXT)";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _db.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO radio (from_player, to_player, message) VALUES (@fromPlayer, @toPlayer, @message)";
                cmd.Parameters.AddWithValue("@fromPlayer", 1);
                cmd.Parameters.AddWithValue("toPlayer", 2);
                cmd.Parameters.AddWithValue("@message", chat);
                cmd.ExecuteNonQuery();
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error " + ex.Message);
            string answer = "Error";

            byte[] buffer = Encoding.UTF8.GetBytes(answer);
            res.ContentType = "text/plain";
            res.StatusCode = (int)HttpStatusCode.InternalServerError;
            res.OutputStream.Write(buffer, 0, buffer.Length);
            res.Close();
        }

    }

    void NotFound(HttpListenerResponse res)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;
        res.Close();
    }
}