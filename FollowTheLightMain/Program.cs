using Npgsql;
using FollowTheLightdb;
using System.Runtime.CompilerServices;


const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
await using var db = NpgsqlDataSource.Create(dbUri);

var databaseCreator = new DatabaseCreator(db);
var databasehelper = new DatabaseHelper(db);

//await databasehelper.ResetTables();
await databaseCreator.CreateTables();
