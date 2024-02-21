using Npgsql;
using System.Net;
using System.Text;
namespace FollowTheLightMain;

public class Player
{
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
        catch (Exception ex)
        {
            string answer = "[ Error: " + ex.Message + " ]";
            byte[] buffer = Encoding.UTF8.GetBytes(answer);
            res.ContentType = "text/plain";
            res.OutputStream.Write(buffer, 0, buffer.Length);
            res.StatusCode = (int)HttpStatusCode.NotFound;
            res.Close();
        }
    }
}