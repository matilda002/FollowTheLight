using System.Net;
using System.Text;
using Npgsql;
namespace FollowTheLightMain;

public class PostPuzzle
{
    private readonly NpgsqlDataSource _db;
    public PostPuzzle(NpgsqlDataSource db)
    {
        _db = db;
    }
    
    // Player One
    public void PuzzleOneP1Post(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd();
        string answer = string.Empty;

        switch (body)
        {
            case "666":
                answer += """
                          You paint the devil's number on the stone, it moves and reveals a tunnel.
                          There is a sign in the tunnel, and hard to read. You think it says:
                          "DON'T TRUST THEM"
                          """;
                break;
            case "/game/player/chat":
                Radio chat = new Radio(_db);
                chat.GameMessage(res);
                break;
            default:
                answer += "A huge cockroach helped you move the rock, and gave you dirty look.\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void PuzzleTwoP1Post(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd();
        string answer = string.Empty;

        switch (body)
        {
            case "NOP":
                answer += """
                          Clue
                          """;
                break;
            case "/game/player/chat":
                Radio chat = new Radio(_db);
                chat.GameMessage(res);
                break;
            default:
                answer += "The letters stopped glowing, and won't lit up again\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void PuzzleThreeP1Post(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd();
        string answer = string.Empty;

        switch (body)
        {
            case "354":
                answer += """
                          You cracked the code! When retrieving the key you notice a note inside:
                          "Maybe there is no-one left..."
                          """;
                break;
            case "/game/player/chat":
                Radio chat = new Radio(_db);
                chat.GameMessage(res);
                break;
            default:
                answer += "The code lock exploded and looks like it burned up a rolled up piece of paper\n";
                break;
        }
        SendResponse(res, answer);
    }
    
    // Player Two
    public void PuzzleOneP2Post(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd();
        string answer = string.Empty;

        switch (body)
        {
            case "NOP":
                answer += """
                          Clue
                          """;
                break;
            case "/game/player/chat":
                Radio chat = new Radio(_db);
                chat.GameMessage(res);
                break;
            default:
                answer += "\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void PuzzleTwoP2Post(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd();
        string answer = string.Empty;

        switch (body)
        {
            case "NOP":
                answer += """
                          Clue
                          """;
                break;
            case "/game/player/chat":
                Radio chat = new Radio(_db);
                chat.GameMessage(res);
                break;
            default:
                answer += "\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void PuzzleThreeP2Post(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd();
        string answer = string.Empty;

        switch (body)
        {
            case "NOP":
                answer += """
                          Clue
                          """;
                break;
            case "/game/player/chat":
                Radio chat = new Radio(_db);
                chat.GameMessage(res);
                break;
            default:
                answer += "\n";
                break;
        }
        SendResponse(res, answer);
    }

    private void SendResponse(HttpListenerResponse response, string content)
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
}