using System.Net;
using System.Text;
namespace FollowTheLightMain;

public class GamePost
{
    public void Game(HttpListenerRequest req, HttpListenerResponse res)
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