using Npgsql;
using System.Net;
using System.Text;

namespace FollowTheLightMain
{
    public class Radio
    {
        private readonly NpgsqlDataSource _db;

        public Radio(NpgsqlDataSource db)
        {
            _db = db;
        }

        public void StoreChat()
        {
            string chat = Console.ReadLine();

            using (var cmd = _db.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS radio(value TEXT)";
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _db.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO radio (value) VALUES (@value)";
                cmd.Parameters.AddWithValue("value", chat);
                cmd.ExecuteNonQuery();
            }

            using (var cmd = _db.CreateCommand())
            {

                cmd.CommandText = "SELECT * FROM radio";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0));
                    }
                }
            }

        }

    }

}
