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
        Console.WriteLine("\n--[          RESET           ]--");
        const string query = "drop schema public cascade; create schema public;";
        _db.CreateCommand(query).ExecuteNonQuery();
    }

    public void PopulateStoryPointIntro()
    {
        Console.WriteLine("--[          Intro           ]--");
        string intro = File.ReadAllText("FollowTheLightMain/sources/storylines/intro.txt");
        var cmd = _db.CreateCommand("insert into storypoints(title, content)" +
                                    "values ($1,$2)");
        cmd.Parameters.AddWithValue("Intro");
        cmd.Parameters.AddWithValue($"{intro}");
        cmd.ExecuteNonQuery();
    }
    
    public void PopulateStoryPointsTable()
    {
        Console.WriteLine("--[          Scenes          ]--");

        string[] titles = {
            "Story One", "Story Two", "Story Three",
            "Story Four", "Story Five", "Story Six", "Story Seven",
            "Story Eight", "Story Nine", "Story Ten", "Story Eleven", "Story Twelve"
        };

        string[][] filePaths = {
            new string[]
            {
                "FollowTheLightMain/sources/storylines/sl1/s1.txt",
                "FollowTheLightMain/sources/storylines/sl1/s2.txt",
                "FollowTheLightMain/sources/storylines/sl1/s3.txt",
                "FollowTheLightMain/sources/storylines/sl1/s4.txt",
                "FollowTheLightMain/sources/storylines/sl1/s5.txt",
                "FollowTheLightMain/sources/storylines/sl1/s6.txt",
            },
            new string[]
            {
                "FollowTheLightMain/sources/storylines/sl2/s1.txt",
                "FollowTheLightMain/sources/storylines/sl2/s2.txt",
                "FollowTheLightMain/sources/storylines/sl2/s3.txt",
                "FollowTheLightMain/sources/storylines/sl2/s4.txt",
                "FollowTheLightMain/sources/storylines/sl2/s5.txt",
                "FollowTheLightMain/sources/storylines/sl2/s6.txt",
            }
        };

        var cmd = _db.CreateCommand("insert into storypoints(title, content) values ($1, $2)");
        for (int sl = 0; sl < filePaths.Length; sl++)
        {
            int minCount = Math.Min(titles.Length, filePaths[sl].Length);
            for (int s = 0; s < minCount; s++)
            {
                string content = File.ReadAllText(filePaths[sl][s]);
                cmd.Parameters.AddWithValue(titles[s]);
                cmd.Parameters.AddWithValue(content);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
        }
    }
    public void PopulatePuzzlesText()
    {
        Console.WriteLine("\n--[          Puzzles         ]--");

        string puzzle1 = File.ReadAllText("FollowTheLightMain/sources/storylines/puzzles-text/stepstonesp1.txt");
        string puzzle2 = File.ReadAllText("FollowTheLightMain/sources/storylines/puzzles-text/wallp1.txt");
        string puzzle3 = File.ReadAllText("FollowTheLightMain/sources/storylines/puzzles-text/lockp1.txt");
        string puzzle1P2 = File.ReadAllText("FollowTheLightMain/sources/storylines/puzzles-text/stepstonesp2.txt");
        string puzzle2P2 = File.ReadAllText("FollowTheLightMain/sources/storylines/puzzles-text/wallp2.txt");
        string puzzle3P2 = File.ReadAllText("FollowTheLightMain/sources/storylines/puzzles-text/lockp2.txt"); 

        var cmd = _db.CreateCommand("insert into storypoints(title, content)" +
                                    "values ($1,$2), ($3,$4), ($5,$6), ($7,$8), ($9,$10), ($11,$12)");

        cmd.Parameters.AddWithValue("Puzzle One");
        cmd.Parameters.AddWithValue($"{puzzle1}");
        cmd.Parameters.AddWithValue("Puzzle Two");
        cmd.Parameters.AddWithValue($"{puzzle2}");
        cmd.Parameters.AddWithValue("Puzzle Three");
        cmd.Parameters.AddWithValue($"{puzzle3}");
        cmd.Parameters.AddWithValue("Puzzle One - P2");
        cmd.Parameters.AddWithValue($"{puzzle1P2}");
        cmd.Parameters.AddWithValue("Puzzle Two - P2");
        cmd.Parameters.AddWithValue($"{puzzle2P2}");
        cmd.Parameters.AddWithValue("Puzzle Three - P2");
        cmd.Parameters.AddWithValue($"{puzzle3P2}");
        cmd.ExecuteNonQuery();
    }
    
    public void PopulateStoryPointEnding()
    {
        Console.WriteLine("--[           End            ]--");
        string end = File.ReadAllText("FollowTheLightMain/sources/storylines/end.txt");
        var cmd = _db.CreateCommand("insert into storypoints(title, content)" +
                                    "values ($1,$2)");
        cmd.Parameters.AddWithValue("End");
        cmd.Parameters.AddWithValue($"{end}");
        cmd.ExecuteNonQuery();
    }
    
    public void PopulateImagesTable()
    {
        Console.WriteLine("--[          Images          ]--");

        string[] filePaths = {
            "FollowTheLightMain/sources/images/jumpscares/js1.txt",
            "FollowTheLightMain/sources/images/jumpscares/js2.txt",
            "FollowTheLightMain/sources/images/jumpscares/js3.txt",
            "FollowTheLightMain/sources/images/Jumpscares/js4.txt",
            "FollowTheLightMain/sources/images/jumpscares/js5.txt",
            "FollowTheLightMain/sources/images/jumpscares/js6.txt",
            "FollowTheLightMain/sources/images/jumpscares/js7.txt",
            "FollowTheLightMain/sources/images/frog.txt",
            "FollowTheLightMain/sources/images/puzzles/stepstonesp1-table.txt",
            "FollowTheLightMain/sources/images/puzzles/wallp1-wall.txt",
            "FollowTheLightMain/sources/images/puzzles/lockp1-lock.txt",
            "FollowTheLightMain/sources/images/puzzles/stepstonesp2-stones.txt",
            "FollowTheLightMain/sources/images/puzzles/wallp2-sign.txt",
            "FollowTheLightMain/sources/images/puzzles/lockp2-door.txt", 
        };

        var cmd = _db.CreateCommand("insert into images(image) values ($1)");
        
        foreach (string img in filePaths)
        {
            string content = File.ReadAllText(img); 
            cmd.Parameters.AddWithValue(content);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
    }
    
    public string GetStoryPointContent(int storyPointId)
    {
        string content = string.Empty;
        try
        {
            var cmd = _db.CreateCommand("select content from storypoints where storypoint_id = $1");
            cmd.Parameters.AddWithValue(storyPointId);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                content = reader.GetString(0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving story point content: {ex.Message}");
        }
        return content;
    }
    public string GetImgContent(int imgId)
    {
        string image = string.Empty;
        try
        {
            var cmd = _db.CreateCommand("select image from images where image_id = $1");
            cmd.Parameters.AddWithValue(imgId);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                image = reader.GetString(0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving story point content: {ex.Message}");
        }
        return image;
    }
}