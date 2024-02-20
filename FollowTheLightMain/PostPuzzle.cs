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
    public void PuzzleOneP1(HttpListenerRequest req, HttpListenerResponse res)
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
                          "DON'T TRUST THEE"
                          """;
                break;
            default:
                answer += "A huge cockroach helped you move the rock, and gave you dirty look.\nYou move on with shame";
                break;
        }
        SendResponse(res, answer);
    }
    public void PuzzleTwoP1(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToUpper();
        string answer = string.Empty;

        switch (body)
        {
            case "NOP":
                answer += """
                          You cracked the code, and a message appears on the wall:
                          
                          "There is always ears hearing your every move,
                          In the darkness, an ear is an eye."
                          """;
                break;
            default:
                answer += """
                          The letters stopped glowing, and won't lit up again. You let out a sigh and
                          hear a chuckle in the distance..."
                          """;
                break;
        }
        SendResponse(res, answer);
    }
    public void PuzzleThreeP1(HttpListenerRequest req, HttpListenerResponse res)
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
            default:
                answer += "The code lock exploded and looks like it burned up a piece of paper\n";
                break;
        }
        SendResponse(res, answer);
    }
    
    // Player Two
    public void PuzzleOneP2(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd();
        string answer = string.Empty;

        switch (body)
        {
            case "0%@00@%":
                answer += """
                          You made it past the water just in time before something dragged you in...
                          Remember, water can be dangerous. In the dark anything can hide
                          """;
                break;
            default:
                DatabaseHelper dbHelper = new(_db);
                string js = dbHelper.GetImgContent(7);
                answer += $"You fell into the water while stepping on the wrong stone...\n{js}";
                break;
        }
        SendResponse(res, answer);
    }
    public void PuzzleTwoP2(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToUpper();
        string answer = string.Empty;

        switch (body)
        {
            case "ANTILOP":
                answer += """
                          Dragon let's you continue and whispers:
                          "It might be light as a feather, but no less sturdy"
                          """;
                break;
            default:
                answer += """
                          "Not the correct answer my friend", says the dragon in a disappointed tone.
                          It still moves out of your way, but you feel it's deathly stare..
                          """;
                break;
        }
        SendResponse(res, answer);
    }
    public void PuzzleThreeP2(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToUpper();
        string answer = string.Empty;

        switch (body)
        {
            case "ENTER":
                answer += """
                          You opened the door! Right when you take a step to the other side, a piece of
                          leather falls on your face. It's almost like human skin with an engraving:
                          "Maybe you're the only one left..."
                          """;
                break;
            default:
                answer += "You slip on a piece of leather, and suddenly a giant cockroach steals it\n";
                break;
        }
        SendResponse(res, answer);
    }

    private void SendResponse(HttpListenerResponse response, string content)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(content);
        response.ContentType = "text/plain";
        
        using (Stream output = response.OutputStream)
        {
            output.Write(buffer, 0, buffer.Length);
        }
        
        response.StatusCode = (int)HttpStatusCode.OK;
        response.Close();
    }
}