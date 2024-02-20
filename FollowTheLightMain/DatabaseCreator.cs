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
        Console.WriteLine("[ Tables: Creating ]\n");

        const string createEnum = "create type player_role as enum ('p1','p2');";
        _db.CreateCommand(createEnum).ExecuteNonQuery();
        
        const string imagesTable = @"create table if not exists images(
            image_id serial primary key,
            image text
        )";
        _db.CreateCommand(imagesTable).ExecuteNonQuery();

        const string storypointTable = @"create table if not exists storypoints(
            storypoint_id serial primary key,
            title text,
            player_role player_role,
            content text
);
        ";
        _db.CreateCommand(storypointTable).ExecuteNonQuery();
        
        const string createChoice = "create type choice as enum ('A','B','C','D', 'CONTINUE');;";
        _db.CreateCommand(createChoice).ExecuteNonQuery();
        
        const string storypathTable = @"create table storypaths(
    storypath_id serial primary key,
    from_point   int references storypoints (storypoint_id),
    to_point     int references storypoints (storypoint_id),
    choice       choice not null default 'CONTINUE',
    effect       int    not null default 0,
    image_id     smallint references images(image_id),
    content      text,
    unique (from_point, to_point, choice)
);";
        _db.CreateCommand(storypathTable).ExecuteNonQuery();

        const string playersTable = @"create table players(
    player_id     serial primary key,
    username      text unique,
    hp            int default 5,
    player_role   player_role not null,
    storypoint_id int references storypoints (storypoint_id)
);
        ";
        _db.CreateCommand(playersTable).ExecuteNonQuery();
        
        const string radioTable = @"create table if not exists radio(
            radio_id serial primary key,
            from_player int references players(player_id),
            to_player int,
            message text,
            message_time timestamp DEFAULT current_timestamp
        )";
        _db.CreateCommand(radioTable).ExecuteNonQuery();
        
        const string view = @"create or replace view player_paths as
    select effect, to_point, storypaths.content, choice, username
    from storypaths
         join players on storypoint_id = from_point
         join storypoints on storypoints.storypoint_id = storypaths.to_point
    and players.player_role = storypoints.player_role
        ;";
        _db.CreateCommand(view).ExecuteNonQuery();
    }
    
}