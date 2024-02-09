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
    listener.BeginGetContext(new AsyncCallback(server.HandleRequest), listener);
    while (listen) { }
}
finally
{
    listener.Stop();
}