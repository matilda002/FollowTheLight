using System.Net;
using System.Text;
using System.Net.Sockets;

bool listen = true;

Console.CancelKeyPress += delegate(object? sender, ConsoleCancelEventArgs e)
{
    Console.WriteLine("Interrupting cancel event");
    e.Cancel = true;
    listen = false;
};

int port = 3000;
listen = true;
string? game = string.Empty;

HttpListener playerOne = new();
playerOne.Prefixes.Add($"http://localhost:{port}/playerOne/"); // <host> kan t.ex. vara 127.0.0.1, 0.0.0.0, ...

try
{
    playerOne.Start();
    playerOne.BeginGetContext(new AsyncCallback(HandleRequest), playerOne);
    while (listen) { };

}
finally
{
    playerOne.Stop();
}

void HandleRequest(IAsyncResult result)
{
    if (result.AsyncState is HttpListener playerOne)
    {
        HttpListenerContext context = playerOne.EndGetContext(result);

        Router(context);
        
        playerOne.BeginGetContext(new AsyncCallback(HandleRequest), playerOne);
    }

    void Router(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        switch (request.HttpMethod, request.Url?.AbsolutePath)
        {
            case ("GET", "/intro"):
                IntroGet(response);
                break;
            case ("GET", "/game"):
                GameGet(response);
                break;
            default:
                NotFound(response);
                break;
        }
    }
    void IntroGet(HttpListenerResponse response)
    {
        string message =
            $@"You've woken up in darkness with no past memories. It's cold and when you scream for help it echoes...
You hear a faint voice coming from a device on the ground. You pick it up and someone responds with 'Who is this? Where am I?'.
After a while they realise no-one knows how they got there. All they know is they have to escape this place through working together..."; // byt ut till vilken text som ska skickas tillbaka
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        response.ContentType = "text/plain";
        response.StatusCode = (int)HttpStatusCode.OK;

        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }
    
    void GameGet(HttpListenerResponse response)
    {
        string message = "\n\ngame one";
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        response.ContentType = "text/plain";
        response.StatusCode = (int)HttpStatusCode.OK;

        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }
    
    void NotFound(HttpListenerResponse res)
    {
        res.StatusCode = (int)HttpStatusCode.NotFound;
        res.Close();
    }
    
}
