using System.Net;
using System.Text;
using Npgsql;
namespace FollowTheLightMain;

public class GameGet
{
    private readonly DatabaseHelper _dbHelper; 
    public GameGet(NpgsqlDataSource db)
    {
        _dbHelper = new(db);
    } 
    
    
    public void Game(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(2);
        SendResponse(response, result);
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