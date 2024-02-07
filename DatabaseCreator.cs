﻿using System.Data.Common;
using System.Runtime.CompilerServices;
using Npgsql;

namespace FollowTheLightdb;


public class DatabaseCreator
{
    private readonly NpgsqlDataSource _db;
    public DatabaseCreator(NpgsqlDataSource db)
    {
        _db = db;
    }

    public async Task CreateTables()
    {
        Console.WriteLine("Creating tables");

        const string playersTable = @"create table players(
        player_id serial primary key,
        username text,
        hp smallint,
        current_storypoint int,
        unique(username)
        )";
        await _db.CreateCommand(playersTable).ExecuteNonQueryAsync();

        const string storypointTable = @"create table storypoint(
            storypoint_id serial primary key,
            title text,
            content text,
            effect smallint
        )";

        const string storypathTable = @"create table storypaths(
            storypath_id serial primary key,
            from_point int references storypoint(storypoint_id),
            to_point int references storypoint(storypoint_id),
            choice int,
            check(from_point <> to_point),
            unique(from_point, to_point)
        )";

        const string radioTable = @"create table radio(
            radio_id serial primary key,
            from_player int references players(player_id),
            to_player int,
            message text
        )";



    }
}