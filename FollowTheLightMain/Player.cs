using Npgsql;
using System.Net;
using System.Text;
namespace FollowTheLightMain;

public class Player
{
    private readonly NpgsqlDataSource _db;

    public Player(NpgsqlDataSource db)
    {
        _db = db;
    }

    public void Register(HttpListenerRequest req, HttpListenerResponse res)
    {
        try
        {
            const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
            var db = NpgsqlDataSource.Create(dbUri);
            StreamReader reader = new(req.InputStream, req.ContentEncoding);
            string body = reader.ReadToEnd();
            
            const string queryToDb = "insert into players (username) values (@1)";
            var cmd = db.CreateCommand(queryToDb);
            cmd.Parameters.AddWithValue("@1", body);
            cmd.ExecuteNonQuery();

            string answer = $"Hello, {body}! Welcome to the game.";
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
    public void ViewStatus(HttpListenerRequest req, HttpListenerResponse res)
    {
        const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
        var db = NpgsqlDataSource.Create(dbUri);
        StreamReader reader = new(req.InputStream, req.ContentEncoding);
        string username = reader.ReadToEnd();

        const string checkUsername = "select count(*) from players where username = @1";
        var cmd = db.CreateCommand(checkUsername);
        cmd.Parameters.AddWithValue("@1", username);
        int count = Convert.ToInt32(cmd.ExecuteScalar());
        
        const string queryHp = "select hp from players where username = @1";
        var cmd2 = db.CreateCommand(queryHp);
        cmd2.Parameters.AddWithValue("@1", username);
        int hp = Convert.ToInt32(cmd2.ExecuteScalar());

        const string queryCsp = "select current_storypoint from players where username = @1";
        var cmd3 = db.CreateCommand(queryCsp);
        cmd3.Parameters.AddWithValue("@1", username);
        int csp = Convert.ToInt32(cmd3.ExecuteScalar());
        
        string answer;
        if (count > 0)
        {
            answer = $"\n--Current player status--\n\n{username}\nHP: {hp}\nCurrent storypoint: {csp}\n";
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