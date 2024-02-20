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
        const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
        using (var connection = new NpgsqlConnection(dbUri))
        {

            Console.WriteLine("Recieving message");
            const string selectQuery = @"select p.username, radio.message from radio join players p on radio.from_player = p.player_id where to_player = 1 or from_player = 1 order by radio.message_time desc limit 10";

            using (var command = new NpgsqlCommand(selectQuery, connection))
            {
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


                    byte[] buffer = Encoding.UTF8.GetBytes(messages);
                    response.ContentType = "text/plain";
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                }
            }
        }
        response.OutputStream.Close();
    }

    public void SendMessageOne(HttpListenerRequest req, HttpListenerResponse res)
    {
        const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
        using (var connection = new NpgsqlConnection(dbUri))
        {
            try
            {

                StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
                string chat = reader.ReadToEnd();

                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO radio (from_player, to_player, message) VALUES (@fromPlayer, @toPlayer, @message)";
                    cmd.Parameters.AddWithValue("@fromPlayer", 1);
                    cmd.Parameters.AddWithValue("@toPlayer", 2);
                    cmd.Parameters.AddWithValue("@message", chat);
                    cmd.ExecuteNonQuery();
                }


                string answer = "Your message has been sent";
                byte[] buffer = Encoding.UTF8.GetBytes(answer);
                res.ContentType = "text/plain";
                res.StatusCode = (int)HttpStatusCode.OK;
                res.OutputStream.Write(buffer, 0, buffer.Length);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.ToString());
                string answer = "Error";
                byte[] buffer = Encoding.UTF8.GetBytes(answer);
                res.ContentType = "text/plain";
                res.StatusCode = (int)HttpStatusCode.InternalServerError;
                res.OutputStream.Write(buffer, 0, buffer.Length);
                res.Close();
            }
            finally
            {
                res.Close();
            }
        }
    }

    public void SendMessageTwo(HttpListenerRequest req, HttpListenerResponse res)
    {
        const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
        using (var connection = new NpgsqlConnection(dbUri))
        {
            try
            {
                StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
                string chat = reader.ReadToEnd();

                using (var cmd = _db.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO radio (from_player, to_player, message) VALUES (@fromPlayer, @toPlayer, @message)";
                    cmd.Parameters.AddWithValue("@fromPlayer", 2);
                    cmd.Parameters.AddWithValue("@toPlayer", 1);
                    cmd.Parameters.AddWithValue("@message", chat);
                    cmd.ExecuteNonQuery();
                }


                string answer = "Your message has been sent";
                byte[] buffer = Encoding.UTF8.GetBytes(answer);
                res.ContentType = "text/plain";
                res.StatusCode = (int)HttpStatusCode.OK;
                res.OutputStream.Write(buffer, 0, buffer.Length);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.ToString());
                string answer = "Error";
                byte[] buffer = Encoding.UTF8.GetBytes(answer);
                res.ContentType = "text/plain";
                res.StatusCode = (int)HttpStatusCode.InternalServerError;
                res.OutputStream.Write(buffer, 0, buffer.Length);
            }
            finally
            {
                res.Close();
            }
        }
    }

}


