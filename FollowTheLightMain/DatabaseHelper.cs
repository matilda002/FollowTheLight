using Npgsql;

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

    public void PopulateStoryPointsTable()
    {
        Console.WriteLine("Populating the storypoints table...");

        string[] titles = {
            "Intro", "Story One", "Story Two", "Story Three",
            "Story Four", "Story Five", "Story Six", "Story Seven",
            "Story Eight", "Story Nine", "Story Ten", "Story Eleven", "Story Twelve"
        };

        string[][] filePaths = {
            new string[]
            {
                "FollowTheLightMain/storylines/intro.txt",
                "FollowTheLightMain/storylines/sl1/s1.txt",
                "FollowTheLightMain/storylines/sl1/s2.txt",
                "FollowTheLightMain/storylines/sl1/s3.txt",
                "FollowTheLightMain/storylines/sl1/s4.txt",
                "FollowTheLightMain/storylines/sl1/s5.txt",
                "FollowTheLightMain/storylines/sl1/s6.txt",
            },
            new string[]
            {
                "FollowTheLightMain/storylines/sl2/s1.txt",
                "FollowTheLightMain/storylines/sl2/s2.txt",
                "FollowTheLightMain/storylines/sl2/s3.txt",
                "FollowTheLightMain/storylines/sl2/s4.txt",
                "FollowTheLightMain/storylines/sl2/s5.txt",
                "FollowTheLightMain/storylines/sl2/s6.txt",
            }
        };

        var cmd = _db.CreateCommand("insert into storypoints(title, content) values ($1, $2)");
        //sl = storyline, s = storypoint
        for (int sl = 0; sl < filePaths.Length; sl++)
        {
            // Load storypoints for each storyline
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
    public string GetStoryPointContent(int storyPointId)
    {
        string content = "";
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
    
    public void PopulateSpTablePuzzle()
    {
        Console.WriteLine("Populating the storypoints table with puzzles...");
        
        string puzzle1 = File.ReadAllText("FollowTheLightMain/Storylines/puzzles-text/stepstonesp1.txt");
        string puzzle1P2 = File.ReadAllText("FollowTheLightMain/Storylines/puzzles-text/stepstonesp2.txt");
        string puzzle2 = File.ReadAllText("FollowTheLightMain/Storylines/puzzles-text/wallp1.txt");
        string puzzle2P2 = File.ReadAllText("FollowTheLightMain/Storylines/puzzles-text/wallp2.txt");
        string puzzle3 = File.ReadAllText("FollowTheLightMain/Storylines/puzzles-text/lockp1.txt");
        string puzzle3P2 = File.ReadAllText("FollowTheLightMain/Storylines/puzzles-text/lockp2.txt"); 
        
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

        string[] filePaths = {
            "FollowTheLightMain/images/jumpscares/js1.txt",
            "FollowTheLightMain/images/jumpscares/js2.txt",
            "FollowTheLightMain/images/jumpscares/js3.txt",
            "FollowTheLightMain/images/Jumpscares/js4.txt",
            "FollowTheLightMain/images/jumpscares/js5.txt",
            "FollowTheLightMain/images/jumpscares/js6.txt",
            "FollowTheLightMain/images/jumpscares/js7.txt",
            "FollowTheLightMain/images/frog.txt",
            "FollowTheLightMain/images/puzzles/stepstonesp2-stones.txt",
            "FollowTheLightMain/images/puzzles/stepstonesp1-table.txt",
            "FollowTheLightMain/images/puzzles/lockp1-lock.txt", 
            "FollowTheLightMain/images/puzzles/lockp2-door.txt", 
            "FollowTheLightMain/images/puzzles/wallp1-wall.txt", 
            "FollowTheLightMain/images/puzzles/wallp2-sign.txt" 
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
}