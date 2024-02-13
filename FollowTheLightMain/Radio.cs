using Npgsql;
using System.Net;
using System.Text;
namespace FollowTheLightMain;


public class Radio
{
    private readonly NpgsqlDataSource _db;

    public Radio(NpgsqlDataSource db)
    {
        _db = db;
    }
    public void GameMessage(HttpListenerResponse response)
    {
        Console.WriteLine("Recieving message");
        const string select = "SELECT message FROM radio WHERE from_player = 1";
        var command = _db.CreateCommand(select);
        command.CommandText = select;
        string message = string.Empty;


        using (var reader = command.ExecuteReader())
        {
            StringBuilder messages = new StringBuilder();
            while (reader.Read())
            {
                message = reader.GetString(0);
                messages.AppendLine(message);
            }

            string AllMessages = messages.ToString();
            byte[] buffer = Encoding.UTF8.GetBytes(AllMessages);
            response.ContentType = "text/plain";
            response.StatusCode = (int)HttpStatusCode.OK;
            response.OutputStream.Write(buffer, 0, buffer.Length);

   

        }
        response.OutputStream.Close();
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
                cmd.Parameters.AddWithValue("@toPlayer", 2);
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

}


