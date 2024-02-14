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
        Console.WriteLine("Creating tables...\n");

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
            hp smallint default (5),
            current_storypoint int default (1) references storypoints(storypoint_id),
            unique(username)
        )";
        _db.CreateCommand(playersTable).ExecuteNonQuery();

        const string storypathTable = @"create table if not exists storypaths(
            storypath_id serial primary key,
            from_point int references storypoints(storypoint_id),
            to_point int references storypoints(storypoint_id),
            choice varchar(5),
            effect smallint,
            image_id smallint references images(image_id),
            check(from_point <> to_point),
            unique(from_point, to_point)
        )";
        
        _db.CreateCommand(storypathTable).ExecuteNonQuery();

        const string radioTable = @"create table if not exists radio(
            radio_id serial primary key,
            from_player int references players(player_id),
            to_player int,
            message text
        )";
        _db.CreateCommand(radioTable).ExecuteNonQuery();
    }
}