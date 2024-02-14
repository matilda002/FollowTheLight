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
            const string selectQuery = "SELECT a.message, b.username FROM radio a INNER JOIN players b ON a.from_player = b.player_id WHERE a.from_player = @playerId1";

            using (var command = new NpgsqlCommand(selectQuery, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@playerId1", 1);

                using (var reader = command.ExecuteReader())
                {
                    StringBuilder messages = new StringBuilder();
                    while (reader.Read())
                    {
                        string message = reader.GetString(0);
                        string username = reader.GetString(1);

                        messages.AppendLine($"{username}: {message}");
                    }

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@playerId1", 2);
                    reader.Close();

                    using (var readerTwo = command.ExecuteReader())
                    {
                        while (readerTwo.Read())
                        {
                            string message = readerTwo.GetString(0);
                            string username = readerTwo.GetString(1);

                            messages.AppendLine($"{username}: {message}");
                        }


                        string allMessages = messages.ToString();
                        byte[] buffer = Encoding.UTF8.GetBytes(allMessages);
                        response.ContentType = "text/plain";
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
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


