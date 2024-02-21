using System.Net;
using System.Text;
using Npgsql;
namespace FollowTheLightMain;

public class GamePost
{
    private readonly NpgsqlDataSource _db;
    public GamePost(NpgsqlDataSource db)
    {
        _db = db;
    } 
    public void Game(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader1 = new StreamReader(req.InputStream, req.ContentEncoding);
        string username = reader1.ReadToEnd().ToLower();
       // metod ska ta in svar till fråga och anv namn från curl samt parsa de
       // query ska skicka ut rätt svartext
       // query ska uppdatera hp beroende på svar
        string queryresult = @"update players
        set hp= hp - (select effect from player_paths where choice = @2 and username = @1),
        storypoint_id = (select to_point from player_paths where choice = @2 and username = @1)
        where username = @1
        returning (select content from player_paths where choice = @2 and username = @1);";
        var updateCmd = _db.CreateCommand(queryresult);
        updateCmd.Parameters.AddWithValue("$1", username);
        updateCmd.Parameters.AddWithValue("$2", "CONTINUE");

        string result = "";
        using (var reader = updateCmd.ExecuteReader())
        {
            if (reader.Read())
            {
                result = reader.GetString(0);
            }
        }
        SendResponse(res, result);
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
        response.StatusCode = (int)HttpStatusCode.Created;
        response.Close();
    }
}