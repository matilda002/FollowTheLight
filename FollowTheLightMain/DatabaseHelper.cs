﻿using Npgsql;

namespace FollowTheLightMain;

public class DatabaseHelper
{
    private readonly NpgsqlDataSource _db;

    public DatabaseHelper(NpgsqlDataSource db)
    {
        _db = db;
    }

    
    public void ResetTables()
    {
        Console.WriteLine("Resetting tables...");
        const string query = "drop schema public cascade; create schema public;";
        _db.CreateCommand(query).ExecuteNonQuery();
    }
 
    

    
    public void PopulateSpTablePuzzle() 
    {
        Console.WriteLine("Populating the storypoints table with puzzles...");

        string puzzle1 = File.ReadAllText($"FollowTheLightMain/Storylines/puzzles-text/lockp1.txt");
        string puzzle1P2 = File.ReadAllText($"FollowTheLightMain/Storylines/puzzles-text/lockp2.txt");
        string puzzle2 = File.ReadAllText($"FollowTheLightMain/Storylines/puzzles-text/stepstonesp1.txt");
        string puzzle2P2 = File.ReadAllText($"FollowTheLightMain/Storylines/puzzles-text/stepstonesp2.txt");
        string puzzle3 = File.ReadAllText($"FollowTheLightMain/Storylines/puzzles-text/wallp1.txt");
        string puzzle3P2 = File.ReadAllText($"FollowTheLightMain/Storylines/puzzles-text/wallp2.txt");
        
        var cmd = _db.CreateCommand("insert into storypoints(title, content)" +
                                    "values ($1,$2), ($3,$4), ($5,$6), ($7,$8), ($9,$10), ($11,$12)");

        cmd.Parameters.AddWithValue("Challenge One");
        cmd.Parameters.AddWithValue($"{puzzle1}");
        cmd.Parameters.AddWithValue("Challenge One - P2");
        cmd.Parameters.AddWithValue($"{puzzle1P2}");
        cmd.Parameters.AddWithValue("Challenge Two");
        cmd.Parameters.AddWithValue($"{puzzle2}");
        cmd.Parameters.AddWithValue("Challenge Two - P2");
        cmd.Parameters.AddWithValue($"{puzzle2P2}");
        cmd.Parameters.AddWithValue("Challenge Three");
        cmd.Parameters.AddWithValue($"{puzzle3}"); 
        cmd.Parameters.AddWithValue("Challenge Three - P2");
        cmd.Parameters.AddWithValue($"{puzzle3P2}");
        cmd.ExecuteNonQuery();
    }

    public void PopulateImagesTable()
    {
        Console.WriteLine("Populating the images table...\n\n");

        string js1 = File.ReadAllText($"FollowTheLightMain/images/jumpscares/js1.txt");
        string js2 = File.ReadAllText($"FollowTheLightMain/images/jumpscares/js2.txt");
        string js3 = File.ReadAllText($"FollowTheLightMain/images/jumpscares/js3.txt");
        string js4 = File.ReadAllText($"FollowTheLightMain/images/Jumpscares/js4.txt");
        string js5 = File.ReadAllText($"FollowTheLightMain/images/jumpscares/js5.txt");
        string js6 = File.ReadAllText($"FollowTheLightMain/images/jumpscares/js6.txt");
        string js7 = File.ReadAllText($"FollowTheLightMain/images/jumpscares/js7.txt");
        string imgFrog = File.ReadAllText($"FollowTheLightMain/images/frog.txt");
        string imgStepStone = File.ReadAllText($"FollowTheLightMain/images/puzzles/stepstonesp2-stones.txt");
        string imgStepStoneTable = File.ReadAllText($"FollowTheLightMain/images/puzzles/stepstonesp1-table.txt");
        string imgLock = File.ReadAllText($"FollowTheLightMain/images/puzzles/lockp1-lock.txt"); 
        string imgLockDoor = File.ReadAllText($"FollowTheLightMain/images/puzzles/lockp2-door.txt"); 
        string imgGlowingWall = File.ReadAllText($"FollowTheLightMain/images/puzzles/wallp1-wall.txt");
        string imgGlowingWallSign = File.ReadAllText($"FollowTheLightMain/images/puzzles/wallp2-sign.txt"); 
        

        var cmd = _db.CreateCommand("insert into images(image)" +
                                                "values ($1), ($2), ($3), ($4), ($5), ($6), ($7), ($8), ($9), ($10), ($11), ($12), ($13), ($14)");
        
        cmd.Parameters.AddWithValue($"{js1}");
        cmd.Parameters.AddWithValue($"{js2}");
        cmd.Parameters.AddWithValue($"{js3}");
        cmd.Parameters.AddWithValue($"{js4}");
        cmd.Parameters.AddWithValue($"{js5}");
        cmd.Parameters.AddWithValue($"{js6}");
        cmd.Parameters.AddWithValue($"{js7}");
        cmd.Parameters.AddWithValue($"{imgFrog}");
        cmd.Parameters.AddWithValue($"{imgStepStone}");
        cmd.Parameters.AddWithValue($"{imgStepStoneTable}");
        cmd.Parameters.AddWithValue($"{imgLock}");
        cmd.Parameters.AddWithValue($"{imgLockDoor}");
        cmd.Parameters.AddWithValue($"{imgGlowingWall}");
        cmd.Parameters.AddWithValue($"{imgGlowingWallSign}");
        
        cmd.ExecuteNonQuery();
    }

    public void PopulateTableStorypaths()
    {
        Console.WriteLine("Populating storypaths-table...");

        const string query = @"INSERT INTO storypaths(
        from_point,
        to_point,
        choice,
        effect
        )
        VALUES ($1, $2, $3, $4)
       
        ;";

        string[] storypath = File.ReadAllLines($"../../../DATA/storypaths.csv");
        for (int i = 1; i <  storypath.Length; i++)
        {
            string[] data = storypath[i].Split(',');
            using (var cmd = _db.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue( int.Parse(data[0]));
                cmd.Parameters.AddWithValue(int.Parse(data[1]));
                cmd.Parameters.AddWithValue( data[2]);
                cmd.Parameters.AddWithValue( int.Parse(data[3]));
                cmd.ExecuteNonQuery();
            }
        }
    }
}
