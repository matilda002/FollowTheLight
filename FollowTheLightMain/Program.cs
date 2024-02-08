using FollowTheLightMain;
using Npgsql;
using System.Net;
using System.Text;

const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
await using var db = NpgsqlDataSource.Create(dbUri);

var dbCreator = new DatabaseCreator(db);
var dbHelper = new DatabaseHelper(db);

await dbHelper.ResetTables();
await dbCreator.CreateTables();
await dbHelper.PopulateStoryPointsTable();
await dbHelper.PopulateStoryPathsTable();

bool listen = true;
int port = 3000;
HttpListener listener = new();
listener.Prefixes.Add($"http://localhost:{port}/");

Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
{
    Console.WriteLine("Interrupting cancel event");
    e.Cancel = true;
    listen = false;
};

try
{
    listener.Start();
    listener.BeginGetContext(HandleRequest, listener);
    while (listen) { }
}
finally
{
    listener.Stop();
}

async void HandleRequest(IAsyncResult result)
{
    if (result.AsyncState is HttpListener requestListener)
    {
        HttpListenerContext context = requestListener.EndGetContext(result);

        await Router(context);

        requestListener.BeginGetContext(HandleRequest, requestListener);
    }
}

async Task Router(HttpListenerContext context)
{
    HttpListenerRequest request = context.Request;
    HttpListenerResponse response = context.Response;

    switch (request.HttpMethod)
    {
        case ("GET"):
            switch (request.Url?.AbsolutePath)
            {
                case ("/intro"):
                    await IntroGet(response);
                    break;
                case ("/game/player/1"):
                    await GameOneGet(response);
                    break;
                case ("/game/player/2"):
                    await GameTwoGet(response);
                    break;
                default:
                    await NotFound(response);
                    break;
            }
            break;

        case ("POST"):
            switch (request.Url?.AbsolutePath)
            {
                case ("/game/player/1"):
                    await GameOnePost(request, response);
                    break;
                case ("/game/player/2"):
                    await GameTwoPost(request, response);
                    break;
                default:
                    await NotFound(response);
                    break;
            }
            break;

        default:
            await NotFound(response);
            break;
    }
}


async Task IntroGet(HttpListenerResponse response)
{
    string resultIntro = string.Empty;
    Console.WriteLine("Printing out 'Intro' from storypoints to player...");

    const string qIntroGet= "select content from storypoints where storypoint_id = 1";
    await using var reader = await db.CreateCommand(qIntroGet).ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        resultIntro = reader.GetString(0);
    }

    byte[] buffer = Encoding.UTF8.GetBytes(resultIntro);
    response.ContentType = "text/plain";
    response.StatusCode = (int)HttpStatusCode.OK;
    
    foreach (byte b in buffer)
    {
        response.OutputStream.WriteByte(b);
        Thread.Sleep(35);
    }
    response.OutputStream.Close();
}

async Task GameOneGet(HttpListenerResponse response)
{
    string resultStoryOne = string.Empty;
    Console.WriteLine("Printing out 'Story One' from storypoints to player...");

    const string qStoryOne = "select content from storypoints where storypoint_id = 2";
    await using var reader = await db.CreateCommand(qStoryOne).ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        resultStoryOne = reader.GetString(0);
    }
    
    byte[] buffer = Encoding.UTF8.GetBytes(resultStoryOne);
    response.ContentType = "text/plain";
    response.StatusCode = (int)HttpStatusCode.OK;

    foreach (byte b in buffer)
    {
        response.OutputStream.WriteByte(b);
        Thread.Sleep(35);
    }
    response.OutputStream.Close();
}
async Task GameTwoGet(HttpListenerResponse response)
{
    string story = "While you're walking through the tunnel you feel a piece paper under your feet, and pick it up." +
                     "Do you:\n\n" +
                     "A. Eat it\n" +
                     "B. Throw it\n" +
                     "C. Burn it\n" +
                     "D. Read it";
    byte[] buffer = Encoding.UTF8.GetBytes(story);
    response.ContentType = "text/plain";
    response.StatusCode = (int)HttpStatusCode.OK;

    foreach (byte b in buffer)
    {
        response.OutputStream.WriteByte(b);
        Thread.Sleep(50);
    }
    response.OutputStream.Close();
}

async Task GameOnePost(HttpListenerRequest req, HttpListenerResponse res)
{
    StreamReader reader = new(req.InputStream, req.ContentEncoding);
    string body = reader.ReadToEnd();
    string answer = string.Empty;

    switch (body)
    {
        case ("A"):
        case ("a"):
            answer +=
                "You find teeth looks like it's human. You turn back around, take the left tunnel and find a torch.\n";
            break;
        case ("B"):
        case ("b"):
            answer +=
                "You find a torch, might be useful later\n";
            break;
        default:
            answer +=
                "That option does not exist\n";
            break;
    }

    //user choice
    byte[] buffer = Encoding.UTF8.GetBytes(answer);
    res.ContentType = "text/plain";
    res.StatusCode = (int)HttpStatusCode.OK;
    foreach (byte b in buffer)
    {
        res.OutputStream.WriteByte(b);
        Thread.Sleep(35);
    }
    
    res.StatusCode = (int)HttpStatusCode.Created;
    res.Close();
}
async Task GameTwoPost(HttpListenerRequest req, HttpListenerResponse res)
{
    StreamReader reader = new(req.InputStream, req.ContentEncoding);
    string body = reader.ReadToEnd();
    string answer = string.Empty;

    switch (body)
    {
        case ("A"):
        case ("a"):
            answer +=
                "You find teeth looks like it's human. You turn back around, take the left tunnel and find a torch.\n";
            break;
        case ("B"):
        case ("b"):
            answer +=
                "You find a torch, might be useful later\n";
            break;
        default:
            answer +=
                "That option does not exist\n";
            break;
    }

    //user choice
    byte[] buffer = Encoding.UTF8.GetBytes(answer);
    res.ContentType = "text/plain";
    res.StatusCode = (int)HttpStatusCode.OK;
    foreach (byte b in buffer)
    {
        res.OutputStream.WriteByte(b);
        Thread.Sleep(35);
    }
    
    res.StatusCode = (int)HttpStatusCode.Created;
    res.Close();
}

Task NotFound(HttpListenerResponse res)
{
    res.StatusCode = (int)HttpStatusCode.NotFound;
    res.Close();
    return Task.CompletedTask;
}