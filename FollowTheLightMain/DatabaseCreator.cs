using Npgsql;
namespace FollowTheLightMain;

public class DatabaseCreator
{
    private readonly NpgsqlDataSource _db;
    public DatabaseCreator(NpgsqlDataSource db)
    {
        _db = db;
    }

    public void CreateTables()
    {
        Console.WriteLine("--[      CREATE TABLES       ]--\n");

        const string imagesTable = @"create table if not exists images(
            image_id serial primary key,
            image text
        )";
        _db.CreateCommand(imagesTable).ExecuteNonQuery();

        const string storypointTable = @"create table if not exists storypoints(
            storypoint_id serial primary key,
            title text,
            content text
        )";
        _db.CreateCommand(storypointTable).ExecuteNonQuery();

        const string playersTable = @"create table if not exists players(
            player_id serial primary key,
            username text,
            current_storypoint int default (1) references storypoints(storypoint_id),
            unique(username)
        )";
        _db.CreateCommand(playersTable).ExecuteNonQuery();
        
        const string radioTable = @"create table if not exists radio(
            radio_id serial primary key,
            from_player int references players(player_id),
            to_player int,
            message text,
            message_time timestamp DEFAULT current_timestamp
        )";
        _db.CreateCommand(radioTable).ExecuteNonQuery();
    }
}