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
        _db.CreateCommand(query).ExecuteNonQueryAsync();
    }

    public void PopulateStoryPointsTable()
    {
        Console.WriteLine("Populating the storypoints table for player one...");

        var cmd = _db.CreateCommand("insert into storypoints(title, content)" +
                                                "values ($1, $2), ($3, $4), ($5,$6), ($7, $8), ($9, $10), ($11, $12), ($13, $14), ($15, $16), ($17, $18),($19, $20), ($21, $22);");

        cmd.Parameters.AddWithValue("Intro");
        cmd.Parameters.AddWithValue("""
                                    Awakening in pitch-black oblivion, memories lost to the void, a bone-chilling cold grips the air. Screams reverberate, swallowed by the oppressive dark. A faint device flickers, its voice pleading, “Who’s there? Where am I?” An unsettling truth lingers—strangers, bound by this abyss, must collaborate to escape.
                                    No past, no exit, just an uneasy pact in this nightmarish hell. Can you unravel the shadows together, or be devoured by the creatures of your own fear? The game begins, and only unity can survive the lurking horrors.
                                    """);
        cmd.Parameters.AddWithValue("Story One");
        cmd.Parameters.AddWithValue("""
                                    In the dark, you find matches. Lighting one reveals a cave with paths on your right and left. Where to? Right or left? Your story starts with a spark in the shadows. You feel fear creeping up.
                                   
                                    A) You go right. Maybe there’s something interesting there.
                                    B) You go left. Maybe there’s a way out there.
                                    C) You stay where you are. Maybe someone will find you there. 
                                    D) You light the whole box of matches. Maybe you’ll see better.
                                    """);
        cmd.Parameters.AddWithValue("Story Two");
        cmd.Parameters.AddWithValue("""
                                    As you walk through the tunnel, you feel something underfoot. It’s a piece of paper. What now? Your choices unfold as you decide what to do next.
                                    
                                    A) You pick up the paper. Maybe there’s something useful on it.
                                    B) You leave the paper. Maybe it’s a trap or a distraction.
                                    C) You burn the paper. Maybe it can be used to fuel your torch. 
                                    D) You eat the paper. Maybe you’re hungry or curious. 
                                    """);
        cmd.Parameters.AddWithValue("Story Three");
        cmd.Parameters.AddWithValue("""
                                    You spot a red frog sitting on a rock, it looks friendly despite the bones scattered around it. What’s your move? Choose wisely as the story continues.
                                    
                                    A) You pet the frog. Maybe it will be your friend.
                                    B) You kiss the frog. Maybe it will turn into something/someone helpful.
                                    C) You ignore the frog. Maybe it’s not important.
                                    D) You poke it with a bone from the ground. Maybe it will move. 
                                    """);
        cmd.Parameters.AddWithValue("Story Four");
        cmd.Parameters.AddWithValue("""
                                    A sinister noise echoes from behind. Quick, what’s your next move? Decide carefully to face the unfolding story.
                                    
                                    A) Quietly run away from the noise. Maybe it’s something dangerous.
                                    B) You hide behind something. Maybe it won’t see or hear you.
                                    C) You make a shushing noise back. Maybe it will be scared of you.
                                    D) You investigate the noise. Maybe it’s something interesting.
                                    """);
        cmd.Parameters.AddWithValue("Story Five");
        cmd.Parameters.AddWithValue("""
                                    A sudden collapse blocks your way with narrow light streaming in from the other side of the stones. Quick, what’s your next move? Decide carefully to face the unfolding story.
                                    
                                    A) You remove some stones. Maybe there’s a hidden passage.
                                    B) You climb over the stones. Maybe there’s a way out.
                                    C) You push the stones. Maybe they will move. 
                                    D) You ignore the stones and sit down and rest, maybe an idea will unfold.
                                    """);
        cmd.Parameters.AddWithValue("Story Six");
        cmd.Parameters.AddWithValue("""
                                    In the passage you notice a tall figure’s shadow in the dark approaching you.
                                    
                                    A) You stand still. Maybe it won’t notice you.
                                    B) You run past it. Maybe you can escape it.
                                    C) You fight back. Maybe you can defeat it. 
                                    D) You chicken out and go back to where the stones collapsed.
                                    """);
        cmd.Parameters.AddWithValue("Story Seven");
        cmd.Parameters.AddWithValue("""
                                    You’ve found the exit, but desperate screams plead for rescue within the cave. What’s your choice now? Leave or venture back into the darkness to investigate?
                                    
                                    A) Leave the cave
                                    B) Go back into the darkness
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