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
        _db.CreateCommand(query).ExecuteNonQueryAsync();
    }

    public void PopulateStoryPointsTable()
    {
        Console.WriteLine("Populating the storypoints table for player one...");

        var cmd = _db.CreateCommand("insert into storypoints(title, content)" +
                                                "values ($1, $2), ($3, $4), ($5,$6), ($7, $8), ($9, $10), ($11, $12), ($13, $14), ($15, $16), ($17, $18),($19, $20), ($21, $22);");

        cmd.Parameters.AddWithValue("Intro");
        cmd.Parameters.AddWithValue("""
                                    Awakening in dark oblivion, memories lost to the void, a bone-chilling cold grips the air. Your screams only echoes, swallowed by the oppressive cave. A faint device flickers on the ground, its voice pleading: Who is there? Where am I?
                                    An unsettling truth lingers-strangers, bound by this abyss, must collaborate to escape. No past, no exit, just an uneasy pact in this nightmare. Can you unravel the shadows together, or be consumed by the echoes of your own fear? The game begins, and only unity can conquer the lurking horrors.
                                    """);
        cmd.Parameters.AddWithValue("Story One");
        cmd.Parameters.AddWithValue("""
                                    In the dark, you find matches. Lighting one reveals more of the cave with paths on your right and left. Where do you choose to go? Your story starts with a spark in the shadows.

                                    A. To your right
                                    B. To your left
                                    """);
        cmd.Parameters.AddWithValue("Story Two");
        cmd.Parameters.AddWithValue("""
                                    As you walk through the tunnel, you feel something underfoot. It's a piece of paper. What now? Your choices unfold as you decide what to do with it.

                                    A. Eat it
                                    B. Throw it behind you
                                    C. Burn it with a match
                                    D. Read it
                                    """);
        cmd.Parameters.AddWithValue("Story Three");
        cmd.Parameters.AddWithValue("""
                                    You spot a red frog sitting on a rock, it looks friendly despite the bones scattered around it. What's your move? Decide carefully to face the consequences.

                                    A. Burn it with your torch
                                    B. Walk past the frog
                                    C. Pick it up with your hands
                                    D. Poke it with a bone from the ground
                                    """);
        cmd.Parameters.AddWithValue("Story Four");
        cmd.Parameters.AddWithValue("""
                                    A sinister noise echoes from behind. Quick, what's your next move? Choose wisely as the story continues.

                                    A. Make a shushing noise
                                    B. Hide behind something
                                    C. Look back and investigate the noise
                                    D. Scream and run
                                    """);
        cmd.Parameters.AddWithValue("Story Five");
        cmd.Parameters.AddWithValue("""
                                    Facing a towering wall, your only option is to climb it. What's your choice?

                                    A. Climb the slippery wall
                                    B. Look around the area
                                    C. Climb the rope
                                    D. Take the ladder with missing steps
                                    """);
        cmd.Parameters.AddWithValue("Story Six"); // story six -> nine are only drafts, not the finished product.
        cmd.Parameters.AddWithValue("""
                                    Reaching the new surface you see withered skulls around you. There is a creepy altar with a candle in the middle of it.

                                    A. Eat the candle
                                    B. Light yourself on fire, the skulls were the last straw
                                    C. Use one of the matches to light up the candle
                                    D. Throw a skull at the alter
                                    """);
        cmd.Parameters.AddWithValue("Story Seven");
        cmd.Parameters.AddWithValue("""
                                    You've found the exit, but desperate screams plead for rescue within the cave. What's your choice now? Leave or venture back into the darkness to investigate?
                                    
                                    A. Leave the cave
                                    B. Go back into the darkness
                                    """);
        cmd.Parameters.AddWithValue("Challenge One");
        cmd.Parameters.AddWithValue("""
                                    You step into an open space with a pool of water and some sort stepping stones with symbols on them. It looks like different paths leading to the other side. While trying to look around for clues on where to step, you have no luck...
                                    Maybe ask the other person how it looks for them or risk your life...You never know what's lurking in the water
                                    """);
        cmd.Parameters.AddWithValue("Challenge Two");
        cmd.Parameters.AddWithValue("""
                                    You come across a creaky half-open door, what's your next move?

                                    A. Push the door and enter cautiously
                                    B. Ignore the door and change your direction
                                    C. Knock on the door to check for a response
                                    D. <choice>
                                    """);
        cmd.Parameters.AddWithValue("Challenge Three");
        cmd.Parameters.AddWithValue("""
                                    You've found the exit, but desperate screams plead for rescue within the cave. What's your choice now? Leave or venture back into the darkness to investigate?

                                    A. Leave the cave
                                    B. Go back into the darkness
                                    """);

        cmd.ExecuteNonQueryAsync();
    }

    public void PopulateStoryPointsTableTwo()
    {
        Console.WriteLine("Populating the storypoints table for player two...");

        var cmd = _db.CreateCommand("insert into storypoints(title, content)" +
                                    "values ($1, $2)");
        
        cmd.Parameters.AddWithValue("Challange One - P2");
        cmd.Parameters.AddWithValue("""
                                    You step inside an open space, with no direction but to go forward. In the distance you see a light and when you arrive you see a candle on a table. 
                                    On closer inspection there are three different symbols repeated in a line...The symbols are only on the table, no use for you but maybe the other person?
                                    """);
        cmd.ExecuteNonQueryAsync();
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
        string imgStepStone = File.ReadAllText($"FollowTheLightMain/images/steppingstones.txt");
        string imgStepStoneTable = File.ReadAllText($"FollowTheLightMain/images/steppingstonestable.txt");
        

        var cmd = _db.CreateCommand("insert into images(image)" +
                                                "values ($1), ($2), ($3), ($4), ($5), ($6), ($7), ($8), ($9)");
        
        cmd.Parameters.AddWithValue($"{js1}");
        cmd.Parameters.AddWithValue($"{js2}");
        cmd.Parameters.AddWithValue($"{js3}");
        cmd.Parameters.AddWithValue($"{js4}");
        cmd.Parameters.AddWithValue($"{js5}");
        cmd.Parameters.AddWithValue($"{js6}");
        cmd.Parameters.AddWithValue($"{imgFrog}");
        cmd.Parameters.AddWithValue($"{imgStepStone}");
        cmd.Parameters.AddWithValue($"{imgStepStoneTable}");
        
        cmd.ExecuteNonQueryAsync();
    }
}