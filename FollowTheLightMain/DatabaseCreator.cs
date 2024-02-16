﻿using Npgsql;

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

        const string playerRole = @"CREATE TYPE player_role AS ENUM ('1P', '2P', 'CO');";
        _db.CreateCommand(playerRole).ExecuteNonQuery();

        const string storypointTable = @"create table if not exists storypoints(
            storypoint_id serial primary key,
            title text,
            player player_role,
            content text
        )";
        _db.CreateCommand(storypointTable).ExecuteNonQuery();

        const string storypathTable = @"create table if not exists storypaths(
            storypath_id serial primary key,
            player player_role,
            from_point int references storypoints(storypoint_id),
            to_point int references storypoints(storypoint_id),
            choice varchar(5),
            effect smallint,
            image_id smallint references images(image_id),
            check(from_point <> to_point),
            unique(player, from_point, to_point, choice)
        )";
        _db.CreateCommand(storypathTable).ExecuteNonQuery();

        const string playersTable = @"create table if not exists players(
            player_id serial primary key,
            username text,
            hp smallint default (5),
            player player_role,
            storypath_id int references storypaths(storypath_id),
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