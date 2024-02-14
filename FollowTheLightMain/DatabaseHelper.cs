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
                "FollowTheLightMain/storylines/intro.txt",
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

    public void PopulateImagesTable()
    {
        Console.WriteLine("Populating the images table...");

        string js1 = File.ReadAllText($"FollowTheLightMain/images/jumpscares/js-1.txt");
        string js2 = File.ReadAllText($"FollowTheLightMain/images/jumpscares/js-2.txt");
        string js3 = File.ReadAllText($"FollowTheLightMain/images/jumpscares/js-3.txt");
        string js4 = File.ReadAllText($"FollowTheLightMain/images/Jumpscares/js-4.txt");
        string js5 = File.ReadAllText($"FollowTheLightMain/images/jumpscares/js-5.txt");
        string js6 = File.ReadAllText($"FollowTheLightMain/images/jumpscares/js-6.txt");
        string imgFrog = File.ReadAllText($"FollowTheLightMain/images/frog.txt");
        

        var cmd = _db.CreateCommand("insert into images(image)" +
                                                "values ($1), ($2), ($3), ($4), ($5), ($6), ($7)");
        
        cmd.Parameters.AddWithValue($"{js1}");
        cmd.Parameters.AddWithValue($"{js2}");
        cmd.Parameters.AddWithValue($"{js3}");
        cmd.Parameters.AddWithValue($"{js4}");
        cmd.Parameters.AddWithValue($"{js5}");
        cmd.Parameters.AddWithValue($"{js6}");
        cmd.Parameters.AddWithValue($"{imgFrog}"); 

        cmd.ExecuteNonQuery();
    }
}