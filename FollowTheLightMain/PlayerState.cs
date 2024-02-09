using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowTheLightMain
{
    public class PlayerState
    {
        private readonly NpgsqlDataSource _db;

        public PlayerState(NpgsqlDataSource db)
        {
            _db = db;
        }

        public void OnlineStatus(string username, bool isOnline)
        {
            string updateQuery = $"UPDATE players SET is_online = {isOnline} WHERE username = '{username}";
            _db.CreateCommand(updateQuery).ExecuteNonQuery();

            Console.WriteLine($"{username} is {(isOnline ? "online" : "offline")}.");
        }

        public bool GetOnlineStatus(string username)
        {
            string selectQuery = $"SELECT is_online FROM players WHERE username = '{username}'";
            using (var reader = _db.CreateCommand(selectQuery).ExecuteReader())
            if (reader.Read())
                {
                    return reader.GetBoolean(0);
                }
            else
            {
                throw new ArgumentException("Invalid player ID.");
            }
        }

    }
}