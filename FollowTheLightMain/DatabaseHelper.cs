using System.Reflection;
using Npgsql;


namespace FollowTheLightMain;
public class DatabaseHelper
{
    private readonly NpgsqlDataSource _db;
    public DatabaseHelper(NpgsqlDataSource db)
    {
        _db = db;
    }
    public async Task ResetTables()
    {
        Console.WriteLine("Resetting tables...\n");
        const string query = "drop schema public cascade; create schema public;";
        await _db.CreateCommand(query).ExecuteNonQueryAsync();
    }
    public async Task PopulateStoryPointsTable()
    {
        using var cmd = _db.CreateCommand(@"INSERT INTO storypoints(title, content) VALUES ($1, $2);");

        cmd.Parameters.AddWithValue("Intro");
        cmd.Parameters.AddWithValue(@"You have woken up in darkness with no past memories. It is cold and when you scream for help it echoes...
        You hear a faint voice coming from a device on the ground. You pick it up and someone responds with: Who is this? Where am I?. 
        After a while they realise no-one knows how they got there. All they know is they have to escape this place through working together...");
        await cmd.ExecuteNonQueryAsync();
    }
    
}