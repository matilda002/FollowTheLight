﻿using System.Net;
using System.Text;
using Npgsql;

namespace FollowTheLightMain;

public class GameGet
{
    private readonly NpgsqlDataSource _db;
    public GameGet(NpgsqlDataSource db)
    {
        _db = db;
    } 
    public void Game(HttpListenerRequest request, HttpListenerResponse response)
    {
        StreamReader reader1 = new StreamReader(request.InputStream, request.ContentEncoding);
        string username = reader1.ReadToEnd().ToLower();

        // query to update player storypoint based on the current one
        string updateQuery = @"update players
                           set storypoint_id = (
                               select sp_to.storypoint_id 
                               from storypoints sp_to
                               join storypaths sp on sp.to_point = sp_to.storypoint_id
                               join players p ON p.storypoint_id = sp.from_point
                               where p.username = $1)
                           where username = $1;";

        var updateCmd = _db.CreateCommand();
        updateCmd.CommandText = updateQuery;
        updateCmd.Parameters.AddWithValue("$1", username);
        updateCmd.ExecuteNonQuery();

        //call view
        string callView = "select storypoint_content from player_storypoints";
        var viewCmd = _db.CreateCommand();
        viewCmd.CommandText = callView;
        
        string result = "";
        using (var reader = viewCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                string storypointContent = reader["storypoint_content"].ToString();
                result += storypointContent;
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