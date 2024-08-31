using DiskSpaceAnalyzerLib.Models;
using SQLite;

namespace DiskSpaceAnalyzerLib.Databases;

internal static class Database
{
    private static SQLiteAsyncConnection? _connection;
    public static SQLiteAsyncConnection Connection
    {
        get
        {
            if (_connection is null)
            {
                _connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                _connection.CreateTableAsync<AnalyzedFile>().Wait();
                _connection.CreateTableAsync<AnalyzedDirectory>().Wait();
            }
            return _connection;
        }
    }

}