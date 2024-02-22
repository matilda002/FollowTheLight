using System.Net;
using System.Text;
using Npgsql;
namespace FollowTheLightMain;

public class GetGame
{
    private readonly DatabaseHelper _dbHelper; 
    public GetGame(NpgsqlDataSource db)
    {
        _dbHelper = new(db);
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

    public void Intro(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(1);
        SendResponse(response, result);
    }
    public void End(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(14);
        SendResponse(response, result);
    }
    
    public void GameOne(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(2);
        SendResponse(response, result);
    }
    public void GameTwo(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(3);
        SendResponse(response, result);
    }
    public void GameThree(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(4);
        string resultImg = _dbHelper.GetImgContent(8);
        SendResponse(response, result + resultImg);
    }
    public void GameFour(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(5);
        SendResponse(response, result);
    }
    public void GameFive(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(6);
        SendResponse(response, result);
    }
    public void GameSix(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(7);
        SendResponse(response, result);
    }
    public void GameSeven(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(8);
        SendResponse(response, result);
    }
    public void GameEight(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(9);
        SendResponse(response, result);
    }
    public void GameNine(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(10);
        SendResponse(response, result);
    }
    public void GameTen(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(11);
        SendResponse(response, result);
    }
    public void GameEleven(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(12);
        SendResponse(response, result);
    }
    public void GameTwelve(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(13);
        SendResponse(response, result);
    }
    
    public void PuzzleOneP1(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(15);
        string resultImg = _dbHelper.GetImgContent(9);
        SendResponse(response, result + resultImg);
    }
    public void PuzzleTwoP1(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(16);
        string resultImg = _dbHelper.GetImgContent(10);
        SendResponse(response, result + resultImg);
    }
    public void PuzzleThreeP1(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(17);
        string resultImg = _dbHelper.GetImgContent(11);
        SendResponse(response, result + resultImg);
    }
    public void PuzzleOneP2(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(18);
        string resultImg = _dbHelper.GetImgContent(12);
        SendResponse(response, result + resultImg);
    }
    public void PuzzleTwoP2(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(19);
        string resultImg = _dbHelper.GetImgContent(13);
        SendResponse(response, result + resultImg);
    }
    public void PuzzleThreeP2(HttpListenerResponse response)
    {
        string result = _dbHelper.GetStoryPointContent(20);
        string resultImg = _dbHelper.GetImgContent(14);
        SendResponse(response, result + resultImg);
    } 
}