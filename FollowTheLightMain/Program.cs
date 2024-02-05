using System.Net;
using System.Text;
using System.Threading;


bool listen = true;

Console.CancelKeyPress += delegate(object? sender, ConsoleCancelEventArgs e)
{
    Console.WriteLine("Interrupting cancel event");
    e.Cancel = true;
    listen = false;
};

int port = 3000;
HttpListener playerOne = new();
playerOne.Prefixes.Add($"http://localhost:{port}/"); // <host> kan t.ex. vara 127.0.0.1, 0.0.0.0, ...

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
            }
            break;
        
        default:
            NotFound(response);
            break;
    }
}

void IntroGet(HttpListenerResponse response)
{
    string intro = @"You've woken up in darkness with no past memories. It's cold and when you scream for help it echoes...
You hear a faint voice coming from a device on the ground. You pick it up and someone responds with 'Who is this? Where am I?'.
After a while they realise no-one knows how they got there. All they know is they have to escape this place through working together...";
    
    byte[] buffer = Encoding.UTF8.GetBytes(intro);
    response.ContentType = "text/plain";
    response.StatusCode = (int)HttpStatusCode.OK;
    
    foreach (byte b in buffer)
    {
        response.OutputStream.WriteByte(b);
        Thread.Sleep(50);
    }
    response.OutputStream.Close();
}

void GameGet(HttpListenerResponse response)
{
    string story = "You look around in the dark and find some matches.\nYou light up a match you find yourself in a cave. " +
                   "Before the light disappears you think you see a tunnel to your right and to your left.\nDo you go to the:\n\n"+
                   "A. Right tunnel\n"+
                   "B. Go forward into the darkness\n"+
                   "C. Left tunnel\n"+
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

void GamePostOne(HttpListenerRequest req, HttpListenerResponse res)
{
    StreamReader reader = new(req.InputStream, req.ContentEncoding);
    string body = reader.ReadToEnd();
    string answer = string.Empty;

    switch (body)
    {
        case ("A"): case ("a"):
            answer +=
                "You find teeth looks like it's human. You turn back around, take the left tunnel and find a torch.";
            break;
        case ("B"): case ("b"):
            answer +=
                "Your walk into a wall. Go to the left tunnel and find a torch. -1hp";
            break;
        case ("C"): case ("c"):
            answer +=
                "You find a torch, might be useful later";
            break;
        case ("D"): case ("d"):
            answer +=
                "You step on a bear-trap. Go to the left tunnel and find a torch -1hp";
            break;
        default:
            answer +=
                "That option does not exist";
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

void GameTwoGet(HttpListenerResponse response)
{
    string story = "While you're walking through the tunnel you feel a piece paper under your feet, and pick it up." +
                     "Do you:\n\n"+
                     "A. Eat it\n"+
                     "B. Throw it\n"+
                     "C. Burn it\n"+
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
                "You almost chocked to death, stupid. -1hp";
            break;
        case ("B"):
        case ("b"):
            answer +=
                "Nothing happens, might regret that later.";
            break;
        case ("C"):
        case ("c"):
            answer +=
                "You burn yourself trying to light it up.-1hp";
            break;
        case ("D"):
        case ("d"):
            answer +=
                "There's writing on the paper in blood \"DO NOT TRUST THE FROG\"";
            break;
        default:
            answer +=
                "That option does not exist";
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
void GameThreeGet(HttpListenerResponse response)
{
    string story = "You see a red frog sitting on a rock. It looks friendly even if there's bones around it..." +
                     "Do you:\n\n"+
                     "A. Burn it with the torch\n"+
                     "B. Walk past the frog\n"+
                     "C. Pick it up\n"+
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


// player two

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
                "It screams in agony while it burns to death";
            break;
        case ("B"):
        case ("b"):
            answer +=
                "You walk past the frog and continue on your path";
            break;
        case ("C"):
        case ("c"):
            answer +=
                "The frog is poisonous, but it jumps out of your hand before damaging you critically. -1hp";
            break;
        case ("D"):
        case ("d"):
            answer +=
                "You step on a bear-trap. Go to the left tunnel and find a torch -1hp";
            break;
        default:
            answer +=
                "The frog makes a squeaking sound";
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
                "You aggroad the monster and it attacks you. -1hp";
            break;
        case ("B"):
        case ("b"):
            answer +=
                "You hide behind a rock, the monster passes you and walks away.";
            break;
        case ("C"):
        case ("c"):
            answer +=
                "You bump into the monster and it scratches your face. -1hp";
            break;
        case ("D"):
        case ("d"):
            answer +=
                "You step on a bear-trap. Go to the left tunnel and find a torch -1hp";
            break;
        default:
            answer +=
                "Your screaming exposes your position and it attacks you, you couldn't outrun the monster. -1hp";
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