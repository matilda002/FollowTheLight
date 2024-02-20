using System.Net;
using System.Text;
namespace FollowTheLightMain;

public class GamePost
{
    public void GameOne(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToLower();
        string answer = string.Empty;

        switch (body)
        {
            case "a":
                answer +=
                    "Option A chosen in the first scenario. You found a torch and noticed that the cave wall has “Fear not the dead” written creepily in blood.\n";
                break;
            case "b":
                answer += "You chose option B in the first scenario.\n";
                break;
            case "c":
                answer +=
                    "Option C chosen in the first scenario. You feel a cold hand grab your ankle and drag you into the dark. You lose a health point.\n";
                break;
            case "d":
                answer +=
                    "Option D chosen in the first scenario. You waste a match and attract unwanted attention. You lose a health point.\n";
                break;
            default:
                answer += "Invalid choice in the first scenario.\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void GameTwo(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToLower();
        string answer = string.Empty;

        switch (body)
        {
            case "a":
                answer +=
                    "Option A chosen in the second scenario. The paper has a drawing of a spider and a note. Maybe the other player can use it, the note has written “Webby loves leg scratches”.\r\n\n";
                break;
            case "b":
                answer += "You chose option B in the second scenario.\n";
                break;
            case "c":
                answer +=
                    "Option C chosen in the second scenario. You destroy a valuable piece of information and the flame burns fast and you get burnt. You lose a health point.\n";
                break;
            case "d":
                answer +=
                    "Option D chosen in the second scenario. You choke on the paper, stupid. You lose a health point.\n";
                break;
            default:
                answer += "Invalid choice in Game One.\n";
                break;
        }

        SendResponse(res, answer);
    }
    public void GameThree(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToLower();
        string answer = string.Empty;

        switch (body)
        {
            case "a":
                answer +=
                    "Option A chosen in the third scenario. The frog is poisonous and you feel a sharp pain in your hand. You lose a health point.\n";
                break;
            case "b":
                answer +=
                    "You chose option B in the third scenario. The frog is disgusted and bites your lip, and you get poisoned by the bite. You lose a health point.\n";
                break;
            case "c":
                answer += "Option C chosen in the third scenario.\n";
                break;
            case "d":
                answer +=
                    "Option D chosen in the third scenario. The frog jumps away and reveals a note that seemed to be under it. You read “The float is reliable”...\n";
                break;
           
            default:
                answer += "Invalid choice in Game One.\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void GameFour(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToLower();
        string answer = string.Empty;

        switch (body)
        {
            case "a":
                answer +=
                    "Option A chosen in the forth scenario. You manage to balance and reach the top. As you climb, you notice that the symbols are a combination of numbers - xxxx and a sentence engraved: “Honor the Gods…”. Maybe they will come in handy later.\n";
                break;
            case "b":
                answer +=
                    "You chose option B in the forth scenario. The rope is infested with maggots and worms. They bite and burrow into your skin. You lose a health point.\n";
                break;

            case "c":
                answer +=
                    "Option C chosen in the forth scenario. The wall is coated with a slimy substance leaking out of the cracks that burns your flesh. You feel a searing pain in your hands and feet. You lose a health point.\n";
                break;
            case "d":
                answer +=
                    "Option D chosen in the forth scenario. You look around for another way. Maybe you’ll discover a hidden passage or a secret door. \n";
                break;
            default:
                answer += "Invalid choice in Game One.\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void GameFive(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToLower();
        string answer = string.Empty;

        switch (body)
        {
            case "a":
                answer +=
                    "Option A chosen in the fifth scenario. Removing some stones reveals a hidden passage. You see a faint light at the end of it.\n";
                break;
            case "b":
                answer += "You chose option B in the fifth scenario. \n";
                break;
            case "c":
                answer +=
                    "Option C chosen in the fifth scenario. You cause another collapse and get crushed by the stones. You lose a health point.\n";
                break;
            case "d":
                answer +=
                    "Option D chosen in the fifth scenario.  You hear a hissing sound and realize the stones are now covered with snakes. You lose a health point.\n";
                break;
            default:
                answer += "Invalid choice in Game One.\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void GameSix(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToLower();
        string answer = string.Empty;

        switch (body)
        {
            case "a":
                answer +=
                    "Option A chosen in the sixth scenario. By quietly standing still, the danger does not sense your presence. When passing you, it is sinisterly almost inaudible repeating “Honor the Gods, Honor the Gods…” \n";
                break;
            case "b":
                answer +=
                    "You chose option B in the sixth scenario. You trip over a skull and fall to the ground. The figure catches up to you and slashes you with its claws. You lose a health point.\n";
                break;
            case "c":
                answer +=
                    "Option C chosen in the sixth scenario. You grab a bone and swing it at the figure. It breaks the bone and grabs you by the neck and mangle you. [ YOU DIED ] \n";
                break;
            case "d":
                answer += "Option D chosen in the sixth scenario. You go back to previous scenario. \n";
                break;
            default:
                answer += "Invalid choice in Game One.\n";
                break;
        }
        SendResponse(res, answer);
    }

    // Player Two Storyline
    public void GameSeven(HttpListenerRequest req, HttpListenerResponse res) // Scenario One
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToLower();
        string answer = string.Empty;

        switch (body)
        {
            case "a":
                answer +=
                    "Option A chosen in the first scenario. Taking a closer look, you see the corpse has been mutilated and eaten by something. There is a note on the corpse’s chest pocket. The note has written “Beware of the red frog. It is poisonous but poking it will make it go away”.. Maybe the other player can use it to a public void the dangers that might unfold. \n";
                break;
            case "b":
                answer += "You chose option B in the first scenario.\n";
                break;
            case "c":
                answer +=
                    "Option C chosen in the first scenario. You disturb a swarm of flesh-eating insects that attack you. You lose a health point.\r\n \n";
                break;
            case "d":
                answer +=
                    "Option D chosen in the first scenario. You trip over a wire and trigger a trap that shoots arrows made of human bones at you. You lose a health point. \n";
                break;
            default:
                answer += "Invalid choice in Game One.\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void GameEight(HttpListenerRequest req, HttpListenerResponse res) // Scenario Two
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToLower();
        string answer = string.Empty;

        switch (body)
        {
            case "a":
                answer +=
                    "Option A chosen in the second scenario. The spider seems pleased with the scratches and purrs softly. It pulls a string on the web which opens a secret passage to continue. passing the spider it screeches and whispers “If it can not hear you, it can not see you” before disappearing into the depth. \n";
                break;
            case "b":
                answer +=
                    "You chose option B in the second scenario. The spider gets angry and spits venom at you. You lose a health point.\n";
                break;
            case "c":
                answer +=
                    "Option C chosen in the second scenario. The spider ignores you and wraps you in its web. You lose a health point.\n";
                break;
            case "d":
                answer += "Option D chosen in the second scenario. \n";
                break;
            default:
                answer += "Invalid choice in Game One.\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void GameNine(HttpListenerRequest req, HttpListenerResponse res) // Scenario Three
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToLower();
        string answer = string.Empty;

        switch (body)
        {
            case "a":
                answer += "Option A chosen in the third scenario. \n";
                break;
            case "b":
                answer +=
                    "You chose option B in the third scenario. You hide behind a pile of bones and see a shadowy figure pass by. It looks like a tall repulsing creature with horns and a wretched tail. You notice a ladder engraved on his back.\n";
                break;
            case "c":
                answer +=
                    "Option C chosen in the third scenario. You provoke the creature and it attacks you. You lose a health point. \n";
                break;
            case "d":
                answer +=
                    "Option D chosen in the third scenario. You get too close to the creature and it bites you. You lose a health point.\r\n\n";
                break;
            default:
                answer += "Invalid choice in Game One.\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void GameTen(HttpListenerRequest req, HttpListenerResponse res)
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToLower();
        string answer = string.Empty;

        switch (body)
        {
            case "a":
                answer +=
                    "Option A chosen in the fourth scenario.The raft is sturdy despite looking half done and takes you safely to the other side where you spot a withered sign saying: “Stop running and Follow The Light”... \n";
                break;
            case "b":
                answer +=
                    "You chose option B in the fourth scenario. Embarking the boat you notice little too late that it has holes. The boat starts to leak and sink rapidly. You try to swim back to the shore, but you feel something grab your leg and pull you down. You scream, but no one can hear you. You drowned. [ YOU DIED ] \n";
                break;
            case "c":
                answer +=
                    "Option C chosen in the fourth scenario. Swept away by the current, you hit your head on a rock and almost lose consciousness. You lose a health point. You wake up back on the shore.\n";
                break;
            case "d":
                answer +=
                    "Option D chosen in the fourth scenario. The waterfall is not a gentle shower, but a raging torrent. It slams you against the rocks and pushes you back to the shore. You are bruised, battered, and soaked. You lose a health point and your dignity. That was stupid.\n";
                break;
            default:
                answer += "Invalid choice in Game One.\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void GameEleven(HttpListenerRequest req, HttpListenerResponse res) // Scenario Five
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToLower();
        string answer = string.Empty;

        switch (body)
        {
            case "a":
                answer +=
                    "Option A chosen in the fifth scenario. By quietly standing still, the danger does not sense your presence. When passing you, it is sinisterly almost inaudible repeating “Honor the Gods, Honor the Gods…” \n";
                break;
            case "b":
                answer +=
                    "You chose option B in the fifth scenario. You trip over a skull and fall to the ground. The figure catches up to you and slashes you with its claws. You lose a health point.\n";
                break;
            case "c":
                answer +=
                    "Option C chosen in the fifth scenario. You grab a bone and swing it at the figure. It breaks the bone and grabs you by the neck and mangle you. [ YOU DIED ] \n";
                break;
            case "d":
                answer += "Option D chosen in the fifth scenario. \n";
                break;
            default:
                answer += "Invalid choice in Game One.\n";
                break;
        }
        SendResponse(res, answer);
    }
    public void GameTwelve(HttpListenerRequest req, HttpListenerResponse res) // Scenario Six
    {
        StreamReader reader = new StreamReader(req.InputStream, req.ContentEncoding);
        string body = reader.ReadToEnd().ToLower();
        string answer = string.Empty;

        switch (body)
        {
            case "a":
                answer +=
                    "Option A chosen in the sixth scenario. You light the candle, feeling a surge of gratitude from the gods. The wall behind you begins to rumble and quake. You turn around and see a large crack forming on the surface. A faint light seeps through the gap. You have discovered a hidden chamber that was guarded by the gods.\n";
                break;
            case "b":
                answer +=
                    "You chose option B in the sixth scenario. Ah yes, you have angered the Gods. Wretched screams close in and something bites you and leaves a blood wound. You lose a health point.\n";
                break;
            case "c":
                answer +=
                    "Option C chosen in the sixth scenario. Ah yes, you have angered the Gods. You feel a excruciating pain around your hand and drop the candle. You lose a health point.\n";
                break;
            case "d":
                answer += "Option D chosen in the sixth scenario. \n";
                break;
            default:
                answer += "Invalid choice in Game One.\n";
                break;
        }
        SendResponse(res, answer);
    }
    
    private void SendResponse(HttpListenerResponse response, string content)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(content);
        response.ContentType = "text/plain";
        response.StatusCode = (int)HttpStatusCode.OK;

        using (Stream output = response.OutputStream)
        {
            output.Write(buffer, 0, buffer.Length);
        }

        response.StatusCode = (int)HttpStatusCode.Created;
        response.Close();
    }
}