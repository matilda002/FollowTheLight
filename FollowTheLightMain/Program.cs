using Npgsql;
using FollowTheLightMain;

const string dbUri = "Host=localhost;Port=5455;Username=postgres;Password=postgres;Database=followthelightdb;";
await using var db = NpgsqlDataSource.Create(dbUri);