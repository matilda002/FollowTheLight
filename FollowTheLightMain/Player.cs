using Npgsql;
using System.Net;
using System.Text;
namespace FollowTheLightMain;

public class Player
{
    public string Username; 

    public Player(string username)
    {
        Username = username;
    }
    public void RegisterPost(HttpListenerRequest req, HttpListenerResponse res)
    {
        try
        {
            const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
            var db = NpgsqlDataSource.Create(dbUri);
            StreamReader reader = new(req.InputStream, req.ContentEncoding);
            string username = reader.ReadToEnd();

            Player activeUsername = new Player(username);
            
            string queryToDb = "insert into players (username) values (@1)";
            var cmd = db.CreateCommand(queryToDb);
            cmd.Parameters.AddWithValue("@1", username);
            cmd.ExecuteNonQuery();

            string answer = $"Hello, {username}! Welcome to the game.";
            byte[] buffer = Encoding.UTF8.GetBytes(answer);
            res.ContentType = "text/plain";
            res.StatusCode = (int)HttpStatusCode.Created;
            res.OutputStream.Write(buffer, 0, buffer.Length);
            
            res.Close();
        }
        catch
        {
            string answer = "error occured";
            byte[] buffer = Encoding.UTF8.GetBytes(answer);
            res.ContentType = "text/plain";
            res.OutputStream.Write(buffer, 0, buffer.Length);
            res.StatusCode = (int)HttpStatusCode.NotFound;
            res.Close();
        }
    }

    public void LoginPost(HttpListenerRequest req, HttpListenerResponse res)
    {
        const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
        var db = NpgsqlDataSource.Create(dbUri);
        StreamReader reader = new(req.InputStream, req.ContentEncoding);
        string username = reader.ReadToEnd();

        string query = "select count(*) from players where username = @1";
        var cmd = db.CreateCommand(query);
        cmd.Parameters.AddWithValue("@1", username);
        int count = Convert.ToInt32(cmd.ExecuteScalar());

        string answer;
        if (count > 0)
        {
            answer = "Username correct.";
        }
        else
        {
            answer = "Username incorrect.";
        }
        
        byte[] buffer = Encoding.UTF8.GetBytes(answer);
        res.ContentType = "text/plain";
        res.StatusCode = (int)HttpStatusCode.OK;
        res.OutputStream.Write(buffer, 0, buffer.Length);
        
        res.Close();
    }
}