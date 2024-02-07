using Npgsql;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        const string query = @"INSERT INTO storyponts(
        title, 
        content, 
        ) 
        VALUES ('Intro', 
        'You've woken up in darkness with no past memories. It's cold and when you scream for help it echoes...
You hear a faint voice coming from a device on the ground. You pick it up and someone responds with 'Who is this? Where am I?'. 
After a while they realise no-one knows how they got there. All they know is they have to escape this place through working together...'
);";

        var cmd = _db.CreateCommand(query);
        await cmd.ExecuteNonQueryAsync();
    }
}