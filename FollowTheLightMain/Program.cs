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
                    GameTwoGet(response);
                    break;
                case ("/game/player/3"):
                    GameThreeGet(response);
                    break;
                case ("/game/player/4"):
                    GameFourGet(response);
                    break;
                case ("/game/player/5"):
                    GameFiveGet(response);
                    break;
                case ("/game/player/10"):
                    GameTenGet(response);
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
                    GameTwoPost(request, response);
                    break;
                case ("/game/player/3"):
                    GameThreePost(request, response);
                    break;
                case ("/game/player/4"):
                    GameFourPost(request, response);
                    break;
                case ("/game/player/5"):
                    GameFivePost(request, response);
                    break;
                case ("/game/player/10"):
                    GameTenPost(request, response);
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

void GameTwoGet(HttpListenerResponse response)
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
void GameThreeGet(HttpListenerResponse response)
{
    string story = "You see a red frog sitting on a rock. It looks friendly even if there's bones around it..." +
                     "Do you:\n\n" +
                     "A. Burn it with the torch\n" +
                     "B. Walk past the frog\n" +
                     "C. Pick it up\n" +
                     "D. Poke it with a bone from the ground";
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
void GameFourGet(HttpListenerResponse response)
{
    string story = "You hear a sinister noise behind you..." +
                     "Do you:\n\n" +
                     "A. Make a shushing noise\n" +
                     "B. Hide behind something\n" +
                     "C. Look back and investiagte the noise\n" +
                     "D. Scream and run";
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
void GameFiveGet(HttpListenerResponse response)
{
    string story = "You find a wall, you need to climb over it...." +
                     "Do you:\n\n" +
                     "A. Climb the slippery wall\n" +
                     "B. Look around the area\n" +
                     "C. Climb the rope\n" +
                     "D. Take the ladder with missing steps";
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
void GameTenGet(HttpListenerResponse response)
{
    string story = "You found the exit.... but you hear someone screaming in the cave...." +
                     "Do you:\n\n" +
                     "A. Leave\n" +
                     "B. Go back\n";

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
void GameTwoPost(HttpListenerRequest req, HttpListenerResponse res)
{
    StreamReader reader = new(req.InputStream, req.ContentEncoding);
    string body = reader.ReadToEnd();
    string answer = string.Empty;

    switch (body)
    {
        case ("A"):
        case ("a"):
            answer +=
                "You almost chocked to death, stupid. -1hp\n";
            break;
        case ("B"):
        case ("b"):
            answer +=
                "Nothing happens, might regret that later.\n";
            break;
        case ("C"):
        case ("c"):
            answer +=
                "You burn yourself trying to light it up.-1hp\n";
            break;
        case ("D"):
        case ("d"):
            answer +=
                "There's writing on the paper in blood \"DO NOT TRUST THE FROG\"\n";
            break;
        default:
            answer +=
                "That option does not exist\n";
            break;
    }

    byte[] buffer = Encoding.UTF8.GetBytes(answer);
    res.ContentType = "text/plain";
    res.StatusCode = (int)HttpStatusCode.OK;

    foreach (byte b in buffer)
    {
        res.OutputStream.WriteByte(b);
        Thread.Sleep(50);
    }

    //jumpscare
    string[] popup = File.ReadAllLines("popup-2.txt");
    List<byte> byteList = new List<byte>();
    foreach (string line in popup)
    {
        byte[] lineBytes = Encoding.UTF8.GetBytes(line + "\n");
        byteList.AddRange(lineBytes);
    }

    byte[] test = byteList.ToArray();
    foreach (byte x in test)
    {
        res.OutputStream.WriteByte(x);
    }

    res.OutputStream.Close();

    Console.WriteLine($"Player answered the following to question one: {body}");

    res.StatusCode = (int)HttpStatusCode.Created;
    res.Close();
}
void GameThreePost(HttpListenerRequest req, HttpListenerResponse res)
{
    StreamReader reader = new(req.InputStream, req.ContentEncoding);
    string body = reader.ReadToEnd();
    string answer = string.Empty;

    switch (body)
    {
        case ("A"):
        case ("a"):
            answer +=
                "It screams in agony while it burns to death \n";
            break;
        case ("B"):
        case ("b"):
            answer +=
                "You walk past the frog and continue on your path \n";
            break;
        case ("C"):
        case ("c"):
            answer +=
                "The frog is poisonous, but it jumps out of your hand before damaging you critically. -1hp\n";
            break;
        case ("D"):
        case ("d"):
            answer +=
                "You poke it with a stick, it makes a squeaking noise.\n";
            break;
        default:
            answer +=
                "That option does not exist\n";
            break;
    }

    byte[] buffer = Encoding.UTF8.GetBytes(answer);
    res.ContentType = "text/plain";
    res.StatusCode = (int)HttpStatusCode.OK;

    foreach (byte b in buffer)
    {
        res.OutputStream.WriteByte(b);
        Thread.Sleep(50);
    }

    //jumpscare
    string[] popup = File.ReadAllLines("popup-3.txt");
    List<byte> byteList = new List<byte>();
    foreach (string line in popup)
    {
        byte[] lineBytes = Encoding.UTF8.GetBytes(line + "\n");
        byteList.AddRange(lineBytes);
    }

    byte[] test = byteList.ToArray();
    foreach (byte x in test)
    {
        res.OutputStream.WriteByte(x);
    }

    Console.WriteLine($"Player answered the following to question one: {body}");
    res.OutputStream.Close();

    res.StatusCode = (int)HttpStatusCode.Created;
    res.Close();
}
void GameFourPost(HttpListenerRequest req, HttpListenerResponse res)
{
    StreamReader reader = new(req.InputStream, req.ContentEncoding);
    string body = reader.ReadToEnd();
    string answer = string.Empty;

    switch (body)
    {
        case ("A"):
        case ("a"):
            answer +=
                "You aggroad the monster and it attacks you. -1hp\n";
            break;
        case ("B"):
        case ("b"):
            answer +=
                "You hide behind a rock, the monster passes you and walks away.\n";
            break;
        case ("C"):
        case ("c"):
            answer +=
                "You bump into the monster and it scratches your face. -1hp\n";
            break;
        case ("D"):
        case ("d"):
            answer +=
                "Your screaming exposes your position and it attacks you, you couldn't outrun the monster. -1hp\n";
            break;
        default:
            answer +=
                "That option does not exist\n";
            break;
    }

    byte[] buffer = Encoding.UTF8.GetBytes(answer);
    res.ContentType = "text/plain";
    res.StatusCode = (int)HttpStatusCode.OK;

    foreach (byte b in buffer)
    {
        res.OutputStream.WriteByte(b);
        Thread.Sleep(50);
    }
    res.OutputStream.Close();

    Console.WriteLine($"Player answered the following to question one: {body}");

    res.StatusCode = (int)HttpStatusCode.Created;
    res.Close();
}
void GameFivePost(HttpListenerRequest req, HttpListenerResponse res)
{
    StreamReader reader = new(req.InputStream, req.ContentEncoding);
    string body = reader.ReadToEnd();
    string answer = string.Empty;

    switch (body)
    {
        case ("A"):
        case ("a"):
            answer +=
                "You slipp and fall. -1hp\n";
            break;
        case ("B"):
        case ("b"):
            answer +=
                "You find a secret passage, but you notice it was a dead end by walking into a wall. -1hp\n";
            break;
        case ("C"):
        case ("c"):
            answer +=
                "The rope snaps in half and you fall. -1hp\n";
            break;
        case ("D"):
        case ("d"):
            answer +=
                "You maanage to climb over the wall.\n";
            break;
        default:
            answer +=
                "That option does not exist\n";
            break;
    }

    byte[] buffer = Encoding.UTF8.GetBytes(answer);
    res.ContentType = "text/plain";
    res.StatusCode = (int)HttpStatusCode.OK;

    foreach (byte b in buffer)
    {
        res.OutputStream.WriteByte(b);
        Thread.Sleep(50);
    }
    res.OutputStream.Close();

    Console.WriteLine($"Player answered the following to question one: {body}");

    res.StatusCode = (int)HttpStatusCode.Created;
    res.Close();
}
void GameTenPost(HttpListenerRequest req, HttpListenerResponse res)
{
    StreamReader reader = new(req.InputStream, req.ContentEncoding);
    string body = reader.ReadToEnd();
    string answer = string.Empty;

    switch (body)
    {
        case ("A"):
        case ("a"):
            answer +=
                "You decided to leave - The End\n";
            break;
        case ("B"):
        case ("b"):
            answer +=
                "You decided to go back deep into the dark cave and you notice that the screaming stopped... And turned into growling... - The End\n";
            break;
        default:
            answer +=
                "That option does not exist\n";
            break;
    }

    byte[] buffer = Encoding.UTF8.GetBytes(answer);
    res.ContentType = "text/plain";
    res.StatusCode = (int)HttpStatusCode.OK;

    foreach (byte b in buffer)
    {
        res.OutputStream.WriteByte(b);
        Thread.Sleep(50);
    }
    res.OutputStream.Close();

    Console.WriteLine($"Player answered the following to question one: {body}");

    res.StatusCode = (int)HttpStatusCode.Created;
    res.Close();
}

Task NotFound(HttpListenerResponse res)
{
    res.StatusCode = (int)HttpStatusCode.NotFound;
    res.Close();
    return Task.CompletedTask;
}