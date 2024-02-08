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
        Console.WriteLine("Resetting tables...");
        const string query = "drop schema public cascade; create schema public;";
        await _db.CreateCommand(query).ExecuteNonQueryAsync();
    }
    public async Task PopulateStoryPointsTable()
    {
        await using var cmd = _db.CreateCommand(@"insert into storypoints(title, content) values ($1, $2), ($3, $4);");

        cmd.Parameters.AddWithValue("Intro");
        cmd.Parameters.AddWithValue(@"Awakening in dark oblivion, memories lost to the void, a bone-chilling cold grips the air. Your screams only echoes, swallowed by the oppressive cave. A faint device flickers on the ground, its voice pleading: Who is there? Where am I?
An unsettling truth lingers-strangers, bound by this abyss, must collaborate to escape. No past, no exit, just an uneasy pact in this nightmare. Can you unravel the shadows together, or be consumed by the echoes of your own fear? The game begins, and only unity can conquer the lurking horrors.");

        cmd.Parameters.AddWithValue("Story One");
        cmd.Parameters.AddWithValue(@"In the dark, you find matches. Lighting one reveals more of the cave with paths on your right and left. Where do you choose to go? Your story starts with a spark in the shadows.

A. To your right
B. To your left");
        await cmd.ExecuteNonQueryAsync();
    }
}