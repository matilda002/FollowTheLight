using Npgsql;
using System.Net;
using System.Text;
namespace FollowTheLightMain;

public class Radio
{
    private readonly NpgsqlDataSource _db;
    public string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";

    public Radio(NpgsqlDataSource db)
    {
        _db = db;
    }

    public void GameMessage(HttpListenerResponse res)
    {
        using (var connection = new NpgsqlConnection(dbUri))
        {

            Console.WriteLine("Recieving message");
            const string selectQuery = @"select p.username, radio.message from radio join players p on radio.from_player = p.player_id where to_player = 1 or from_player = 1 order by radio.message_time desc limit 10";

            using var command = new NpgsqlCommand(selectQuery, connection);
            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                string messages = string.Empty;
                while (reader.Read())
                {
                    string username = reader.GetString(0);
                    string message = reader.GetString(1);
                    messages = $"{username}: {message}\n" + messages;
                }
                SendResponseOk(res, messages);
            }
        }
    }

    public void SendMessageOne(HttpListenerRequest req, HttpListenerResponse res)
    {
        using (var connection = new NpgsqlConnection(dbUri))
        {
            try
            {

                StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
                string chat = reader.ReadToEnd();

                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "insert into radio (from_player, to_player, message) VALUES ($1, $2, $3)";
                    cmd.Parameters.AddWithValue(1);
                    cmd.Parameters.AddWithValue(2);
                    cmd.Parameters.AddWithValue(chat);
                    cmd.ExecuteNonQuery();
                }

                string answer = "Your message has been sent";
                SendResponseOk(res, answer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ Error: " + ex.Message + " ]");
                string answer = "Error";
                SendResponseEx(res, answer);
            }
            finally
            {
                res.Close();
            }
        }
    }

    public void SendMessageTwo(HttpListenerRequest req, HttpListenerResponse res)
    {
        using (var connection = new NpgsqlConnection(dbUri))
        {
            try
            {
                StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
                string chat = reader.ReadToEnd();

                using (var cmd = _db.CreateCommand())
                {
                    cmd.CommandText = "insert into radio (from_player, to_player, message) VALUES ($1, $2, $3)";
                    cmd.Parameters.AddWithValue(2);
                    cmd.Parameters.AddWithValue(1);
                    cmd.Parameters.AddWithValue(chat);
                    cmd.ExecuteNonQuery();
                }

                string answer = "Your message has been sent!";
                SendResponseOk(res, answer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ Error: " + ex.Message + " ]");
                string answer = "Error";
                SendResponseEx(res, answer);
            }
            finally
            {
                res.Close();
            }
        }
    }

    public void SendResponseOk(HttpListenerResponse res, string content)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(content);
        res.ContentType = "text/plain";

        using (Stream output = res.OutputStream)
        {
            output.Write(buffer, 0, buffer.Length);
        }

        res.StatusCode = (int)HttpStatusCode.OK;
        res.Close();
    }
    public void SendResponseEx(HttpListenerResponse res, string content)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(content);
        res.ContentType = "text/plain";

        using (Stream output = res.OutputStream)
        {
            output.Write(buffer, 0, buffer.Length);
        }

        res.StatusCode = (int)HttpStatusCode.InternalServerError;
        res.Close();
    }
}
