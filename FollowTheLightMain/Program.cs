using FollowTheLightMain;
using Npgsql;
using System.Net;

const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
var db = NpgsqlDataSource.Create(dbUri);

var dbCreator = new DatabaseCreator(db);
var dbHelper = new DatabaseHelper(db);

dbHelper.ResetTables();
dbCreator.CreateTables();

dbHelper.PopulateStoryPointIntro();
dbHelper.PopulateStoryPointsTable();
dbHelper.PopulateStoryPointEnding();
dbHelper.PopulatePuzzlesText();
dbHelper.PopulateImagesTable();

Console.WriteLine(@"
 ________       __   __                         _   __               _____      _         __       _                
|_   __  |     [  | [  |                       / |_[  |             |_   _|    (_)       [  |     / |_              
  | |_ \_|.--.  | |  | |  .--.   _   _   __   `| |-'| |--.  .---.     | |      __   .--./)| |--. `| |-'             
  |  _| / .'`\ \| |  | |/ .'`\ \[ \ [ \ [  ]   | |  | .-. |/ /__\\    | |   _ [  | / /'`\;| .-. | | |               
 _| |_  | \__. || |  | || \__. | \ \/\ \/ /    | |, | | | || \__.,   _| |__/ | | | \ \._//| | | | | |,   _   _   _  
|_____|  '.__.'[___][___]'.__.'   \__/\__/     \__/[___]|__]'.__.'  |________|[___].',__`[___]|__]\__/  (_) (_) (_) 
                                                                                 ( ( __))                           ");
Console.WriteLine(@"Follow the clues masked in whispers of terror, and uncover the chilling secrets that lurk in the darkness to get out");

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

Server server = new Server(db);
try
{
    listener.Start();
    listener.BeginGetContext(server.HandleRequest, listener);
    while (listen) { }
}
finally
{
    listener.Stop();
}

