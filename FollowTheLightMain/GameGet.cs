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
        // uppdatera storypoint id då man flyttar sig till nästa storypoint
        string updateQuery = @"update players
            set storypoint_id = (
            select sp_to.storypoint_id from storypoints sp_to
            join storypaths sp on sp.to_point = sp_to.storypoint_id
            join players p ON p.storypoint_id = sp.from_point
            where p.username = $1)
            where username = $1;";

        var updateCmd = _db.CreateCommand();
        updateCmd.CommandText = updateQuery;
        updateCmd.Parameters.AddWithValue("$1", username); 
        
        
        // query för att visa storypoint content för rätt spelare beroende på anv.namn
        string query = @"select title, content
        from storypoints
        join players on storypoints.storypoint_id = players.storypoint_id
        where (players.player_role = storypoints.player_role or storypoints.player_role is null)
        and username = $1;";
        var cmd = _db.CreateCommand();
        cmd.CommandText = queryresult;
        cmd.Parameters.AddWithValue("$1", username);
        
        string result = "";
        using (var reader = updateCmd.ExecuteReader())
        {
            if (reader.Read())
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