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
                default:
                    NotFound(response);
                    break; 
            }
            break;
        
        case ("POST"):
            switch (request.Url?.AbsolutePath)
            {
                case ("/game/player/1"):
                    GamePost(request, response);
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

    response.OutputStream.Write(buffer, 0, buffer.Length);
    response.OutputStream.Close();
}

void GamePost(HttpListenerRequest req, HttpListenerResponse res)
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

    res.OutputStream.Write(buffer, 0, buffer.Length);
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

    response.OutputStream.Write(buffer, 0, buffer.Length);
    response.OutputStream.Close();
}
// A. You almost chocked to death, stupid -1hp
// B. Nothing happens, might regret that later
// C. You burn yourself trying to light it up
// D. There's writing on the paper in blood "DO NOT TRUST THE FROG"

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

    response.OutputStream.Write(buffer, 0, buffer.Length);
    response.OutputStream.Close();
}
// A. It screams in agony while it burns to death
// B. You walk past the frog and continue on your path
// C. The frog is poisonous, but it jumps out of your hand before damaging you critically. -1hp
// D. The frog makes a squeaking sound 



// player two

void NotFound(HttpListenerResponse res)
{
    res.StatusCode = (int)HttpStatusCode.NotFound;
    res.Close();
}