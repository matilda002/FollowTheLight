using Npgsql;
using FollowTheLightMain;
using System.Net;
using System.Text;

const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
await using var db = NpgsqlDataSource.Create(dbUri);

var databaseCreator = new DatabaseCreator(db);
var databasehelper = new DatabaseHelper(db);

//await databasehelper.ResetTables();
//await databaseCreator.CreateTables();
await databasehelper.PopulateStoryPointsTable();

bool listen = true;
int port = 3000;
HttpListener playerOne = new();
playerOne.Prefixes.Add($"http://localhost:{port}/");

Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
{
    Console.WriteLine("Interrupting cancel event");
    e.Cancel = true;
    listen = false;
};

try
{
    playerOne.Start();
    playerOne.BeginGetContext(HandleRequest, playerOne);
    while (listen) { }
}
finally
{
    playerOne.Stop();
}

void HandleRequest(IAsyncResult result)
{
    if (result.AsyncState is HttpListener listener)
    {
        HttpListenerContext context = listener.EndGetContext(result);

        Router(context);

        listener.BeginGetContext(HandleRequest, listener);
    }
}

void Router(HttpListenerContext context)
{
    HttpListenerRequest request = context.Request;
    HttpListenerResponse response = context.Response;

    switch (request.HttpMethod)
    {
        case ("GET"):
            switch (request.Url?.AbsolutePath)
            {
                case ("/intro"):
                    IntroGet(response);
                    break;
                case ("/game/player/1"):
                    GameGet(response);
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
                    NotFound(response);
                    break;
            }
            break;

        case ("POST"):
            switch (request.Url?.AbsolutePath)
            {
                case ("/game/player/1"):
                    GamePostOne(request, response);
                    break;
                case ("/game/player/2"):
                    GamePostTwo(request, response);
                    break;
                case ("/game/player/3"):
                    GamePostThree(request, response);
                    break;
                case ("/game/player/4"):
                    GamePostFour(request, response);
                    break;
                case ("/game/player/5"):
                    GamePostFive(request, response);
                    break;
                case ("/game/player/10"):
                    GamePostTen(request, response);
                    break;
            }
            break;

        default:
            NotFound(response);
            break;
    }
}

void IntroGet(HttpListenerResponse response)
{
    string introGet = "SELECT content FROM storypoints WHERE storypoint_id = 1";

    byte[] buffer = Encoding.UTF8.GetBytes(introGet);
    response.ContentType = "text/plain";
    response.StatusCode = (int)HttpStatusCode.OK;

    //foreach (byte b in buffer)
    //{
    //    response.OutputStream.WriteByte(b);
    //    Thread.Sleep(50);
    //}
    response.OutputStream.Close();
}

void GameGet(HttpListenerResponse response)
{
    string story = "You look around in the dark and find some matches.\nYou light up a match you find yourself in a cave. " +
                   "Before the light disappears you think you see a tunnel to your right and to your left.\nDo you go to the:\n\n" +
                   "A. Right tunnel\n" +
                   "B. Go forward into the darkness\n" +
                   "C. Left tunnel\n" +
                   "D. Tunnel behind you";
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

void GamePostOne(HttpListenerRequest req, HttpListenerResponse res)
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
                "Your walk into a wall. Go to the left tunnel and find a torch. -1hp\n";
            break;
        case ("C"):
        case ("c"):
            answer +=
                "You find a torch, might be useful later\n";
            break;
        case ("D"):
        case ("d"):
            answer +=
                "You step on a bear-trap. Go to the left tunnel and find a torch -1hp\n";
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
        Thread.Sleep(50);
    }

    //jumpscare
    string[] popup = File.ReadAllLines("popup-1.txt");
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
void GamePostTwo(HttpListenerRequest req, HttpListenerResponse res)
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
void GamePostThree(HttpListenerRequest req, HttpListenerResponse res)
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
void GamePostFour(HttpListenerRequest req, HttpListenerResponse res)
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
void GamePostFive(HttpListenerRequest req, HttpListenerResponse res)
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
void GamePostTen(HttpListenerRequest req, HttpListenerResponse res)
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

void NotFound(HttpListenerResponse res)
{
    res.StatusCode = (int)HttpStatusCode.NotFound;
    res.Close();
}