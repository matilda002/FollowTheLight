using Npgsql;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FollowTheLightdb;
public class DatabaseHelper
{
    private readonly NpgsqlDataSource _db;
    public DatabaseHelper(NpgsqlDataSource db)
    {
        _db = db;
    }
    public async Task ResetTables()
    {
        Console.WriteLine("Resetting tables...\n");
        const string query = "drop schema public cascade; create schema public;";
        await _db.CreateCommand(query).ExecuteNonQueryAsync();
    }

}   