using FollowTheLightMain;
using Npgsql;
using System.Net;

const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
var db = NpgsqlDataSource.Create(dbUri);

var dbCreator = new DatabaseCreator(db);
var dbHelper = new DatabaseHelper(db);

//dbHelper.ResetTables();
dbCreator.CreateTables();
dbHelper.PopulateStoryPointsTable();
dbHelper.PopulateStoryPointsTableTwo();
dbHelper.PopulateImagesTable();

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

Server server = new(db);
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