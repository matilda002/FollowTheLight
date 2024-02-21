using System.Net;
using System.Text;
using Npgsql;

namespace FollowTheLightMain;

public class GameGet
{
    private readonly DatabaseHelper _dbHelper; 
    private readonly NpgsqlDataSource _db;

    public GameGet(NpgsqlDataSource db)
    {
        _db = db;
        _dbHelper = new(db);
    } 
    public void Game(HttpListenerRequest request, HttpListenerResponse response)
    {
        StreamReader reader1 = new StreamReader(request.InputStream, request.ContentEncoding);
        string username = reader1.ReadToEnd().ToLower();
        
        string queryHpUpdate = @"UPDATE players SET hp = hp - (select effect 
        from player_paths where choice = @2 AND username = @1),
            storypoint_id = (SELECT to_point FROM player_paths WHERE choice = @2 AND username = @1)
        WHERE username = @1;
        SELECT content FROM player_paths WHERE choice = @2 AND username = @1;";
        var updateCmd = _db.CreateCommand(queryHpUpdate);
        updateCmd.Parameters.AddWithValue("@1", username);
        updateCmd.Parameters.AddWithValue("@2", "CONTINUE");
        
        var result = "";
        using (var reader = updateCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                result = reader.GetString(0); 
            }
        }
        SendResponse(response, result);
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